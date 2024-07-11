namespace Core.Application.DTOs.Rent
{
    public class GetRentInformationRequest
    {
        public DateOnly StartAt { get; init; }
        public DateOnly EndAt { get; init; }
        public DateOnly ExpectedReturnAt { get; init; }
    }
}
