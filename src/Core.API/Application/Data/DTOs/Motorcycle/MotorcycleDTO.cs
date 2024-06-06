namespace Core.API.Application.Data.DTOs.Motorcycle
{
    public class MotorcycleDTO()
    {
        public Guid Id { get; set; }
        public uint Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
    }
}
