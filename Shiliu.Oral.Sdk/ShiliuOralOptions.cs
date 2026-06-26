namespace Shiliu.Oral.Sdk
{
    /// <summary>
    /// Configuration options for the Shiliu Oral English SDK.
    /// </summary>
    public class ShiliuOralOptions
    {
        /// <summary>Base URL for the AI Talk API (scenes, tasks, records, translation, TTS, etc.).</summary>
        public string AiTalkBaseUrl { get; set; } = "https://ai-talk.unipus.cn";

        /// <summary>Base URL for the Speech Oral Evaluation (SOE) API.</summary>
        public string SoeBaseUrl { get; set; } = "https://zt.unipus.cn";

        /// <summary>Source identifier sent in HTTP headers (default: "11806").</summary>
        public string Source { get; set; } = "11806";

        /// <summary>SOE app key for HMAC authentication.</summary>
        public string SoeAppKey { get; set; } = "aioral";

        /// <summary>SOE app secret for HMAC authentication.</summary>
        public string SoeAppSecret { get; set; } = "IlNkMpXGnUUZRZlq";

        /// <summary>Baidu ASR app ID.</summary>
        public string BaiduAppId { get; set; } = "116450031";

        /// <summary>Baidu ASR app key.</summary>
        public string BaiduAppKey { get; set; } = "X3YmhBMw5aUcmppkJZocbemp";

        /// <summary>Baidu ASR WebSocket URI.</summary>
        public string BaiduAsrWebSocketUri { get; set; } = "wss://aip.baidubce.com/ws/realtime_speech_trans";
    }
}
