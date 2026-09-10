using System.Net;

namespace MyWebApi.Middleware;

public abstract class ApiException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }

    public List<string> Errors { get; }

    protected ApiException(string message, List<string>? errors = null)
        : base(message)
    {
        Errors = errors ?? new List<string>();
    }
}

public class NotFoundException : ApiException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public NotFoundException(string resource, object key)
        : base($"{resource} with id '{key}' was not found")
    {
    }
}
