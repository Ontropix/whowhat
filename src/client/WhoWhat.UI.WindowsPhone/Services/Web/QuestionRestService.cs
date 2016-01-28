using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class QuestionRestService : RestService
    {
        public QuestionRestService(AppSettings appSettings) : base(appSettings)
        {
        }

        public Task<QuestionSummaryResponse> Popular(int skip, int take)
        {
            return Feed("popular", skip, take);
        }

        public Task<QuestionSummaryResponse> Live(int skip, int take)
        {
            return Feed("live", skip, take);
        }

        public Task<QuestionSummaryResponse> Unanswered(int skip, int take)
        {
            return Feed("unanswered", skip, take);
        }

        private async Task<QuestionSummaryResponse> Feed(string feed,int skip, int take)
        {
            RestClient client = new RestClient(AppSettings.ApiUri);

            RestRequest request = new RestRequest("questions/" + feed, Method.GET);
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);

            return await client.ExecuteTask<QuestionSummaryResponse>(request);
        }

        public async Task<AskQuestionResponse> AskQuestion(string body, List<string> tags, byte[] bytes)
        {
            RestClient client = CreateAuthClient();

            RestRequest request = new RestRequest("questions/ask", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddBody(
                new
                {
                    Body = body,
                    Tags = tags,
                    Bytes = Convert.ToBase64String(bytes)
                }
            );

            return await client.ExecuteTask<AskQuestionResponse>(request);
        }

        public async Task<QuestionDetailsResponse> Details(string questionId)
        {
            RestClient client = CreateClient();

            RestRequest request = new RestRequest(string.Format("questions/{0}/details", questionId));

            return await client.ExecuteTask<QuestionDetailsResponse>(request);
        }

        public async Task<QuestionStatusResponse> ChangeInfo(string questionId, string body, List<string> tags)
        {
            RestClient client = CreateAuthClient();

            RestRequest request = new RestRequest(string.Format("questions/{0}/change-info", questionId), Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddBody(
                new
                {
                    QuestionId = questionId,
                    Body = body,
                    Tags = tags,
                }
            );

            return await client.ExecuteTask<QuestionStatusResponse>(request);
        }

        public async Task<QuestionStatusResponse> ChangeImage(string questionId,  byte[] bytes)
        {
            RestClient client = CreateAuthClient();

            RestRequest request = new RestRequest(string.Format("questions/{0}/change-image", questionId), Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddBody(
                new
                {
                    QuestionId = questionId,
                    Bytes = Convert.ToBase64String(bytes)
                }
            );

            return await client.ExecuteTask<QuestionStatusResponse>(request);
        }


        public async Task<AnswerStatusResponse> AnswerQuestion(string questionId, string body)
        {
            RestClient client = CreateAuthClient();

            RestRequest request = new RestRequest(string.Format("questions/{0}/answer", questionId), Method.POST);
            request.AddParameter("body", body);

            return await client.ExecuteTask<AnswerStatusResponse>(request);
        }


        #region Question owner actions

        public Task<QuestionStatusResponse> VoteDown(string questionId)
        {
            return DoQuestionAction(questionId, "vote-down");
        }

        public Task<QuestionStatusResponse> VoteUp(string questionId)
        {
            return DoQuestionAction(questionId, "vote-up");
        }

        public Task<QuestionStatusResponse> Unvote(string questionId)
        {
            return DoQuestionAction(questionId, "unvote");
        }

        public Task<QuestionStatusResponse> Remove(string questionId)
        {
            return DoQuestionAction(questionId, "remove");
        } 

        private async Task<QuestionStatusResponse> DoQuestionAction(string questionId, string action)
        {
            RestClient client = CreateAuthClient();

            RestRequest request = new RestRequest(string.Format("questions/{0}/{1}", questionId, action), Method.POST);

            return await client.ExecuteTask<QuestionStatusResponse>(request);
        }

        #endregion
    }
}
