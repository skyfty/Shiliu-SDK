using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shiliu.Oral.Sdk.Abstractions.Models;

namespace Shiliu.Oral.Sdk.Abstractions.Services
{
    /// <summary>
    /// Translation service: text translation + simultaneous (real-time) translation.
    /// </summary>
    public interface ITranslationService
    {
        /// <summary>Translate a single text string.</summary>
        Task<TextTranslationResult> TranslateTextAsync(TextTranslationRequest request, CancellationToken ct = default);

        /// <summary>Get available language list.</summary>
        Task<Page<List<TranslateLanguage>>> GetLanguageListAsync(int pageNum, int pageSize, CancellationToken ct = default);

        /// <summary>Translate using AI model (chat context).</summary>
        Task<ModelTranslationResult> ModelTranslateAsync(string from, string to, string text, string taskId, CancellationToken ct = default);

        /// <summary>Get text translation history.</summary>
        Task<Page<List<TextTranslationRecord>>> GetTranslateResultListAsync(int pageNum, int pageSize, CancellationToken ct = default);

        /// <summary>Delete a text translation record.</summary>
        Task<DeleteRecordResult> DeleteTextTranslationAsync(long id, CancellationToken ct = default);

        // Simultaneous translation methods

        /// <summary>Create a new simultaneous translation session.</summary>
        Task<CreatedSiRecord> CreateSiRecordAsync(string originalLanguage, string translatedLanguage, CancellationToken ct = default);

        /// <summary>Get simultaneous translation history.</summary>
        Task<Page<List<SiRecord>>> GetSiRecordListAsync(int pageNum, int pageSize, CancellationToken ct = default);

        /// <summary>Save an entry in a simultaneous translation session.</summary>
        Task<CreatedSiRecord> SaveSiRecordAsync(long sid, string originalText, string translatedText, int sort, CancellationToken ct = default);

        /// <summary>End a simultaneous translation session and save audio.</summary>
        Task<object> SaveRecordEndAsync(long sid, string audioUrl, int audioDuration, int status, CancellationToken ct = default);

        /// <summary>Get simultaneous translation detail entries.</summary>
        Task<Page<List<SiRecordDetail>>> GetSiRecordDetailAsync(long sid, long pageNum, long pageSize, CancellationToken ct = default);

        /// <summary>Delete a simultaneous translation record.</summary>
        Task<DeleteRecordResult> DeleteSiRecordAsync(long id, CancellationToken ct = default);

        /// <summary>Get Microsoft translation auth token.</summary>
        Task<string> GetMicrosoftTranslationAuthTokenAsync(string device, CancellationToken ct = default);

        /// <summary>ITS translation (for digital human).</summary>
        Task<TextTranslationResult> ItsTranslateAsync(string from, string to, string data, CancellationToken ct = default);
    }
}
