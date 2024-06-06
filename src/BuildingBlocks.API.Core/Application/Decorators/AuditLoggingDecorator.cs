using BuildingBlocks.API.Core.Application.Decorators.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.API.Core.Application.Decorators
{
    public sealed class AuditLoggingDecorator<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IRequestHandler<TRequest> _handler;

        public AuditLoggingDecorator(ILogger<TRequest> logger, IRequestHandler<TRequest> handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task Handle(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command of type {requestName}: {request}", request.GetType().Name, request);

            await _handler.Handle(request, cancellationToken);
        }
    }
    public sealed class AuditLoggingDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ICommandHandler<TRequest, TResponse> _handler;

        public AuditLoggingDecorator(ILogger<TRequest> logger, ICommandHandler<TRequest, TResponse> handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command of type {requestName}: {request}", request.GetType().Name, request);

            return await _handler.Handle(request, cancellationToken);
        }
    }
}