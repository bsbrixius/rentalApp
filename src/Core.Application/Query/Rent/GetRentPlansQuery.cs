using Core.Application.DTOs.Rent;
using Core.Domain.Aggregates.Rent;
using MediatR;

namespace Core.Application.Query.Rent
{
    public sealed class GetRentPlansQuery : IRequest<IEnumerable<RentPlanDTO>>
    {
        internal sealed class GetRentPlansQueryHandler : IRequestHandler<GetRentPlansQuery, IEnumerable<RentPlanDTO>>
        {
            public async Task<IEnumerable<RentPlanDTO>> Handle(GetRentPlansQuery request, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new List<RentPlanDTO>(RentPlans.PlanByDays.Values.Select(x => new RentPlanDTO()
                {
                    Name = x.Name,
                    Days = x.Days,
                    DailyPriceInCents = x.DailyPriceInCents,
                    UnusedDaysPenaltyPercentage = x.UnusedDaysPenaltyPercentage
                })));
            }
        }
    }
}
