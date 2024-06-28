using BuildingBlocks.Domain.Base;

namespace Core.Domain.Aggregates.Renter
{
    public class Renter : AggregateRoot
    {
        public Renter() : base()
        {
            Rents = new List<Rent.Rent>();
        }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateOnly Birthdate { get; set; }
        public string CNH { get; set; }//ValueObject CNH
        public string CNHCategory { get; set; }//ValueObject CNH
        public string CNHUrl { get; set; }//ValueObject CNH
        public virtual List<Rent.Rent> Rents { get; set; }
    }
}
