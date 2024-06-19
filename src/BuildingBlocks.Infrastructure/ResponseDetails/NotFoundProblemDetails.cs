using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class NotFoundProblemDetails : ProblemDetailsBase
    {
        public NotFoundProblemDetails(string? detailsMessage = null)
        {
            Title = "Not found";
            Status = StatusCodes.Status404NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Detail = detailsMessage;
        }
    }
}
