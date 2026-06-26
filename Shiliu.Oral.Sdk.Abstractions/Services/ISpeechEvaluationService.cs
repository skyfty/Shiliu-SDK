using System.Threading;
using System.Threading.Tasks;
using Shiliu.Oral.Sdk.Abstractions.Models;

namespace Shiliu.Oral.Sdk.Abstractions.Services
{
    /// <summary>
    /// Speech oral evaluation service (pronunciation scoring).
    /// </summary>
    public interface ISpeechEvaluationService
    {
        /// <summary>Get server timestamp for auth signature.</summary>
        Task<long> GetTimestampAsync(CancellationToken ct = default);

        /// <summary>Acquire an evaluation token (V3).</summary>
        Task<AcquireTokenResponse> AcquireV3Async(AcquireTokenRequest request, CancellationToken ct = default);

        /// <summary>Submit audio for real-time evaluation. Returns result directly if server responds synchronously.</summary>
        Task<EvaluationResult> SubmitEvaluationAsync(SubmitEvaluationRequest request, CancellationToken ct = default);

        /// <summary>Query evaluation results by correct ID.</summary>
        Task<EvaluationResult> QueryEvaluationResultAsync(string correctId, CancellationToken ct = default);

        /// <summary>Generate HMAC auth header for SOE API.</summary>
        Task<string> GenerateAuthHeaderAsync(CancellationToken ct = default);
    }
}
