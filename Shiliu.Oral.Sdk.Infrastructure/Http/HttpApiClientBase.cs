using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiliu.Oral.Sdk.Abstractions.Models;

namespace Shiliu.Oral.Sdk.Infrastructure.Http
{
    /// <summary>
    /// Base class for HTTP API clients. Provides GET/POST helpers with serialization.
    /// Adapted from the original AiTalkClient's GetAsync/PostAsync helpers.
    /// </summary>
    public abstract class HttpApiClientBase
    {
        protected readonly IHttpClientFactory HttpClientFactory;
        protected readonly string ClientName;
        protected readonly ILogger Logger;

        protected HttpApiClientBase(IHttpClientFactory httpClientFactory, string clientName, ILogger logger)
        {
            HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            ClientName = clientName ?? throw new ArgumentNullException(nameof(clientName));
            Logger = logger;
        }

        /// <summary>Perform a GET request.</summary>
        protected async Task<string> GetAsync(string endpoint, CancellationToken ct = default)
        {
            var client = HttpClientFactory.CreateClient(ClientName);
            string url = new Uri(client.BaseAddress, endpoint).ToString();

            Logger?.LogDebug("GET {Url}", url);

            var response = await client.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            Logger?.LogDebug("GET {Url} OK", url);
            return data;
        }

        /// <summary>Perform a POST request with JSON body.</summary>
        protected async Task<string> PostAsync(string endpoint, string jsonData, CancellationToken ct = default)
        {
            var client = HttpClientFactory.CreateClient(ClientName);
            string url = new Uri(client.BaseAddress, endpoint).ToString();

            Logger?.LogDebug("POST {Url}", url);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content, ct);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            Logger?.LogDebug("POST {Url} OK", url);
            return data;
        }

        /// <summary>Parse an AiTalk-style API response.</summary>
        protected ApiResponse<T> ParseResponse<T>(string responseContent)
        {
            return JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        /// <summary>Parse a SOE-style API response.</summary>
        protected SoeApiResponse<T> ParseSoeResponse<T>(string responseContent)
        {
            return JsonSerializer.Deserialize<SoeApiResponse<T>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
