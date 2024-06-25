namespace Core.Application.Data.DTOs.Motorcycle
{
    public record MotorcycleDTO()
    {
        public Guid Id { get; set; }
        public uint Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public static MotorcycleDTO From(Domain.Motorcycle motorcycle)
        {
            return new MotorcycleDTO
            {
                Id = motorcycle.Id,
                Year = motorcycle.Year,
                Model = motorcycle.Model,
                Plate = motorcycle.Plate
            };
        }
    }
}
