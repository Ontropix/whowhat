using System;
using System.Net;
using System.Threading.Tasks;
using Caliburn.Micro;
using RestSharp;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public static class RestClientExtensions
    {
        public static async Task<T> ExecuteTask<T>(this RestClient client, RestRequest request) where T : Services.Model.RestResponse, new()
        {
            var tcs = new TaskCompletionSource<IRestResponse<T>>();

            //Prevent caching on the client
            request.AddHeader("If-Modified-Since", DateTime.UtcNow.ToString("r"));

            client.ExecuteAsync<T>(request, tcs.SetResult);
            IRestResponse<T> response = await tcs.Task;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //E.g. Session has expired
                SingOuter singOuter = IoC.Get<SingOuter>();
                singOuter.SignOut();
                return default (T);
            }

            GuardError(request, response);

            return response.Data;
        }

        private static void GuardError<T>(RestRequest request, IRestResponse<T> response) where T : Services.Model.RestResponse
        {
            Services.Model.RestResponse data = response.Data;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new RestException("HttpException. Status Code = " + response.StatusCode)
                {
                    ErrorCode = response.StatusCode.ToString(),
                    StatusCode = response.StatusCode,
                    Resource = request.Resource
                };
            }

            if (data != null && data.ResponseStatus != null)
            {
                var status = data.ResponseStatus;

                throw new RestException(status.Message)
                {
                    ErrorCode = status.ErrorCode,
                    StatusCode = response.StatusCode,
                    Resource = request.Resource
                };
            }
  
        }
    }

    public class RestException : Exception
    {
        public string Resource { get; set; }
        public string ErrorCode { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public RestException(string message): base(message)
        {
        }
    }

}
