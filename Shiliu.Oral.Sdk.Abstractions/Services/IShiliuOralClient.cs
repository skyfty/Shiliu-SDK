using System;
using System.Threading.Tasks;

namespace Shiliu.Oral.Sdk.Abstractions.Services
{
    /// <summary>
    /// Unified facade for the Shiliu Oral English SDK.
    /// Provides access to all core services through a single entry point.
    /// </summary>
    public interface IShiliuOralClient : IDisposable
    {
        /// <summary>AI conversation service (scene-based dialogue practice).</summary>
        IAiTalkService AiTalk { get; }

        /// <summary>Speech oral evaluation service (pronunciation scoring).</summary>
        ISpeechEvaluationService SpeechEvaluation { get; }

        /// <summary>Speech recognition service (ASR).</summary>
        ISpeechRecognizer SpeechRecognizer { get; }

        /// <summary>Translation service (text + simultaneous).</summary>
        ITranslationService Translation { get; }

        /// <summary>Audio capture service (microphone recording).</summary>
        IAudioCaptureService AudioCapture { get; }

        /// <summary>Audio playback service.</summary>
        IAudioPlayerService AudioPlayer { get; }
    }
}
