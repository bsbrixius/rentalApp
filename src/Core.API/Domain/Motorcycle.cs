using BuildingBlocks.Infrastructure.Base;
using Core.API.Domain.Events.Motorcycle;

namespace Core.API.Domain
{
    public class Motorcycle : AuditableEntity
    {
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
