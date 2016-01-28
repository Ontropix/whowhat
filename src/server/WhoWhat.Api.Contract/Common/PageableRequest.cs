namespace WhoWhat.Api.Contract
{
    public abstract class PageableRequest<T> : BaseRequest<T> where T : BaseResponse
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}