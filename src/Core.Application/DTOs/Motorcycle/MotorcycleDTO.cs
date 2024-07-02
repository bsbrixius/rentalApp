namespace Core.Application.DTOs.Motorcycle
{
    public record MotorcycleDTO()
    {
        public Guid Id { get; set; }
        public uint Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public static MotorcycleDTO From(Domain.Aggregates.Motorcycle.Motorcycle motorcycle)
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
