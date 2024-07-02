using Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Commands.Renter
{
    public sealed class UpdateRenterCNHCommand : IRequest
    {
        public Guid UserId { get; init; }
        public string Number { get; init; }
        public CNHCategoryType CNHType { get; init; }
        public IFormFile CNHFile { get; init; }
        internal sealed class UpdateRenterCNHCommandHandler : IRequestHandler<UpdateRenterCNHCommand>
        {
            public UpdateRenterCNHCommandHandler()
            {
            }

            public async Task Handle(UpdateRenterCNHCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
