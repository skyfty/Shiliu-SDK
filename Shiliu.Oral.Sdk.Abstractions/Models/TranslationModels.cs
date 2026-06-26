using System.Collections.Generic;

namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Language option for translation.
    /// </summary>
    public class TranslateLanguage
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string EnglishName { get; set; }

        public string CodeBd { get; set; }

        public string CodeAzure { get; set; }

        public int Sort { get; set; }

        public int IsDeleted { get; set; }
    }

    /// <summary>
    /// Simultaneous translation record.
    /// Clean DTO — no INotifyPropertyChanged, no UI properties.
    /// </summary>
    public class SiRecord
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string OriginalLanguage { get; set; }

        public string TranslatedLanguage { get; set; }

        public string AudioUrl { get; set; }

        public int? AudioDuration { get; set; }

        public int Status { get; set; }

        public string Created { get; set; }

        public string OriginalText { get; set; }
    }

    /// <summary>
    /// Created simultaneous translation session response.
    /// </summary>
    public class CreatedSiRecord
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }

    /// <summary>
    /// Single entry in a simultaneous translation session.
    /// </summary>
    public class SiRecordDetail
    {
        public long Id { get; set; }
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public int Sort { get; set; }
    }

    /// <summary>
    /// Simultaneous translation history page.
    /// </summary>
    public class SiRecordHistoryPage
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<SiRecord> Data { get; set; }
    }

    /// <summary>
    /// Text translation request.
    /// </summary>
    public class TextTranslationRequest
    {
        public string OriginalLanguage { get; set; }
        public string TranslatedLanguage { get; set; }
        public string OriginalText { get; set; }
    }

    /// <summary>
    /// Text translation result.
    /// </summary>
    public class TextTranslationResult
    {
        public string TranslatedText { get; set; }

        public string OriginalText { get; set; }

        public long Id { get; set; }

        public string Created { get; set; }

        public string OriginalLanguage { get; set; }

        public string TranslatedLanguage { get; set; }
    }

    /// <summary>
    /// AI model translation response.
    /// </summary>
    public class ModelTranslationResult
    {
        public string TranslatedText { get; set; }
        public string SourceText { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }

    /// <summary>
    /// Text translation history record.
    /// </summary>
    public class TextTranslationRecord
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string OriginalText { get; set; }

        public string OriginalLanguage { get; set; }

        public string OriginalLanguageName { get; set; }

        public string TranslatedText { get; set; }

        public string TranslatedLanguage { get; set; }

        public string TranslatedLanguageName { get; set; }

        public int CharacterCount { get; set; }

        public string Created { get; set; }

        public string IsDeleted { get; set; }
    }
}
