using System;

namespace Shiliu.Oral.Sdk.Abstractions.Events
{
    /// <summary>
    /// Partial (interim) ASR result — recognition is still in progress.
    /// </summary>
    public class AsrPartialEventArgs : EventArgs
    {
        public string Text { get; }
        public string TranslatedText { get; }

        public AsrPartialEventArgs(string text, string translatedText)
        {
            Text = text;
            TranslatedText = translatedText;
        }
    }

    /// <summary>
    /// Final ASR result — recognition is complete for this utterance.
    /// </summary>
    public class AsrFinalEventArgs : EventArgs
    {
        public string Text { get; }
        public string TranslatedText { get; }
        public string AudioData { get; }

        public AsrFinalEventArgs(string text, string translatedText, string audioData = null)
        {
            Text = text;
            TranslatedText = translatedText;
            AudioData = audioData;
        }
    }

    /// <summary>
    /// ASR error event arguments.
    /// </summary>
    public class AsrErrorEventArgs : EventArgs
    {
        public string Message { get; }
        public int Code { get; }
        public Exception Exception { get; }

        public AsrErrorEventArgs(string message, int code = 0, Exception exception = null)
        {
            Message = message;
            Code = code;
            Exception = exception;
        }
    }
}
