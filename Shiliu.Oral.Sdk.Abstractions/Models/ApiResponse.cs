namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Unified API response wrapper matching AiTalk API format.
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Numeric status code returned by the API (server-defined).
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Human-readable message or error description.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Payload value returned by the API. The concrete type is T.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// API response wrapper matching SOE API format.
    /// </summary>
    public class SoeApiResponse<T>
    {
        /// <summary>
        /// Numeric status code returned by SOE API.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Informational message or explanation from SOE API.
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Response payload (data) returned by the SOE API.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Whether the SOE API reports success for the request.
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// Paginated response wrapper.
    /// </summary>
    public class Page<T>
    {
        /// <summary>
        /// Total number of items available across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number (1-based).
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Size (number of items) per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Page data payload. The concrete shape is T (commonly a list).
        /// </summary>
        public T Data { get; set; }
    }
}
