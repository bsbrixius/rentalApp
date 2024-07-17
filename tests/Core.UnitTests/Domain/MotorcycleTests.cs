using BuildingBlocks.Infrastructure.Exceptions;
using Core.Domain.Aggregates.Motorcycle;
using Testing.Base.Helpers;

namespace Core.UnitTests.Domain
{
    public class MotorcycleTests
    {

        [Fact]
        public void Create_New_Plate_Motorcycle_Valid()
        {
            var year = 2014u;
            var model = "test model";
            var plate = DataGenerator.GenerateNewPlate();
            var motorcycle = new Motorcycle(year, model, plate);

            Assert.Equal(year, motorcycle.Year);
            Assert.Equal(model, motorcycle.Model);
            Assert.Equal(plate, motorcycle.Plate);
        }

        [Fact]
        public void Create_Old_Plate_Motorcycle_Valid()
        {
            var year = 2014u;
            var model = "test model";
            var plate = DataGenerator.GenerateOldPlate();
            var motorcycle = new Motorcycle(year, model, plate);

            Assert.Equal(year, motorcycle.Year);
            Assert.Equal(model, motorcycle.Model);
            Assert.Equal(plate, motorcycle.Plate);
        }

        [Fact]
        public void Create_Invalid_Plate_Motorcycle_Exception()
        {
            var year = 2014u;
            var model = "test model";
            var plate = "INVALIDPLATE";
            Assert.Throws<DomainException>(() => new Motorcycle(year, model, plate));
        }

    }
}
