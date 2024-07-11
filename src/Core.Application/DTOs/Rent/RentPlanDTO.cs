namespace Core.Application.DTOs.Rent
{
    public class RentPlanDTO
    {
        public string Name { get; set; }
        public int Days { get; set; }
        public int DailyPriceInCents { get; set; }
        public decimal UnusedDaysPenaltyPercentage { get; set; }
    }
}
