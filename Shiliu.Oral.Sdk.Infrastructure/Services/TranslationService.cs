using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiliu.Oral.Sdk.Abstractions.Auth;
using Shiliu.Oral.Sdk.Abstractions.Exceptions;
using Shiliu.Oral.Sdk.Abstractions.Enums;
using Shiliu.Oral.Sdk.Abstractions.Models;
using Shiliu.Oral.Sdk.Abstractions.Services;
using Shiliu.Oral.Sdk.Infrastructure.Http;

namespace Shiliu.Oral.Sdk.Infrastructure.Services
{
    /// <summary>
    /// Translation service implementation — owns all translation-related HTTP endpoints.
    /// </summary>
    public class TranslationService : HttpApiClientBase, ITranslationService
    {
        public TranslationService(IHttpClientFactory httpClientFactory, ILogger<TranslationService> logger, ICurrentLanguageProvider currentLanguageProvider = null)
            : base(httpClientFactory, "AiTalkClient", logger, currentLanguageProvider)
        {
        }

        public async Task<TextTranslationResult> TranslateTextAsync(TextTranslationRequest request, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/translate/text", JsonSerializer.Serialize(new { originalLanguage = request.OriginalLanguage, translatedLanguage = request.TranslatedLanguage, originalText = request.OriginalText }), ct);
            var result = ParseResponse<TextTranslationResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"文本翻译失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Page<List<TranslateLanguage>>> GetLanguageListAsync(int pageNum, int pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/translate/list", JsonSerializer.Serialize(new { pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<TranslateLanguage>>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取语言列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<ModelTranslationResult> ModelTranslateAsync(string from, string to, string text, string taskId, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/task/modelTranslation", JsonSerializer.Serialize(new { from, to, text, extra = new { scenes = "翻译", taskId } }), ct);
            var result = ParseResponse<ModelTranslationResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"模型翻译失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Page<List<TextTranslationRecord>>> GetTranslateResultListAsync(int pageNum, int pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/translate/queryList", JsonSerializer.Serialize(new { pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<TextTranslationRecord>>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取翻译列表失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<DeleteRecordResult> DeleteTextTranslationAsync(long id, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/translate/deleteText", JsonSerializer.Serialize(new { id }), ct);
            var result = ParseResponse<DeleteRecordResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"删除翻译记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<CreatedSiRecord> CreateSiRecordAsync(string originalLanguage, string translatedLanguage, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/si/record/start", JsonSerializer.Serialize(new { originalLanguage, translatedLanguage }), ct);
            var result = ParseResponse<CreatedSiRecord>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"创建同声翻译记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Page<List<SiRecord>>> GetSiRecordListAsync(int pageNum, int pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/si/record/queryList", JsonSerializer.Serialize(new { pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<SiRecord>>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取同声翻译记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<CreatedSiRecord> SaveSiRecordAsync(long sid, string originalText, string translatedText, int sort, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/si/record/add", JsonSerializer.Serialize(new { sid, originalText, translatedText, sort }), ct);
            var result = ParseResponse<CreatedSiRecord>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"保存同声翻译记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<object> SaveRecordEndAsync(long sid, string audioUrl, int audioDuration, int status, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/si/record/end", JsonSerializer.Serialize(new { sid, audioUrl, audioDuration, status }), ct);
            var result = ParseResponse<object>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"结束同声翻译失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<Page<List<SiRecordDetail>>> GetSiRecordDetailAsync(long sid, long pageNum, long pageSize, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/si/record/detail", JsonSerializer.Serialize(new { sid, pageNum, pageSize }), ct);
            var result = ParseResponse<Page<List<SiRecordDetail>>>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取同声翻译详情失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<DeleteRecordResult> DeleteSiRecordAsync(long id, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/si/deleteRecord", JsonSerializer.Serialize(new { id }), ct);
            var result = ParseResponse<DeleteRecordResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"删除同声翻译记录失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<string> GetMicrosoftTranslationAuthTokenAsync(string device, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/mic/getToken", JsonSerializer.Serialize(new { device }), ct);
            var result = ParseResponse<string>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"获取微软翻译Token失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }

        public async Task<TextTranslationResult> ItsTranslateAsync(string from, string to, string data, CancellationToken ct = default)
        {
            var content = await PostAsync("/api/speak/common/its", JsonSerializer.Serialize(new { from, to, data }), ct);
            Logger?.LogInformation("ItsTranslateAsync 原始响应: {Content}", content);
            var result = ParseResponse<TextTranslationResult>(content);
            if (result.Success) return result.Value;
            throw new ShiliuOralException($"ITS翻译失败: {result.Message}", category: ShiliuOralErrorCategory.ServerError);
        }
    }
}
