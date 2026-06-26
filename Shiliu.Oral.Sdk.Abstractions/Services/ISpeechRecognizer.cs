using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Shiliu.Oral.Sdk.Abstractions.Events;

namespace Shiliu.Oral.Sdk.Abstractions.Services
{
    /// <summary>
    /// Speech recognition service (ASR — Automatic Speech Recognition).
    /// Supports both one-shot and continuous streaming recognition.
    /// </summary>
    public interface ISpeechRecognizer : IDisposable
    {
        /// <summary>Connect to the speech recognition service.</summary>
        Task ConnectAsync(CancellationToken ct = default);

        /// <summary>Send the START message to begin recognition.</summary>
        Task SendStartMessageAsync(string fromLanguage, string toLanguage, CancellationToken ct = default);

        /// <summary>Enqueue audio data for streaming recognition.</summary>
        void EnqueueAudioData(byte[] audioData);

        /// <summary>Start the audio streaming send loop.</summary>
        Task SendAudioStreamAsync(CancellationToken ct = default);

        /// <summary>Disconnect from the recognition service.</summary>
        Task DisconnectAsync();

        /// <summary>Fired when a text recognition result is received.</summary>
        event Action<string> OnTextMessage;

        /// <summary>Fired when binary audio data is received.</summary>
        event Action<MemoryStream> OnBinaryMessage;

        /// <summary>Fired when the recognition session ends.</summary>
        event Action OnEndMessage;
    }
}
