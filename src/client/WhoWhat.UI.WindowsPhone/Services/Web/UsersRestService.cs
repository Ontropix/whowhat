using System;
using System.Threading.Tasks;
using Microsoft.Phone.Info;
using RestSharp;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class UsersRestService : RestService
    {
        public UsersRestService(AppSettings appSettings)
            : base(appSettings)
        {
        }
        public async Task<ProfileResponse> Profile(string userId)
        {
            RestClient client = base.CreateAuthClient();

            RestRequest request = new RestRequest(string.Format("users/{0}/profile", userId), Method.GET);

            return await client.ExecuteTask<ProfileResponse>(request);
        }

        public Task<ProfileResponse> ProfileMe()
        {
            return Profile("me");
        }

        public async Task<QuestionSummaryResponse> Questions(string userId, int skip, int take)
        {
            RestClient client = CreateAuthClient();

            RestRequest request = new RestRequest(string.Format("/users/{0}/questions", userId), Method.GET);
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);

            return await client.ExecuteTask<QuestionSummaryResponse>(request);
        }

        public Task<QuestionSummaryResponse> QuestionsMe(int skip, int take)
        {
            return Questions("me", skip, take);
        }

        public async Task<QuestionSummaryResponse> Answers(string userId, int skip, int take)
        {
            RestClient client = CreateAuthClient();

            RestRequest request = new RestRequest(string.Format("users/{0}/answers", userId), Method.GET);
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);

            return await client.ExecuteTask<QuestionSummaryResponse>(request);
        }

        public Task<QuestionSummaryResponse> AnswersMe(int skip, int take)
        {
            return Answers("me", skip, take);
        }

        public async Task<Model.RestResponse> PushSubscribe(Uri token)
        {
            RestClient client = CreateAuthClient();
            RestRequest request = new RestRequest("/users/me/push-subscribe", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(
                new
                {
                    DeviceId = GetDeviceId(),
                    DeviceOs = "WP",
                    Token = token.ToString()
                }
            );

            return await client.ExecuteTask<Model.RestResponse>(request);
        }

        public async Task<Model.RestResponse> PushUnsubscribe()
        {
            RestClient client = CreateAuthClient();
            RestRequest request = new RestRequest("/users/me/push-unsubscribe", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(
                new
                {
                    DeviceId = GetDeviceId()
                }
            );

            return await client.ExecuteTask<Model.RestResponse>(request);
        }

        private string GetDeviceId()
        {
            byte[] bytes = (byte[]) DeviceExtendedProperties.GetValue("DeviceUniqueId");
            return Convert.ToBase64String(bytes);
        }


        public async Task<NotificationsCountResponse> NotificationsCount()
        { 
            RestClient client = CreateAuthClient();
            RestRequest request = new RestRequest("users/me/notifications-count", Method.GET);
            return await client.ExecuteTask<NotificationsCountResponse>(request);
        }

        public async Task<NotificationsResponse> NotificationsMe(int skip, int take)
        {
            RestClient client = CreateAuthClient();
            RestRequest request = new RestRequest("/users/me/notifications", Method.GET);
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);

            return await client.ExecuteTask<NotificationsResponse>(request);
        }

    }
}
