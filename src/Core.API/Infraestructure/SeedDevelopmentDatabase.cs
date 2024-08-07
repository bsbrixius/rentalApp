﻿using Core.Data;
using Core.Domain.Aggregates.Motorcycle;

namespace Core.API.Infraestructure
{
    public static class SeedDevelopmentDatabase
    {
        public static async Task TrySeedDevelopmentDatabaseAsync(this CoreContext context)
        {
            await SeedMotorcycles(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedMotorcycles(CoreContext context)
        {
            var motorcycles = new List<Motorcycle>()
            {
                new Motorcycle(2024, "Harley-Davidson Sportster", "XYZ1234"),
                new Motorcycle(2019, "Honda CBR600RR", "ABC5678"),
                new Motorcycle(2022, "Yamaha YZF-R1", "DEF9012"),
                new Motorcycle(2018, "Kawasaki Ninja ZX-10R", "GHI3456"),
                new Motorcycle(2020, "Ducati Panigale V4", "JKL7890")
            };

            foreach (var motorcycle in motorcycles)
            {
                if (!context.Motorcycles.Any(x => x.Plate == motorcycle.Plate))
                {
                    await context.Motorcycles.AddAsync(motorcycle);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
