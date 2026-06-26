using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Request to acquire an evaluation token from SOE service.
    /// </summary>
    public class AcquireTokenRequest
    {
        public int? Lang { get; set; }
        public string Engine { get; set; }
        public string DisEngine { get; set; }
        public int QuesType { get; set; }
        public string UserId { get; set; }
        public string ClassId { get; set; }
        public string SchoolCode { get; set; }
        public string BussinessId { get; set; }
        public string RegionCode { get; set; }
        public int SceneId { get; set; }
        public long? Duration { get; set; }
        public int? Vip { get; set; }
        public int? Age { get; set; }
        public string SsoId { get; set; }
    }

    /// <summary>
    /// Response from SOE token acquisition.
    /// </summary>
    public class AcquireTokenResponse
    {
        public string Token { get; set; }
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// Request to submit audio for speech evaluation.
    /// </summary>
    public class SubmitEvaluationRequest
    {
        public string Engine { get; set; }
        public string AudioPath { get; set; }
        public string FileId { get; set; }
        public string RequestId { get; set; }
        public string RequestJson { get; set; }
        public string Vip { get; set; }
        public string UserId { get; set; }
        public string ClassId { get; set; }
        public string SchoolCode { get; set; }
        public string BussinessId { get; set; }
        public string RegionCode { get; set; }
        public int SceneId { get; set; }
        public string QuesId { get; set; }
        public int QuesType { get; set; }
    }

    /// <summary>
    /// The evaluation request JSON embedded in SubmitEvaluationRequest.
    /// </summary>
    public class EvaluationRequestJson
    {
        public string RefText { get; set; }
        public int Rank { get; set; }
        public Dictionary<int, object> Ext { get; set; }
    }

    /// <summary>
    /// Engine extension parameters for SOE.
    /// </summary>
    public class EngineExtOptions
    {
        public string AudioType { get; set; }
        public int SampleRate { get; set; }
        public int Seek { get; set; }
        public float Precision { get; set; }
        public float Slack { get; set; }
        public string Keywords { get; set; }
        public int AgeGroup { get; set; }
        public string CustomizedLexicon { get; set; }
        public string Mode { get; set; }
    }

    /// <summary>
    /// SOE evaluation result.
    /// </summary>
    public class EvaluationResult
    {
        public string CorrectId { get; set; }

        [JsonPropertyName("overall")]
        public double OverallScore { get; set; }

        [JsonPropertyName("pronunciation")]
        public double PronScore { get; set; }

        [JsonPropertyName("fluency")]
        public double FluencyScore { get; set; }

        [JsonPropertyName("integrity")]
        public double IntegrityScore { get; set; }

        public WordScore[] Words { get; set; }
        public string AudioUrl { get; set; }
    }

    public class WordScore
    {
        public string Word { get; set; }
        public double Score { get; set; }
        public int IsOov { get; set; }
    }
}
