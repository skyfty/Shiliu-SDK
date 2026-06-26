using System;

namespace Shiliu.Oral.Sdk.Abstractions.Exceptions
{
    /// <summary>
    /// Unified exception type for all Shiliu Oral SDK errors.
    /// Callers can catch this single type and use Category for dispatch.
    /// </summary>
    public class ShiliuOralException : Exception
    {
        /// <summary>Machine-readable error code.</summary>
        public string ErrorCode { get; }

        /// <summary>Error category for programmatic handling.</summary>
        public Enums.ShiliuOralErrorCategory Category { get; }

        public ShiliuOralException(string message, string errorCode = null, Enums.ShiliuOralErrorCategory category = Enums.ShiliuOralErrorCategory.Unknown)
            : base(message)
        {
            ErrorCode = errorCode;
            Category = category;
        }

        public ShiliuOralException(string message, Exception innerException, string errorCode = null, Enums.ShiliuOralErrorCategory category = Enums.ShiliuOralErrorCategory.Unknown)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            Category = category;
        }
    }
}
