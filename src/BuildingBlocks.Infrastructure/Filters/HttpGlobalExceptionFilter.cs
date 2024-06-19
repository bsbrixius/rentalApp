using Authentication.API.Domain.Expections;
using BuildingBlocks.Infrastructure.Domain.Exceptions;
using BuildingBlocks.Infrastructure.ResponseDetails;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private const string CorrelationIdHeaderKey = "X-Correlation-Id";
        private string correlationId;

        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;
        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            ArgumentNullException.ThrowIfNull(env, nameof(env));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ForbiddenException:
                    HandleForbiddenException(context);
                    break;
                case BadRequestException:
                    HandleBadRequestException(context);
                    break;
                case ConflictException:
                    HandleConflictException(context);
                    break;
                case NotFoundException:
                    HandleNotFoundException(context);
                    break;
                case UnauthorizedException:
                    HandleUnauthorizedException(context);
                    break;
                case DomainException:
                case ValidationException:
                    HandleInternalServerError(context);
                    break;
            }
            context.ExceptionHandled = true;
        }

        private void HandleForbiddenException(ExceptionContext context)
        {
            var problemDetails = new ForbiddenProblemDetails(context.Exception.Message);
            problemDetails.Instance = context.HttpContext.Request.Path;
            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var problemDetails = new BadRequestProblemDetails(context.Exception.Message);
            problemDetails.Instance = context.HttpContext.Request.Path;
            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.ExceptionHandled = true;
        }

        private void HandleConflictException(ExceptionContext context)
        {
            var problemDetails = new ConflictProblemDetails(context.Exception.Message);
            problemDetails.Instance = context.HttpContext.Request.Path;
            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var problemDetails = new NotFoundProblemDetails(context.Exception.Message);
            problemDetails.Instance = context.HttpContext.Request.Path;
            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedException(ExceptionContext context)
        {
            var problemDetails = new UnauthorizedProblemDetails(context.Exception.Message);
            problemDetails.Instance = context.HttpContext.Request.Path;
            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.ExceptionHandled = true;
        }

        private void HandleInternalServerError(ExceptionContext context)
        {
            context.HttpContext.Items.TryGetValue("body", out object body);
            var problemDetails = _env.IsDevelopment() ?
                new InternalServerErrorProblemDetails(context.Exception) :
                new InternalServerErrorProblemDetails();
            problemDetails.Instance = context.HttpContext.Request.Path;

            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
