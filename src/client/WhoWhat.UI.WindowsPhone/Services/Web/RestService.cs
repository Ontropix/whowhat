using System;
using System.Net;
using RestSharp;
using WhoWhat.UI.WindowsPhone.Core;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class RestService
    {
        protected readonly AppSettings AppSettings;

        public RestService(AppSettings appSettings)
        {
            this.AppSettings = appSettings;
        }

        /// <summary>
        /// Create a client to make requests requiring authentication
        /// </summary>
        /// <returns></returns>
        protected RestClient CreateAuthClient()
        {
            RestClient client = CreateClient();

            CookieContainer container = new CookieContainer();
            container.Add(new Uri(AppSettings.ApiUri), new Cookie("ss-id", AppSettings.SsId));
            container.Add(new Uri(AppSettings.ApiUri), new Cookie("ss-pid", AppSettings.SsPid));
            client.CookieContainer = container;

            return client;
        }

        protected RestClient CreateClient()
        {
            RestClient client = new RestClient(AppSettings.ApiUri);
            return client;
        }
    }
}
