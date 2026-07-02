using System.Text.Json.Serialization;

namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Unified API response wrapper matching AiTalk API format.
    /// </summary>
    public class ApiResponse<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("value")]
        public T Value { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }

    /// <summary>
    /// API response wrapper matching SOE API format.
    /// </summary>
    public class SoeApiResponse<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("info")]
        public string Info { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }

    /// <summary>
    /// Paginated response wrapper.
    /// </summary>
    public class Page<T>
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
