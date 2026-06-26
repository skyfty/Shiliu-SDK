using System.Threading;
using System.Threading.Tasks;

namespace Shiliu.Oral.Sdk.Abstractions.Auth
{
    /// <summary>
    /// Provides the current selected spoken-language code for outgoing SDK HTTP requests.
    /// </summary>
    public interface ICurrentLanguageProvider
    {
        Task<string> GetCurrentLanguageAsync(CancellationToken ct = default);
    }
}
