using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions;

public class UnauthorizedApiException : BadRequestApiException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public override string ErrorCode => "UNAUTHORIZED";

    public UnauthorizedApiException(string? message = "User not authorized", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}