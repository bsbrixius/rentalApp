using BuildingBlocks.Utils;
using Core.Application.DTOs.Rent;
using FluentValidation;
using MediatR;

namespace Core.Application.Query.Rent
{
    public sealed class GetRentInformationQuery : IRequest<RentInformationDTO>
    {
        public DateOnly StartAt { get; init; }
        public DateOnly EndAt { get; init; }
        public DateOnly ExpectedReturnAt { get; init; }

        internal sealed class GetRentInformationQueryHandler : IRequestHandler<GetRentInformationQuery, RentInformationDTO>
        {
            public GetRentInformationQueryHandler()
            {
            }
            public async Task<RentInformationDTO> Handle(GetRentInformationQuery request, CancellationToken cancellationToken)
            {
                new GetRentInformationQueryValidator().ValidateAndThrow(request);
                var rent = new Domain.Aggregates.Rent.Rent(request.StartAt, request.EndAt, request.ExpectedReturnAt);
                return await Task.FromResult(new RentInformationDTO()
                {
                    StartAt = rent.StartAt,
                    EndAt = rent.EndAt,
                    ExpectedReturnAt = rent.ExpectedReturnAt,
                    DailyPriceInCents = rent.DailyPriceInCents,
                    PriceInCents = rent.PriceInCents,
                    PenaltyPriceInCents = rent.PenaltyPriceInCents
                });
            }
        }
    }

    public sealed class GetRentInformationQueryValidator : AbstractValidator<GetRentInformationQuery>
    {
        public GetRentInformationQueryValidator()
        {
            RuleFor(x => x.StartAt)
                .GreaterThan(DateTime.Today.AddDays(-1).ToDateOnly());
            RuleFor(x => x.EndAt)
                .GreaterThan(DateTime.Today.AddDays(-1).ToDateOnly())
                .Must((model, endAt) => endAt > model.StartAt)
                    .WithMessage("EndAt must be greater than StartAt");
            RuleFor(x => x.ExpectedReturnAt)
                .GreaterThan(DateTime.Today.AddDays(-1).ToDateOnly())
                .Must((model, expectedReturnAt) => expectedReturnAt > model.StartAt)
                    .WithMessage("ExpectedReturnAt must be greater than StartAt"); ;
        }
    }
}
