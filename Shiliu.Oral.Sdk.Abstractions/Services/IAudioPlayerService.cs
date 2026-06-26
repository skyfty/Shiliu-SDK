using System;

namespace Shiliu.Oral.Sdk.Abstractions.Services
{
    /// <summary>
    /// Audio playback service for playing audio files.
    /// </summary>
    public interface IAudioPlayerService : IDisposable
    {
        /// <summary>Whether audio is currently playing.</summary>
        bool IsPlaying { get; }

        /// <summary>Start playing an audio file.</summary>
        void Play(string audioFilePath);

        /// <summary>Stop playback.</summary>
        void Stop();

        /// <summary>Fired when playback stops (naturally or via Stop).</summary>
        event EventHandler PlaybackStopped;
    }
}
