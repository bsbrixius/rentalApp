using Ardalis.GuardClauses;

namespace Core.Domain.Aggregates.Rent
{

    public static class RentPlans
    {
        public class Plan
        {
            public readonly string Name;
            public readonly int Days;
            public readonly int DailyPriceInCents;
            public readonly decimal UnusedDaysPenaltyPercentage;
            public Plan(int days, int dailyPriceInCents, decimal unusedDaysPenaltyPercentage, string name)
            {
                Days = days;
                DailyPriceInCents = dailyPriceInCents;
                UnusedDaysPenaltyPercentage = unusedDaysPenaltyPercentage;
                Name = name.Replace('_', ' ');
            }
            public int PriceInCents => Days * DailyPriceInCents;
            public int? PenaltyPriceInCents(int expectedDays)
            {
                if (expectedDays < Days)
                {
                    var unusedDays = Days - expectedDays;
                    var penalty = (int)(DailyPriceInCents * UnusedDaysPenaltyPercentage) * unusedDays;

                    return penalty;
                }
                else if (expectedDays > Days)
                {
                    var extraDays = expectedDays - Days;
                    var penalty = extraDays * PenaltyPriceInCentsPerDay;
                    return penalty;
                }
                return null;
            }

            public int TotalPriceWithPenaltyInCents(int totalRentDays)
            {
                Guard.Against.NegativeOrZero(totalRentDays, nameof(totalRentDays));

                if (totalRentDays < Days)
                {
                    var price = totalRentDays * DailyPriceInCents;
                    var unusedDays = Days - totalRentDays;
                    var penalty = (int)(DailyPriceInCents * UnusedDaysPenaltyPercentage) * unusedDays;

                    return price + penalty;
                }
                else if (totalRentDays > Days)
                {
                    var price = PriceInCents;
                    var extraDays = totalRentDays - Days;
                    var penalty = extraDays * PenaltyPriceInCentsPerDay;
                    return price + penalty;
                }
                return PriceInCents;
            }
        }
        public static readonly int PenaltyPriceInCentsPerDay = 5000;

        private static Plan Plan_7_Days => new Plan(7, 3000, 0.20m, nameof(Plan_7_Days));
        private static Plan Plan_15_Days => new Plan(15, 2800, 0.40m, nameof(Plan_15_Days));
        private static Plan Plan_30_Days => new Plan(30, 2200, 0.60m, nameof(Plan_30_Days));
        private static Plan Plan_45_Days => new Plan(45, 2000, 0.70m, nameof(Plan_45_Days));
        private static Plan Plan_50_Days => new Plan(50, 1800, 0.70m, nameof(Plan_50_Days));

        public static readonly Dictionary<int, Plan> PlanByDays = new Dictionary<int, Plan>
        {
            {Plan_7_Days.Days,Plan_7_Days},
            {Plan_15_Days.Days,Plan_15_Days},
            {Plan_30_Days.Days,Plan_30_Days},
            {Plan_45_Days.Days,Plan_45_Days},
            {Plan_50_Days.Days,Plan_50_Days}
        };
    }
}
