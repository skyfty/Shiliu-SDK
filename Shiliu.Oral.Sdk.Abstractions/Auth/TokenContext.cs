namespace Shiliu.Oral.Sdk.Abstractions.Auth
{
    /// <summary>
    /// Token context passed between SDK layers.
    /// </summary>
    public class TokenContext
    {
        /// <summary>Raw JWT token string.</summary>
        public string Token { get; set; }

        /// <summary>Additional auth header value (used for SOE HMAC auth).</summary>
        public string AuthHeader { get; set; }

        /// <summary>Source identifier for API routing.</summary>
        public string Source { get; set; } = "11806";

        public bool HasToken => !string.IsNullOrEmpty(Token);
    }
}
