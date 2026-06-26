using System.Threading;
using System.Threading.Tasks;

namespace Shiliu.Oral.Sdk.Abstractions.Auth
{
    /// <summary>
    /// Token provider interface. SDK callers implement this to supply authentication tokens.
    /// SDK calls GetAccessTokenAsync before every HTTP/WebSocket request.
    /// OnTokenExpiredAsync is called when SDK receives a 401 response.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Get the current valid access token.
        /// Return format: the raw token string (no "Bearer" prefix needed — SDK adds it).
        /// </summary>
        Task<string> GetAccessTokenAsync(CancellationToken ct = default);

        /// <summary>
        /// Called when SDK receives a 401 Unauthorized response.
        /// The implementer should refresh the token or trigger re-login.
        /// Return true if token was successfully refreshed, false otherwise.
        /// </summary>
        Task<bool> OnTokenExpiredAsync(CancellationToken ct = default);
    }
}
