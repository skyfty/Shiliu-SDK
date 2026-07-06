using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiliu.Oral.Sdk.Abstractions.Exceptions;
using Shiliu.Oral.Sdk.Abstractions.Enums;
using Shiliu.Oral.Sdk.Abstractions.Auth;
using Shiliu.Oral.Sdk.Abstractions.Models;
using Shiliu.Oral.Sdk.Abstractions.Services;
using Shiliu.Oral.Sdk.Infrastructure.Http;

namespace Shiliu.Oral.Sdk.Infrastructure.Services
{
    /// <summary>
    /// Speech oral evaluation service using HMAC auth.
    /// Adapted from the original SoeClient.
    /// </summary>
    public class SpeechEvaluationService : HttpApiClientBase, ISpeechEvaluationService
    {
        private readonly string _appKey;
        private readonly string _appSecret;

        public SpeechEvaluationService(
            IHttpClientFactory httpClientFactory,
            ILogger<SpeechEvaluationService> logger,
            ICurrentLanguageProvider currentLanguageProvider,
            string appKey = "aioral",
            string appSecret = "IlNkMpXGnUUZRZlq")
            : base(httpClientFactory, "SoeClient", logger, currentLanguageProvider)
        {
            _appKey = appKey;
            _appSecret = appSecret;
        }

        public async Task<long> GetTimestampAsync(CancellationToken ct = default)
        {
            var content = await GetAsync("/soe/api/t", ct);
            var result = ParseSoeResponse<long>(content);
            if (result.Success) return result.Data;
            throw new ShiliuOralException($"获取服务器时间戳失败: {result.Info}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<AcquireTokenResponse> AcquireV3Async(AcquireTokenRequest request, CancellationToken ct = default)
        {
            var content = await PostAsync("/soe/api/acquire/v3", JsonSerializer.Serialize(request), ct);
            var result = ParseSoeResponse<AcquireTokenResponse>(content);
            if (result.Success) return result.Data;
            throw new ShiliuOralException($"获取令牌v3失败: {result.Info}", category: ShiliuOralErrorCategory.ServerError);
        }

        private static readonly JsonSerializerOptions _camelCaseOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<EvaluationResult> SubmitEvaluationAsync(SubmitEvaluationRequest request, CancellationToken ct = default)
        {
            var auth = await GenerateAuthHeaderAsync(ct);
            var client = HttpClientFactory.CreateClient(ClientName);
            var url = new Uri(client.BaseAddress, "/soe/api/correct").ToString();

            // requestJson 是字符串，服务端期望收到字符串（java.lang.String），直接序列化整个 request
            var requestBody = JsonSerializer.Serialize(request, _camelCaseOptions);

            Serilog.Log.Information("SubmitEvaluation 请求体: {Body}", requestBody);
            var httpContent = new System.Net.Http.StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url) { Content = httpContent };
            httpRequest.Headers.Add("auth", auth);
            httpRequest.Headers.Add("lang", request.Lang.ToString());
            var response = await client.SendAsync(httpRequest, ct);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Serilog.Log.Information("SubmitEvaluation 响应: {Content}", content);
            using var doc = JsonDocument.Parse(content);
            bool success = doc.RootElement.GetProperty("success").GetBoolean();
            if (!success)
            {
                var msg = doc.RootElement.GetProperty("info").GetString();
                throw new ShiliuOralException($"实时评测失败: {msg}", category: ShiliuOralErrorCategory.ServerError);
            }
            var data = doc.RootElement.GetProperty("data");
            var correctId = data.GetProperty("correctId").GetString();
            // 服务端可能同步返回 result 字段（status=8），直接解析
            if (data.TryGetProperty("result", out var resultProp) && resultProp.ValueKind == JsonValueKind.String)
            {
                var resultJson = resultProp.GetString();
                if (!string.IsNullOrEmpty(resultJson))
                {
                    var evalResult = JsonSerializer.Deserialize<EvaluationResult>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (evalResult != null)
                    {
                        evalResult.CorrectId = correctId;
                        return evalResult;
                    }
                }
            }
            // 服务端未同步返回结果，返回只含 correctId 的空结果，由调用方轮询
            return new EvaluationResult { CorrectId = correctId };
        }

        public async Task<EvaluationResult> QueryEvaluationResultAsync(string correctId, CancellationToken ct = default)
        {
            var auth = await GenerateAuthHeaderAsync(ct);
            var client = HttpClientFactory.CreateClient(ClientName);
            var url = new Uri(client.BaseAddress, $"/soe/api/result/{correctId}").ToString();

            Logger?.LogDebug("GET {Url}", url);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequest.Headers.Add("auth", auth);
  
            var response = await client.SendAsync(httpRequest, ct);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Logger?.LogDebug("GET {Url} OK", url);

            Serilog.Log.Information("QueryEvaluation 原始响应: {Content}", responseContent);
            var result = ParseSoeResponse<QueryResultRaw>(responseContent);
            Serilog.Log.Information("QueryEvaluation parsed: Success={S}, DataNull={DN}, Status={St}, ResultNull={RN}",
                result?.Success, result?.Data == null, result?.Data?.status, string.IsNullOrEmpty(result?.Data?.result));
            if (result.Success && result.Data != null)
            {
                if (result.Data.status == 2 && !string.IsNullOrEmpty(result.Data.result))
                {
                    Serilog.Log.Information("QueryEvaluation result JSON: {J}", result.Data.result);
                    return JsonSerializer.Deserialize<EvaluationResult>(result.Data.result, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return new EvaluationResult { CorrectId = correctId };
            }
            // record not exist 或其他"未就绪"状态，视为处理中，不抛异常
            return new EvaluationResult { CorrectId = correctId };
        }

        // Cached server-local time offset (ms). Synced once, reused for all subsequent requests.
        private long _timeOffsetMs = long.MinValue;
        private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);

        private async Task<long> GetCachedTimestampAsync(CancellationToken ct = default)
        {
            if (_timeOffsetMs == long.MinValue)
            {
                await _syncLock.WaitAsync(ct);
                try
                {
                    if (_timeOffsetMs == long.MinValue)
                    {
                        long serverTime = await GetTimestampAsync(ct);
                        long localTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        _timeOffsetMs = serverTime - localTime;
                    }
                }
                finally { _syncLock.Release(); }
            }
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + _timeOffsetMs;
        }

        public async Task<string> GenerateAuthHeaderAsync(CancellationToken ct = default)
        {
            long timestamp = await GetCachedTimestampAsync(ct);
            string ts = timestamp.ToString();
            string tsFirst7 = ts.Substring(0, 7);
            string tsLast10 = ts.Substring(3, 10);
            string tsLast6 = ts.Substring(ts.Length - 6);

            string md5Input = $"{_appKey}:{_appSecret}:{tsLast10}";
            string md5Hash = GetMd5Hash(md5Input);

            string rawAuth = $"{_appKey}:{tsFirst7}{md5Hash}{tsLast6}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(rawAuth));
        }

        private static string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (byte b in hashBytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
       

        private class QueryResultRaw
        {
            public long status { get; set; }
            public string result { get; set; }
        }
    }
}