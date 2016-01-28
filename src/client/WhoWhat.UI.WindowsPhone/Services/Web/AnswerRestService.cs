using System.Threading.Tasks;
using RestSharp;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class AnswerRestService : RestService
    {
        public AnswerRestService(AppSettings appSettings): base(appSettings)
        {
        }

        public Task<VoteAnswerResponse> VoteUp(string answerId)
        {
            return DoAnswerAction<VoteAnswerResponse>(answerId, "vote-up");
        }

        public Task<VoteAnswerResponse> VoteDown(string answerId)
        {
            return DoAnswerAction<VoteAnswerResponse>(answerId, "vote-down");
        }

        public Task<VoteAnswerResponse> Unvote(string answerId)
        {
            return DoAnswerAction<VoteAnswerResponse>(answerId, "unvote");
        }

        public Task<AnswerStatusResponse> MarkAsAccepted(string answerId)
        {
            return DoAnswerAction<AnswerStatusResponse>(answerId, "mark-accepted");
        }

        public Task<AnswerStatusResponse> UnmarkAsAccepted(string answerId)
        {
            return DoAnswerAction<AnswerStatusResponse>(answerId, "unmark-accepted");
        }

        public Task<AnswerStatusResponse> Remove(string answerId)
        {
            return DoAnswerAction<AnswerStatusResponse>(answerId, "remove");
        }

        public async Task<AnswerStatusResponse> ChangeBody(string answerId, string body)
        {
            RestClient client = CreateAuthClient();
            RestRequest request = new RestRequest(string.Format("answers/{0}/change-body", answerId), Method.POST);
            request.AddParameter("body", body);
            return await client.ExecuteTask<AnswerStatusResponse>(request);
        } 

        private async Task<T> DoAnswerAction<T>(string answerId, string action) where T: Model.RestResponse, new()
        {
            RestClient client = CreateAuthClient();
            RestRequest request = new RestRequest(string.Format("answers/{0}/{1}", answerId, action), Method.POST);
            return await client.ExecuteTask<T>(request);
        }
    }
}
