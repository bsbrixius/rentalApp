using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.ResponseDetails
{
    public class InternalServerErrorProblemDetails : ProblemDetailsBase
    {

        public InternalServerErrorProblemDetails()
        {
            Title = "Internal server error";
            Status = StatusCodes.Status500InternalServerError;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
        }

        public InternalServerErrorProblemDetails(Exception exception)
        {
            Title = "Internal server error";
            Status = StatusCodes.Status500InternalServerError;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
            Exception = new ExceptionResponse(exception);
        }

        public ExceptionResponse? Exception { get; set; }
    }

    public class ExceptionResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public object RequestBody { get; set; }

        public ExceptionResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            InnerException = ex.InnerException?.Message;
            StackTrace = ex.ToString();
        }
    }
}
