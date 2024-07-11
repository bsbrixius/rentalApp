using BuildingBlocks.Domain.Base;
using Core.Domain.ValueObjects;

namespace Core.Domain.Aggregates.Renter
{
    public class Renter : AggregateRoot
    {
        public Renter() : base()
        {
            Rents = new List<Rent.Rent>();
        }

        public Renter(Guid userId, string name, string CNPJ, DateOnly birthdate) : this()
        {
            UserId = userId;
            Name = name;
            this.CNPJ = CNPJ;
            Birthdate = birthdate;
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateOnly Birthdate { get; set; }
        public CNH? CNH { get; set; }
        public virtual List<Rent.Rent> Rents { get; set; }
        public Guid? CurrentMotorcycleId { get; set; }
        public virtual Motorcycle.Motorcycle? CurrentMotorcycle { get; set; }

        public void UpdateCNH(CNH cnh)
        {
            CNH = cnh;
        }

        public void AddRent(Rent.Rent rent)
        {
            Rents.Add(rent);
        }

        public void SetCurrentMotorcycle(Motorcycle.Motorcycle motorcycle)
        {
            CurrentMotorcycleId = motorcycle.Id;
        }
    }
}
