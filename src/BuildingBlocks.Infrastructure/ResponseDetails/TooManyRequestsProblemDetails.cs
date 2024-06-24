using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class TooManyRequestsProblemDetails : ProblemDetailsBase
    {
        public TooManyRequestsProblemDetails()
        {
            Title = "Too Many Requests";
            Status = StatusCodes.Status429TooManyRequests;
            Type = "https://datatracker.ietf.org/doc/html/rfc6585#section-4";
        }
    }
}
