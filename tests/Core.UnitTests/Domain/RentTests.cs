using BuildingBlocks.Infrastructure.Exceptions;
using BuildingBlocks.Utils;
using Core.Domain.Aggregates.Rent;

namespace Core.UnitTests.Domain
{
    public class RentTests
    {
        [Fact]
        public void Create_Rent_Valid()
        {
            var startDate = DateTime.UtcNow.AddDays(-15).ToDateOnly();
            var endDate = DateTime.UtcNow.ToDateOnly();
            var expectedReturnAt = DateTime.UtcNow.ToDateOnly();

            var rent = new Rent(startDate, endDate, expectedReturnAt);
        }

        [Fact]
        public void Create_Rent_Invalid_Plan()
        {
            var startDate = DateTime.UtcNow.AddDays(-99).ToDateOnly();
            var endDate = DateTime.UtcNow.ToDateOnly();
            var expectedReturnAt = DateTime.UtcNow.ToDateOnly();

            Assert.Throws<DomainException>(() => new Rent(startDate, endDate, expectedReturnAt));
        }

        [Fact]
        public void Create_Rent_Invalid_StartDate_EndDate()
        {
            var startDate = DateTime.UtcNow.AddDays(+15).ToDateOnly();
            var endDate = DateTime.UtcNow.ToDateOnly();
            var expectedReturnAt = DateTime.UtcNow.ToDateOnly();

            Assert.Throws<DomainException>(() => new Rent(startDate, endDate, expectedReturnAt));
        }

        [Fact]
        public void Create_Rent_Invalid_StartDate_ExpectedReturnAt()
        {
            var startDate = DateTime.UtcNow.AddDays(-15).ToDateOnly();
            var endDate = DateTime.UtcNow.ToDateOnly();
            var expectedReturnAt = DateTime.UtcNow.AddDays(-20).ToDateOnly();

            Assert.Throws<DomainException>(() => new Rent(startDate, endDate, expectedReturnAt));
        }
    }
}
