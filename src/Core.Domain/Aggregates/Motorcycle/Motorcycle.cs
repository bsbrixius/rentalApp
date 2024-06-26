using BuildingBlocks.Domain.Base;
using Core.Domain.Events.Motorcycle;

namespace Core.Domain.Aggregates.Motorcycle
{
    public class Motorcycle : AuditableEntity
    {
        string oldPattern = @"^[A-Z]{3}-[0-9]{4}$";
        string pattern = @"^[A-Z]{3}[0-9][A-Z][0-9]{2}$";


        public Motorcycle(uint year, string model, string plate) : base()
        {
            Year = year;
            Model = model;
            Plate = plate;
            AddMotorcycleRegisteredDomainEvent();
        }


        public uint Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }//ValueObject -> Modelo Novo / Antigo

        #region Domain Events
        private void AddMotorcycleRegisteredDomainEvent()
        {
            AddDomainEvent(new MotorcycleRegisteredDomainEvent(this));
        }
        #endregion
    }
}
