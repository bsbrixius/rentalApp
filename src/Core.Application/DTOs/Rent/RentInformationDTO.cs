namespace Core.Application.DTOs.Rent
{
    public class RentInformationDTO
    {
        public DateOnly StartAt { get; init; }
        public DateOnly EndAt { get; init; }
        public DateOnly ExpectedReturnAt { get; init; }
        public int DailyPriceInCents { get; init; }
        public int PriceInCents { get; init; }
        public int? PenaltyPriceInCents { get; init; }
    }
}
