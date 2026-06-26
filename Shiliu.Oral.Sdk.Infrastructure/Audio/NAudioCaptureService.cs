using System;
using System.IO;
using NAudio.Wave;
using Shiliu.Oral.Sdk.Abstractions.Events;
using Shiliu.Oral.Sdk.Abstractions.Services;

namespace Shiliu.Oral.Sdk.Infrastructure.Audio
{
    /// <summary>
    /// NAudio-based audio capture service.
    /// Adapted from the original AudioCaptureService.
    /// </summary>
    public class NAudioCaptureService : IAudioCaptureService
    {
        private WaveInEvent _waveIn;
        private bool _isStopping;
        private string _outputFilePath;
        private WaveFileWriter _waveFileWriter;

        public bool IsCapturing { get; private set; }

        public event EventHandler<AudioFrameEventArgs> AudioDataAvailable;

        public void Start(string outputFilePath)
        {
            if (IsCapturing) return;

            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 16, 1)
            };

            _outputFilePath = outputFilePath;

            _waveIn.DataAvailable += OnWaveInDataAvailable;

            if (File.Exists(_outputFilePath))
            {
                using (var stream = new FileStream(_outputFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                    File.Delete(_outputFilePath);
                }
            }

            _waveIn.StartRecording();
            IsCapturing = true;
            _isStopping = false;

            _waveFileWriter = new WaveFileWriter(_outputFilePath, _waveIn.WaveFormat);
        }

        public void Stop()
        {
            if (!IsCapturing) return;

            _waveIn.StopRecording();
            IsCapturing = false;
            _isStopping = true;

            _waveFileWriter?.Dispose();
            _waveFileWriter = null;
        }

        public void Dispose()
        {
            if (_waveIn != null)
            {
                _waveIn.DataAvailable -= OnWaveInDataAvailable;
                _waveIn.Dispose();
                _waveIn = null;
            }

            Stop();
        }

        private void OnWaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            if (!IsCapturing) return;

            byte[] audioData = new byte[e.BytesRecorded];
            Array.Copy(e.Buffer, audioData, e.BytesRecorded);

            AudioDataAvailable?.Invoke(this, new AudioFrameEventArgs(audioData, e.BytesRecorded));

            if (!_isStopping)
            {
                _waveFileWriter?.Write(audioData, 0, audioData.Length);
            }
        }
    }
}
