using System;

namespace Shiliu.Oral.Sdk.Abstractions.Events
{
    /// <summary>
    /// AI conversation error event arguments.
    /// </summary>
    public class AiTalkErrorEventArgs : EventArgs
    {
        public string Message { get; }
        public string ConversationId { get; }
        public Exception Exception { get; }

        public AiTalkErrorEventArgs(string message, string conversationId = null, Exception exception = null)
        {
            Message = message;
            ConversationId = conversationId;
            Exception = exception;
        }
    }
}
