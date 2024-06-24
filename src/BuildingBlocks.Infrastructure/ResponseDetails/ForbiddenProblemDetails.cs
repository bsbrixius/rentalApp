using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class ForbiddenProblemDetails : ProblemDetailsBase
    {
        public ForbiddenProblemDetails(string? detailsMessage = null)
        {
            Title = "Forbidden";
            Status = StatusCodes.Status403Forbidden;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
            Detail = detailsMessage;
        }
    }
}
