using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;

namespace WhoWhat.UI.WindowsPhone.Core
{
    public class AppSettings
    {

        public event EventHandler NotificationsCountChanged;

        public AppSettings()
        {
            var settings = XDocument.Load("settings.xml").Descendants("settng")
                .ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);

            FacebookAuthUri = settings["FacebookAuthUri"];
            VkAuthUri = settings["VkAuthUri"];
            ApiUri = settings["ApiUri"];
            AuthSucceessUri = settings["AuthSucceessUri"];
        }

        public string VkAuthUri { get; private set; }
        public string FacebookAuthUri { get; private set; }
        public string ApiUri { get; private set; }

        public string AuthSucceessUri { get; private set; }

        private static readonly IsolatedStorageSettings Settings = IsolatedStorageSettings.ApplicationSettings;

        public string SsId
        {
            get { return GetSetting("SsId", string.Empty); }
            set { SaveSetting("SsId", value); }
        }

        public string SsPid
        {
            get { return GetSetting("SsPid", string.Empty); }
            set { SaveSetting("SsPid", value); }
        }

        public string UserId
        {
            get { return GetSetting("UserId", string.Empty); }
            set { SaveSetting("UserId", value); }
        }

        public bool IsPushEnabled
        {
            get { return GetSetting("IsPushEnabled", false); }
            set { SaveSetting("IsPushEnabled", value); }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(SsId) && !string.IsNullOrEmpty(SsPid); }
        }

        public bool IsFirstLaunch
        {
            get { return GetSetting("IsFirstLaunch", true); }
            set { SaveSetting("IsFirstLaunch", value); }
        }

        public int MinQuestionLength
        {
            get { return 15; }
        }

        public int MinAnswerLength
        {
            get { return 4; }
        }

        public int NotificationsCount
        {
            get { return GetSetting("NotificationsCount", 0); }
            set
            {
                int count = GetSetting("NotificationsCount", 0);
                if (count != value)
                {
                    SaveSetting("NotificationsCount", value);
                    if (NotificationsCountChanged != null)
                    {
                        NotificationsCountChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private static void SaveSetting<TValue>(string settingName, TValue value)
        {
            if (!Settings.Contains(settingName))
                Settings.Add(settingName, value);
            else
                Settings[settingName] = value;

            Settings.Save();
        }

        private static TValue GetSetting<TValue>(string settingName, TValue value)
        {
            if (Settings.Contains(settingName))
            {
                return (TValue)Settings[settingName];
            }

            return value;
        }

    }
}
