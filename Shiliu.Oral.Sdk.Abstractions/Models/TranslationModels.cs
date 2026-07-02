using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Language option for translation.
    /// </summary>
    public class TranslateLanguage
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("languageCode")]
        public string Code { get; set; }

        [JsonPropertyName("languageName")]
        public string Name { get; set; }

        [JsonPropertyName("englishName")]
        public string EnglishName { get; set; }

        [JsonPropertyName("languageCodeBd")]
        public string CodeBd { get; set; }

        [JsonPropertyName("languageCodeAzure")]
        public string CodeAzure { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        [JsonPropertyName("isDeleted")]
        public int IsDeleted { get; set; }
    }

    /// <summary>
    /// Simultaneous translation record.
    /// Clean DTO — no INotifyPropertyChanged, no UI properties.
    /// </summary>
    public class SiRecord
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("originalLanguage")]
        public string OriginalLanguage { get; set; }

        [JsonPropertyName("translatedLanguage")]
        public string TranslatedLanguage { get; set; }

        [JsonPropertyName("audioUrl")]
        public string AudioUrl { get; set; }

        [JsonPropertyName("audioDuration")]
        public int? AudioDuration { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("originalText")]
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
        [JsonPropertyName("translatedText")]
        public string TranslatedText { get; set; }

        [JsonPropertyName("originalText")]
        public string OriginalText { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("originalLanguage")]
        public string OriginalLanguage { get; set; }

        [JsonPropertyName("translatedLanguage")]
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
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("originalText")]
        public string OriginalText { get; set; }

        [JsonPropertyName("originalLanguage")]
        public string OriginalLanguage { get; set; }

        [JsonPropertyName("originalLanguageName")]
        public string OriginalLanguageName { get; set; }

        [JsonPropertyName("translatedText")]
        public string TranslatedText { get; set; }

        [JsonPropertyName("translatedLanguage")]
        public string TranslatedLanguage { get; set; }

        [JsonPropertyName("translatedLanguageName")]
        public string TranslatedLanguageName { get; set; }

        [JsonPropertyName("characterCount")]
        public int CharacterCount { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("isDeleted")]
        public string IsDeleted { get; set; }
    }
}
