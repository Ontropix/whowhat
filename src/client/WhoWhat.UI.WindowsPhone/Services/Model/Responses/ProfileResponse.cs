using System;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    public class ProfileResponse : RestResponse
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

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName ?? string.Empty, LastName ?? string.Empty).Trim();
            }
        }

        public Uri SocialLogo
        {
            get
            {
                if (!string.IsNullOrEmpty(VkProfile))
                {
                    return new Uri("/Assets/vk.png", UriKind.Relative);
                }

                if (!string.IsNullOrEmpty(FbProfile))
                {
                    return new Uri("/Assets/facebook.png", UriKind.Relative);
                }

                return null;
            }
        }
    }
}
