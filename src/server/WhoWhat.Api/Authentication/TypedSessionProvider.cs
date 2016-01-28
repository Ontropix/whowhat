namespace WhoWhat.Api
{
    /// <summary>
    /// Provides access to <see cref="CustomUserSession"/>. 
    /// The intention is using during unit testing to inject a typed user session.
    /// </summary>
    public interface ITypedSessionProvider
    {
        CustomUserSession Session { get; } 
    }
}
