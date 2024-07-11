using BuildingBlocks.Infrastructure.Exceptions;
using Core.Domain.Aggregates.Renter;
using Core.Domain.Enums;
using Core.Domain.ValueObjects;
using Crosscutting.StorageService.Base;
using FluentValidation;
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
            private readonly IRenterRepository _renterRepository;
            private readonly IStorageService _storageService;

            public UpdateRenterCNHCommandHandler(
                IRenterRepository renterRepository,
                IStorageService storageService
                )
            {
                ArgumentNullException.ThrowIfNull(renterRepository, nameof(renterRepository));
                ArgumentNullException.ThrowIfNull(storageService, nameof(storageService));
                _renterRepository = renterRepository;
                _storageService = storageService;
            }

            public async Task Handle(UpdateRenterCNHCommand request, CancellationToken cancellationToken)
            {
                new UpdateRenterCNHCommandValidator().ValidateAndThrow(request);

                if (_renterRepository.HasAnyWith(x => x.CNH != null && x.CNH.Number == request.Number && x.UserId != request.UserId))
                    throw new DomainException($"There is already a CNH with this Number: {request.Number}");

                var renter = await _renterRepository.GetByUserIdAsync(request.UserId);
                if (renter == null)
                    throw new NotFoundException($"Renter not found with UserId: {request.UserId}");

                using (var stream = request.CNHFile.OpenReadStream())
                {
                    var fileName = $"{renter.UserId}/documents/cnh{Path.GetExtension(request.CNHFile.FileName)}";
                    var storageResponse = await _storageService.UploadAsync("renter", fileName, stream, request.CNHFile.ContentType);
                    var cnh = new CNH(request.CNHType, request.Number, storageResponse.Url);
                    renter.UpdateCNH(cnh);
                    _renterRepository.Update(renter);
                    await _renterRepository.CommitAsync();
                }
            }
        }
    }

    public sealed class UpdateRenterCNHCommandValidator : AbstractValidator<UpdateRenterCNHCommand>
    {
        public UpdateRenterCNHCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Number).NotEmpty().NotNull();
            RuleFor(x => x.CNHType).IsInEnum();
            RuleFor(x => x.CNHFile)
                .NotNull()
                .Must(x => x.Length > 0)
                .Must(x => Path.GetExtension(x.FileName) == ".png" || Path.GetExtension(x.FileName) == ".bmp")
                .WithMessage("File must be a .png or .bmp");
        }
    }
}
