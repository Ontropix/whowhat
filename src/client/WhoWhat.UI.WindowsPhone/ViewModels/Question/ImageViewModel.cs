using System;
using Caliburn.Micro;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Question
{
    public class ImageViewModel: Screen
    {
        public Uri Image { get; set; }

        private string body;
        public string Body
        {
            get { return body; }
            set
            {
                if (value == body) return;
                body = value;
                NotifyOfPropertyChange(() => Body);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!string.IsNullOrEmpty(Body))
            {
                Body = Body.Substring(0, Math.Min(128, Body.Length));
            }
        }
    }
}
