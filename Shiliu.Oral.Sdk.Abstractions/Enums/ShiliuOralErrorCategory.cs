namespace Shiliu.Oral.Sdk.Abstractions.Enums
{
    /// <summary>
    /// Categories for SDK errors.
    /// </summary>
    public enum ShiliuOralErrorCategory
    {
        Unknown,
        Network,
        Authentication,
        InvalidArgument,
        ServerError,
        AudioDevice,
        RateLimit,
        Timeout
    }
}
