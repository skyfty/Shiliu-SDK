using System;
using Shiliu.Oral.Sdk.Abstractions.Services;

namespace Shiliu.Oral.Sdk
{
    /// <summary>
    /// Unified facade for the Shiliu Oral English SDK.
    /// Aggregates all core services through a single entry point.
    /// </summary>
    public class ShiliuOralClient : IShiliuOralClient
    {
        public IAiTalkService AiTalk { get; }
        public ISpeechEvaluationService SpeechEvaluation { get; }
        public ISpeechRecognizer SpeechRecognizer { get; }
        public ITranslationService Translation { get; }
        public IAudioCaptureService AudioCapture { get; }
        public IAudioPlayerService AudioPlayer { get; }

        public ShiliuOralClient(
            IAiTalkService aiTalk,
            ISpeechEvaluationService speechEvaluation,
            ISpeechRecognizer speechRecognizer,
            ITranslationService translation,
            IAudioCaptureService audioCapture,
            IAudioPlayerService audioPlayer)
        {
            AiTalk = aiTalk ?? throw new ArgumentNullException(nameof(aiTalk));
            SpeechEvaluation = speechEvaluation ?? throw new ArgumentNullException(nameof(speechEvaluation));
            SpeechRecognizer = speechRecognizer ?? throw new ArgumentNullException(nameof(speechRecognizer));
            Translation = translation ?? throw new ArgumentNullException(nameof(translation));
            AudioCapture = audioCapture ?? throw new ArgumentNullException(nameof(audioCapture));
            AudioPlayer = audioPlayer ?? throw new ArgumentNullException(nameof(audioPlayer));
        }

        public void Dispose()
        {
            SpeechRecognizer?.Dispose();
            AudioCapture?.Dispose();
            AudioPlayer?.Dispose();
        }
    }
}
