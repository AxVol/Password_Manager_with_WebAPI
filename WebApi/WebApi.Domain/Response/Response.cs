using WebApi.Domain.Enum;

namespace WebApi.Domain.Response
{
    public class Response<T> : IResponse<T>
    {
        public string Description { get; set; }

        public RequestStatus Status { get; set; }

        public T? Value { get; set; }
        public IEnumerable<T> Values { get; set; }
    }
}
