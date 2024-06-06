using MediatR;

namespace BuildingBlocks.API.Core.Application.Decorators.Base
{
    public interface ICommandHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest

    {
        Task Handle(TRequest request);
    }
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

    {
        Task Handle(TRequest request);
    }
}
