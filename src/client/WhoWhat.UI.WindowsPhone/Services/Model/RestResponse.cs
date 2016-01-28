using System;
using System.Collections.Generic;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    public class RestResponse
    {
        public ResponseStatus ResponseStatus { get; set; }

        public DateTime ServerTimeUtc { get; set; }
    }

    public class ResponseStatus
    {
        public string ErrorCode { get; set; }

        public List<ResponseError> Errors { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }

    public class ResponseError
    {
        public string ErrorCode { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
