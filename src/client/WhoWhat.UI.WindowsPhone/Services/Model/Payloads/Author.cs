namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    [PropertyChanged.ImplementPropertyChanged]
    public class Author
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUri { get; set; }
        public int Rating { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}