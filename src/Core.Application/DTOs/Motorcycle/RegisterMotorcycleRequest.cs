﻿namespace Core.Application.DTOs.Motorcycle
{
    public class RegisterMotorcycleRequest
    {
        public required uint Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }
    }
}
