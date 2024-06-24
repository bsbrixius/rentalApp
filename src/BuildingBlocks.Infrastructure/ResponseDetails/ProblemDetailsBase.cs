using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class ProblemDetailsBase : ProblemDetails
    {
        public string? CorrelationId { get; set; }
        public string? TraceId { get; set; }
    }
}
