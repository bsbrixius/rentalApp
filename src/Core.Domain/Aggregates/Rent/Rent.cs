using BuildingBlocks.Domain.Base;
using BuildingBlocks.Infrastructure.Exceptions;
using Core.Domain.Enums;

namespace Core.Domain.Aggregates.Rent
{
    public class Rent : AggregateRoot
    {
        public Rent() : base()
        {

        }

        public Rent(DateOnly startAt, DateOnly endAt, DateOnly expectedReturnAt) : this()
        {
            if (startAt > endAt)
                throw new DomainException("Start date must be before end date");

            if (startAt > expectedReturnAt)
                throw new DomainException("Start date must be before expected return date");

            Status = RentStatusType.Pending;
            StartAt = startAt;
            EndAt = endAt;
            ExpectedReturnAt = expectedReturnAt;
            CalculateCosts();
        }

        public RentStatusType Status { get; private set; }
        public DateOnly StartAt { get; private set; }
        public DateOnly EndAt { get; private set; }
        public DateOnly ExpectedReturnAt { get; private set; }
        public int DailyPriceInCents { get; private set; }
        public int PriceInCents { get; private set; }
        public int? PenaltyPriceInCents { get; private set; }
        public Guid MotorcycleId { get; set; }
        public virtual Motorcycle.Motorcycle Motorcycle { get; }
        public Guid RenterId { get; set; }
        public virtual Renter.Renter Renter { get; }

        public void CalculateCosts()
        {
            var totalDays = EndAt.DayNumber - StartAt.DayNumber;
            var expectedDays = ExpectedReturnAt.DayNumber - StartAt.DayNumber;

            if (!RentPlans.PlanByDays.ContainsKey(totalDays))
            {
                throw new DomainException($"There is no plan defined for {totalDays} days.");
            }
            var selectedPlan = RentPlans.PlanByDays[totalDays];
            DailyPriceInCents = selectedPlan.DailyPriceInCents;
            PriceInCents = selectedPlan.TotalPriceWithPenaltyInCents(expectedDays);
            PenaltyPriceInCents = selectedPlan.PenaltyPriceInCents(expectedDays);
        }
    }
}
