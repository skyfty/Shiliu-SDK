using System;
using NAudio.Wave;
using Shiliu.Oral.Sdk.Abstractions.Services;

namespace Shiliu.Oral.Sdk.Infrastructure.Audio
{
    /// <summary>
    /// NAudio-based audio playback service.
    /// Adapted from the original WavePlayerAudioService.
    /// </summary>
    public class WavePlayerService : IAudioPlayerService
    {
        private IWavePlayer _wavePlayer;
        private AudioFileReader _audioFileReader;

        public bool IsPlaying { get; private set; }

        public event EventHandler PlaybackStopped;

        public void Play(string audioFilePath)
        {
            Stop();

            _audioFileReader = new AudioFileReader(audioFilePath);
            _wavePlayer = new WaveOutEvent();
            _wavePlayer.PlaybackStopped += OnPlaybackStopped;
            _wavePlayer.Init(_audioFileReader);
            _wavePlayer.Play();
            IsPlaying = true;
        }

        public void Stop()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.PlaybackStopped -= OnPlaybackStopped;
                _wavePlayer.Stop();
                _wavePlayer.Dispose();
                _wavePlayer = null;
            }

            _audioFileReader?.Dispose();
            _audioFileReader = null;

            IsPlaying = false;
        }

        public void Dispose()
        {
            Stop();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            IsPlaying = false;
            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}
