using BuildingBlocks.API.Core.Application.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.API.Core.Application.Decorators
{
    public sealed class DatabaseRetryDecorator<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IRequestHandler<TRequest> _handler;
        private readonly DatabaseRetrySettings _databaseRetrySettings;

        public DatabaseRetryDecorator(ILogger<TRequest> logger, IRequestHandler<TRequest> handler, IOptions<DatabaseRetrySettings> databaseRetrySettings)
        {
            _logger = logger;
            _handler = handler;
            _databaseRetrySettings = databaseRetrySettings.Value;
        }

        public async Task Handle(TRequest request, CancellationToken cancellationToken)
        {

            for (int i = 0; ; i++)
            {
                try
                {
                    await _handler.Handle(request, cancellationToken);
                }
                catch (Exception ex)
                {
                    if (i >= _databaseRetrySettings.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;
                }
            }
        }

        private bool IsDatabaseException(Exception exception)
        {
            string message = exception.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery is not possible")
                || message.Contains("error occurred while establishing a connection");
        }
    }


    public sealed class DatabaseRetryDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IRequestHandler<TRequest, TResponse> _handler;
        private readonly DatabaseRetrySettings _databaseRetrySettings;

        public DatabaseRetryDecorator(ILogger<TRequest> logger, IRequestHandler<TRequest, TResponse> handler, IOptions<DatabaseRetrySettings> databaseRetrySettings)
        {
            _logger = logger;
            _handler = handler;
            _databaseRetrySettings = databaseRetrySettings.Value;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {

            for (int i = 0; ; i++)
            {
                try
                {
                    return await _handler.Handle(request, cancellationToken);
                }
                catch (Exception ex)
                {
                    if (i >= _databaseRetrySettings.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;
                }
            }
        }

        private bool IsDatabaseException(Exception exception)
        {
            string message = exception.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery is not possible")
                || message.Contains("error occurred while establishing a connection");
        }
    }
}
