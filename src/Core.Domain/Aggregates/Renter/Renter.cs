using BuildingBlocks.Domain.Base;
using BuildingBlocks.Infrastructure.Exceptions;
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
            UpdateBirthdate(birthdate);
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; private set; }
        public DateOnly Birthdate { get; private set; }
        public CNH? CNH { get; private set; }
        public virtual List<Rent.Rent> Rents { get; set; }
        public Guid? CurrentMotorcycleId { get; set; }
        public virtual Motorcycle.Motorcycle? CurrentMotorcycle { get; set; }

        public void UpdateBirthdate(DateOnly birthdate)
        {
            var age = (DateTimeOffset.UtcNow.Year - birthdate.Year);
            if (age < 18)
            {
                throw new DomainException("Renter must be at least 18 years old.");
            }
            Birthdate = birthdate;
        }

        public void UpdateCNPJ(string cnpj)
        {
            CNPJ = cnpj;
        }

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
