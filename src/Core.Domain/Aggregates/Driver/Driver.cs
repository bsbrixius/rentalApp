using BuildingBlocks.Domain.Base;

namespace Core.Domain.Aggregates.Driver
{
    public class Driver : AggregateRoot
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime Birthdate { get; set; }
        public string CNH { get; set; }//ValueObject CNH
        public string CNHCategory { get; set; }//ValueObject CNH
        public string CNHUrl { get; set; }//ValueObject CNH
    }
}
