using Microsoft.AspNetCore.Http;
//TODO In construction
namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class BadRequestProblemDetails : ProblemDetailsBase
    {
        public BadRequestProblemDetails(string? detailsMessage = null)
        {
            Title = "Bad request";
            Status = StatusCodes.Status400BadRequest;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            Detail = detailsMessage;
        }
    }
}
