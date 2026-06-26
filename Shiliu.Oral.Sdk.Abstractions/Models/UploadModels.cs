namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    /// <summary>
    /// Result of uploading a file to cloud storage.
    /// </summary>
    public class UploadResult
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Device verification request.
    /// </summary>
    public class DeviceVerificationRequest
    {
        public int SourceId { get; set; } = 11806;
        public string SerialNumber { get; set; }
    }

    /// <summary>
    /// Save record end request (for simultaneous translation).
    /// </summary>
    public class SaveRecordEndRequest
    {
        public long Sid { get; set; }
        public string AudioUrl { get; set; }
        public int AudioDuration { get; set; }
        public int Status { get; set; }
    }

    /// <summary>
    /// Simultaneous translation save request.
    /// </summary>
    public class SaveSiRecordRequest
    {
        public long Sid { get; set; }
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public int Sort { get; set; }
    }

    /// <summary>
    /// Task start request.
    /// </summary>
    public class StartTaskRequest
    {
        public int SceneId { get; set; }
        public int TaskType { get; set; } = 1;
    }

    /// <summary>
    /// Delete translation record request.
    /// </summary>
    public class DeleteRecordResult
    {
        public bool Success { get; set; }
    }

    /// <summary>
    /// Audio format/quality information for a recording.
    /// </summary>
    public class AudioFileInfo
    {
        public string FilePath { get; set; }
        public int DurationMs { get; set; }
        public int SampleRate { get; set; }
        public int BitsPerSample { get; set; }
        public int Channels { get; set; }
    }
}
