using WebApi.Domain.Enum;

namespace WebApi.Domain.Response
{
    public interface IResponse<T>
    {
        string Description { get; set; }
        RequestStatus Status { get; set; }
        T? Value { get; set; }
        IEnumerable<T> Values { get; set; }
    }
}
