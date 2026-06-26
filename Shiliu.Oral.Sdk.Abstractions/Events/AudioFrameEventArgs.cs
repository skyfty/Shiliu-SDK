using System;

namespace Shiliu.Oral.Sdk.Abstractions.Events
{
    /// <summary>
    /// Event arguments for audio frame data.
    /// </summary>
    public class AudioFrameEventArgs : EventArgs
    {
        /// <summary>Raw PCM audio data.</summary>
        public byte[] AudioData { get; }

        /// <summary>Number of bytes recorded in this frame.</summary>
        public int BytesRecorded { get; }

        /// <summary>Audio sample rate (Hz).</summary>
        public int SampleRate { get; }

        /// <summary>Audio bits per sample.</summary>
        public int BitsPerSample { get; }

        /// <summary>Number of audio channels.</summary>
        public int Channels { get; }

        public AudioFrameEventArgs(byte[] audioData, int bytesRecorded, int sampleRate = 16000, int bitsPerSample = 16, int channels = 1)
        {
            AudioData = audioData;
            BytesRecorded = bytesRecorded;
            SampleRate = sampleRate;
            BitsPerSample = bitsPerSample;
            Channels = channels;
        }
    }
}
