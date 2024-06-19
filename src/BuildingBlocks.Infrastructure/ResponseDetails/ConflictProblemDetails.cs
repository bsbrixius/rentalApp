using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class ConflictProblemDetails : ProblemDetailsBase
    {
        public ConflictProblemDetails(string? detailsMessage = null)
        {
            Title = "Conflict";
            Status = StatusCodes.Status409Conflict;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
            Detail = detailsMessage;
        }
    }
}
