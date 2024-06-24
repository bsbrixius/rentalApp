using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class UnauthorizedProblemDetails : ProblemDetailsBase
    {
        public UnauthorizedProblemDetails(string? detailsMessage = null)
        {
            Title = "Unauthorized";
            Status = StatusCodes.Status401Unauthorized;
            Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
            Detail = detailsMessage;
        }
    }
}
