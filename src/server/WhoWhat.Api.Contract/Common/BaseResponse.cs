using System;
using ServiceStack.ServiceInterface.ServiceModel;

namespace WhoWhat.Api.Contract
{
    public abstract class BaseResponse
    {
        protected BaseResponse()
        {
            this.ServerTimeUtc = DateTime.UtcNow;
        }

        public ResponseStatus ResponseStatus { get; set; }
        public DateTime ServerTimeUtc { get; set; }
    }
}
