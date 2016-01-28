namespace WhoWhat.Api.Contract.Payload
{
    public class Author
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUri { get; set; }
        public int Rating { get; set; }
    }
}
