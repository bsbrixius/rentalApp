using BuildingBlocks.Domain;

namespace Core.Domain.Domain
{
    public class Motorcycle : Entity
    {
        public uint Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }//ValueObject -> Modelo Novo / Antigo
    }
}
