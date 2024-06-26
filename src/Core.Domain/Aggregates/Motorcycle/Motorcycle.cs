﻿using BuildingBlocks.Domain.Base;
using BuildingBlocks.Infrastructure.Exceptions;
using Core.Domain.Events.Motorcycle;
using System.Text.RegularExpressions;

namespace Core.Domain.Aggregates.Motorcycle
{
    public class Motorcycle : AggregateRoot
    {
        string oldPattern = @"^[A-Z]{3}[0-9]{4}$";
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
        public string Plate { get; private set; }

        public void SetPlate(string newPlate)
        {
            if (Regex.IsMatch(newPlate, oldPattern) || Regex.IsMatch(newPlate, pattern))
            {
                Plate = newPlate;
            }
            else
            {
                throw new DomainException("Invalid plate");
            }
        }

        #region Domain Events
        private void AddMotorcycleRegisteredDomainEvent()
        {
            AddDomainEvent(new MotorcycleRegisteredDomainEvent(this));
        }
        #endregion
    }
}
