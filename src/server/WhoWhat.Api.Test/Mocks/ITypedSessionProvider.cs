namespace WhoWhat.Api.Test.Mocks
{
    public class TypedSessionProvider : ITypedSessionProvider
    {
        public CustomUserSession Session { get; set; }
    }
}
