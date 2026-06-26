namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Task result summary (practice session list item).
    /// Clean DTO — no WPF/UI properties.
    /// </summary>
    public class TaskResult
    {
        public string Id { get; set; }
        public int? VoiceToneId { get; set; }
        public string SpeakerEn { get; set; }
        public string AvatarUrl { get; set; }
        public int? SceneId { get; set; }
        public string Scenes { get; set; }
        public int? CharacterId { get; set; }
        public string UserInstructionPrompt { get; set; }
        public string EnUserInstructionPrompt { get; set; }
        public int? Status { get; set; }
        public object Evaluation { get; set; }
        public object EvaluationContent { get; set; }
        public int? Duration { get; set; }
        public int? RoundCount { get; set; }
        public string Created { get; set; }
        public string Language { get; set; }
        public int? Depth { get; set; }
    }

    /// <summary>
    /// Individual record within a conversation task.
    /// Clean DTO — no INotifyPropertyChanged, no Visibility, no Image paths.
    /// </summary>
    public class TaskRecord
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string BotChContent { get; set; }
        public string BotEnContent { get; set; }
        public object BotAudioUrl { get; set; }
        public int BotAudioDuration { get; set; }
        public object UserChContent { get; set; }
        public object UserEnContent { get; set; }
        public object UserAudioUrl { get; set; }
        public int UserAudioDuration { get; set; }
        public int SpeakType { get; set; }
        public int Speed { get; set; }
        public int VoiceToneId { get; set; }
        public object EvaluationId { get; set; }
        public object Evaluation { get; set; }
        public string Created { get; set; }
        public int Sort { get; set; }
        public string Language { get; set; }
        public UserSingleReviewResult RecordEvaluation { get; set; }
    }

    /// <summary>
    /// User's single review/evaluation result.
    /// Field names match server response (camelCase, PropertyNameCaseInsensitive handles mapping).
    /// </summary>
    public class UserSingleReviewResult
    {
        // Server returns these fields directly (camelCase)
        public double? Overall { get; set; }
        public float? OverallScoreRate { get; set; }
        public double? Fluency { get; set; }
        public double? Integrity { get; set; }
        public double? Pronunciation { get; set; }
        public double? GramVocab { get; set; }
        public double? Rhythm { get; set; }
        public double? Speed { get; set; }
        public int? ScoreSystem { get; set; }
        public string SentenceFeedback { get; set; }
        public string SentenceRefinement { get; set; }
        public System.Collections.Generic.List<SdkSentence> Sentences { get; set; }

        // Populated by AiTalkService after deserialization
        public string UserAudioUrl { get; set; }
        public string OriginalResult { get; set; }
    }

    public class SdkSentence
    {
        public double Overall { get; set; }
        public string Sentence { get; set; }
        public System.Collections.Generic.List<SdkWordInfo> Words { get; set; }
    }

    public class SdkWordInfo
    {
        public string Word { get; set; }
        public int? BeginIndex { get; set; }
        public int? EndIndex { get; set; }
        public SdkScoreInfo Scores { get; set; }
    }

    public class SdkScoreInfo
    {
        public double Overall { get; set; }
    }
}
