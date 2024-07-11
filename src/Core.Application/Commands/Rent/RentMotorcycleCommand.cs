using BuildingBlocks.Infrastructure.Exceptions;
using BuildingBlocks.Utils;
using Core.Domain.Aggregates.Motorcycle;
using Core.Domain.Aggregates.Rent;
using Core.Domain.Aggregates.Renter;
using Core.Domain.Enums;
using FluentValidation;
using MediatR;

namespace Core.Application.Commands.Rent
{
    public sealed class RentMotorcycleCommand : IRequest
    {
        public Guid UserId { get; set; }
        //public DateOnly StartAt { get; init; }
        public DateOnly EndAt { get; init; }
        public DateOnly ExpectedReturnAt { get; init; }

        internal sealed class RentMotorcycleCommandHandler : IRequestHandler<RentMotorcycleCommand>
        {
            private readonly IRentRepository _rentRepository;
            private readonly IMotorcycleRepository _motorcycleRepository;
            private readonly IRenterRepository _renterRepository;

            public RentMotorcycleCommandHandler(
                IRentRepository rentRepository,
                IMotorcycleRepository motorcycleRepository,
                IRenterRepository renterRepository
                )
            {
                ArgumentNullException.ThrowIfNull(rentRepository, nameof(rentRepository));
                ArgumentNullException.ThrowIfNull(motorcycleRepository, nameof(motorcycleRepository));
                ArgumentNullException.ThrowIfNull(renterRepository, nameof(renterRepository));

                _rentRepository = rentRepository;
                _motorcycleRepository = motorcycleRepository;
                _renterRepository = renterRepository;
            }

            public async Task Handle(RentMotorcycleCommand request, CancellationToken cancellationToken)
            {
                new RentMotorcycleCommandValidator().ValidateAndThrow(request);
                var renter = await _renterRepository.GetByUserIdAsync(request.UserId);
                if (renter == null)
                    throw new NotFoundException($"Renter not found with UserId: {request.UserId}");

                if (renter.CNH == null)
                    throw new NotFoundException($"Renter CNH not found");

                if (renter.CNH.Type == CNHCategoryType.B)
                    throw new ConflictException($"Renter CNH Type is not permited to rent");

                var todayStartAt = DateTime.Today.ToDateOnly();

                var motorcycle = await _motorcycleRepository.GetAvailableAtAsync(todayStartAt);
                if (motorcycle == null)
                    throw new NotFoundException($"No motorcycle available to rent");

                var rent = new Domain.Aggregates.Rent.Rent(todayStartAt, request.EndAt, request.ExpectedReturnAt);

                renter.AddRent(rent);
                renter.SetCurrentMotorcycle(motorcycle);
                motorcycle.Rent(rent);

                await _rentRepository.AddAsync(rent);
                _renterRepository.Update(renter);
                _motorcycleRepository.Update(motorcycle);
                await _rentRepository.CommitAsync();
            }
        }
    }

    public class RentMotorcycleCommandValidator : AbstractValidator<RentMotorcycleCommand>
    {
        public RentMotorcycleCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            //RuleFor(x => x.StartAt)
            //    .GreaterThan(DateTime.Today.AddDays(-1).ToDateOnly());
            RuleFor(x => x.EndAt)
                .GreaterThan(DateTime.Today.AddDays(-1).ToDateOnly());
            //.Must((model, endAt) => endAt > model.StartAt)
            //    .WithMessage("EndAt must be greater than StartAt");
            RuleFor(x => x.ExpectedReturnAt)
                .GreaterThan(DateTime.Today.AddDays(-1).ToDateOnly());
            //.Must((model, expectedReturnAt) => expectedReturnAt > model.StartAt)
            //    .WithMessage("ExpectedReturnAt must be greater than StartAt"); ;
        }
    }
}
