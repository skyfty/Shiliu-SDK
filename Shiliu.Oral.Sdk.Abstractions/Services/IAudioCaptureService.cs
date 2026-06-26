using System;
using System.Collections.Generic;
using Shiliu.Oral.Sdk.Abstractions.Events;

namespace Shiliu.Oral.Sdk.Abstractions.Services
{
    /// <summary>
    /// Audio capture service for microphone recording.
    /// SDK callers can either use this service or provide their own audio stream.
    /// </summary>
    public interface IAudioCaptureService : IDisposable
    {
        /// <summary>Whether audio capture is currently active.</summary>
        bool IsCapturing { get; }

        /// <summary>Start audio capture and save to file.</summary>
        void Start(string outputFilePath);

        /// <summary>Stop audio capture.</summary>
        void Stop();

        /// <summary>Fired when new audio data is available.</summary>
        event EventHandler<AudioFrameEventArgs> AudioDataAvailable;
    }
}
