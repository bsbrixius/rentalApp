using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class ValidationProblemDetails : ProblemDetailsBase
    {
        public ValidationProblemDetails()
        {
            Title = "Entity failed validation";
            Status = StatusCodes.Status400BadRequest;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
        }

        public IDictionary<string, ICollection<string>> Errors { get; set; }
    }
}
