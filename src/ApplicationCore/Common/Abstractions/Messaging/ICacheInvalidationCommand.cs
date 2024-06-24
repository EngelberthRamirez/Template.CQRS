namespace ApplicationCore.Common.Abstractions.Messaging
{
    public interface ICacheInvalidationCommand
    {
        IEnumerable<string> CacheKeys { get; }
    }
}
