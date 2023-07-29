using WebApi.Domain.Enum;

namespace WebApi.Domain.Response
{
    public interface IResponse<T>
    {
        string Description { get;}
        RequestStatus Status { get;}
        T? Value { get;}
        IEnumerable<T> Values { get;}
    }
}
