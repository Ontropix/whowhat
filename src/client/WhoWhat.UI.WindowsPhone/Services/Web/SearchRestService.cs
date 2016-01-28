using System.Threading.Tasks;
using RestSharp;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class SearchRestService : RestService
    {
        public SearchRestService(AppSettings appSettings)
            : base(appSettings)
        {
        }

        public async Task<QuestionSummaryResponse> SearchByKeyword (string term, int skip, int take)
        {
            RestClient client = new RestClient(AppSettings.ApiUri);

            RestRequest request = new RestRequest("search/keyword/{term}", Method.GET);
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("term", term, ParameterType.UrlSegment);

            return await client.ExecuteTask<QuestionSummaryResponse>(request);
        }


        public async Task<QuestionSummaryResponse> SearchByTag(string tag, int skip, int take)
        {
            RestClient client = new RestClient(AppSettings.ApiUri);

            RestRequest request = new RestRequest("search/tagged/{tag}", Method.GET);
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("tag", tag, ParameterType.UrlSegment);

            return await client.ExecuteTask<QuestionSummaryResponse>(request);
        }


    }
}

