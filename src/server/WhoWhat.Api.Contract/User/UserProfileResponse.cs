namespace WhoWhat.Api.Contract.User
{
    public class UserProfileResponse : BaseResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoSmallUri { get; set; }
        public string PhotoBigUri { get; set; }

        //Social networks profiles, at least one should be present
        public string FbProfile { get; set; }
        public string VkProfile { get; set; }

        public int Rating { get; set; }

        //Statistics
        public int QuestionsCount { get; set; }
        public int AnswersCount { get; set; }
    }
}
