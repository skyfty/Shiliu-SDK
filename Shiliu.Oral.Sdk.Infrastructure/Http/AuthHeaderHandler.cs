using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Shiliu.Oral.Sdk.Abstractions.Auth;

namespace Shiliu.Oral.Sdk.Infrastructure.Http
{
    /// <summary>
    /// HTTP message handler that automatically injects token and auth headers.
    /// Replaces the original inline token/source/auth header logic.
    /// </summary>
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly string _source;
        private readonly Func<CancellationToken, Task<string>> _authHeaderFactory;

        /// <summary>
        /// Creates an AuthHeaderHandler with an optional custom auth header factory (for SOE HMAC).
        /// </summary>
        public AuthHeaderHandler(ITokenProvider tokenProvider, string source = "11806", Func<CancellationToken, Task<string>> authHeaderFactory = null)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _source = source;
            _authHeaderFactory = authHeaderFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Inject token
            var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Remove("token");
                request.Headers.Add("token", token);
            }

            request.Headers.Remove("source");
            request.Headers.Add("source", _source);

            // Inject custom auth header (e.g. SOE HMAC)
            if (_authHeaderFactory != null)
            {
                var auth = await _authHeaderFactory(cancellationToken);
                if (!string.IsNullOrEmpty(auth))
                {
                    request.Headers.Remove("auth");
                    request.Headers.Add("auth", auth);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Handle 401 — try token refresh once
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (await _tokenProvider.OnTokenExpiredAsync(cancellationToken))
                {
                    token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);
                    if (!string.IsNullOrEmpty(token))
                    {
                        request.Headers.Remove("token");
                        request.Headers.Add("token", token);
                    }
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }
    }
}
