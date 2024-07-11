namespace Core.Application.DTOs.Rent
{
    public class RentMotorcycleRequest
    {
        //public DateOnly StartAt { get; init; }
        public DateOnly EndAt { get; init; }
        public DateOnly ExpectedReturnAt { get; init; }
    }
}
