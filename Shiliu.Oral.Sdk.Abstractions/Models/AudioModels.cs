using System.Collections.Generic;

namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Text-to-speech audio result.
    /// </summary>
    public class TextToAudioResult
    {
        public string Text { get; set; }
        public string Speaker { get; set; }
        public string AudioUrl { get; set; }
    }

    /// <summary>
    /// Available voice tone for TTS.
    /// </summary>
    public class VoiceTone
    {
        public int Id { get; set; }
        public string SpeakerEn { get; set; }
        public string AvatarUrl { get; set; }
        public string SpeakerCn { get; set; }
        public string Language { get; set; }
        public string LanguageType { get; set; }
        public string Gender { get; set; }
        public int IsDefault { get; set; }
        public List<VoiceToneDigitalPerson> DigitalPersons { get; set; }
        public List<int> ScenesEvaluationType { get; set; }
        public int VoiceType { get; set; }
    }

    /// <summary>
    /// Digital person configuration attached to a voice tone.
    /// </summary>
    public class VoiceToneDigitalPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Md5 { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public bool Gender { get; set; }
        public string AgeRange { get; set; }
        public string Style { get; set; }
        public bool IsDefault { get; set; }
        public string LargeAvatarUrl { get; set; }
        public string SmallAvatarUrl { get; set; }
        public string AvatarVideoUrl { get; set; }
    }

    /// <summary>
    /// Recommended speaking prompt for AI conversation.
    /// </summary>
    public class RecommendSpeakResult
    {
        public string Content { get; set; }
        public string Translation { get; set; }
        public string AudioUrl { get; set; }
    }

    /// <summary>
    /// Upload token for cloud storage.
    /// </summary>
    public class UploadTokenResult
    {
        public string Token { get; set; }
        public string Key { get; set; }
        public string Domain { get; set; }
        public string BucketName { get; set; }
        public string FileKey { get; set; }
        public string TransFileKey { get; set; }
        public string TransToken { get; set; }
    }

    /// <summary>
    /// Task detail / evaluation summary.
    /// </summary>
    public class TaskDetail
    {
        public string Id { get; set; }
        public int? VoiceToneId { get; set; }
        public int? SceneId { get; set; }
        public string Scenes { get; set; }
        public int? Status { get; set; }
        public object Evaluation { get; set; }
        public object EvaluationContent { get; set; }
        public int? Duration { get; set; }
        public int? RoundCount { get; set; }
        public string Created { get; set; }
        public List<TaskRecord> Records { get; set; }

        // 评测相关字段
        public string EvaluationStatus { get; set; }
        public double? Score { get; set; }
        public double? ScoreSystem { get; set; }
        public TaskDetailOverallEvaluation OverallEvaluation { get; set; }
        public TaskDetailEvaluationTypeResponse EvaluationTypeResponse { get; set; }
    }

    public class TaskDetailOverallEvaluation
    {
        public double TotalScore { get; set; }
        public float ScoreRate { get; set; }
        public string TotalFeedback { get; set; }
        public List<TaskDetailScoreInfo> ScoreList { get; set; }
    }

    public class TaskDetailScoreInfo
    {
        public string Name { get; set; }
        public string CnName { get; set; }
        public double Score { get; set; }
        public float ScoreRate { get; set; }
        public string Feedback { get; set; }
    }

    public class TaskDetailEvaluationTypeResponse
    {
        public TaskDetailEvaluationDetail GrammarAndSyntax { get; set; }
        public TaskDetailEvaluationDetail Fluency { get; set; }
        public TaskDetailEvaluationDetail CommunicationSkills { get; set; }
        public TaskDetailEvaluationDetail Completeness { get; set; }
        public TaskDetailEvaluationDetail Vocabulary { get; set; }
        public TaskDetailEvaluationDetail Grammar { get; set; }
        public TaskDetailEvaluationDetail AccuracyScope { get; set; }
        public TaskDetailEvaluationDetail LengthCoherence { get; set; }
        public TaskDetailEvaluationDetail FlexibilityAppropriateness { get; set; }
    }

    public class TaskDetailEvaluationDetail
    {
        public int? Score { get; set; }
        public string Remark { get; set; }
        public string EvaluationType { get; set; }
        public string EvaluationTypeCn { get; set; }
    }

    /// <summary>
    /// Word/phrase practice item.
    /// </summary>
    public class WordPhraseItem
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public string Translation { get; set; }
        public string AudioUrl { get; set; }
        public int SceneId { get; set; }
    }

    /// <summary>
    /// Phrase practice list item.
    /// </summary>
    public class PhraseListItem
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Translation { get; set; }
        public List<PhraseSubItem> Items { get; set; }
    }

    public class PhraseSubItem
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
