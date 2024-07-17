using BuildingBlocks.Infrastructure.Exceptions;
using BuildingBlocks.Utils;
using Core.Domain.Aggregates.Renter;
using Core.Domain.Enums;
using Core.Domain.ValueObjects;

namespace Core.UnitTests.Domain
{
    public class RenterTests
    {
        [Fact]
        public void Create_Renter_Valid()
        {
            var userId = Guid.NewGuid();
            var name = "test name";
            var cnpj = "12345678901234";
            var birthdate = new DateOnly(1990, 1, 1);
            var renter = new Renter(userId, name, cnpj, birthdate);

            Assert.Equal(userId, renter.UserId);
            Assert.Equal(name, renter.Name);
            Assert.Equal(cnpj, renter.CNPJ);
            Assert.Equal(birthdate, renter.Birthdate);

            Assert.Null(renter.CNH);
            Assert.Empty(renter.Rents);
            Assert.Null(renter.CurrentMotorcycleId);
            Assert.Null(renter.CurrentMotorcycle);
        }

        [Fact]
        public void UpdateCNPJ_Renter_Valid()
        {
            var userId = Guid.NewGuid();
            var name = "test name";
            var cnpj = "12345678901234";
            var birthdate = new DateOnly(1990, 1, 1);
            var renter = new Renter(userId, name, cnpj, birthdate);

            renter.UpdateCNPJ("12345678901235");
            Assert.Equal("12345678901235", renter.CNPJ);

            Assert.Null(renter.CNH);
            Assert.Empty(renter.Rents);
            Assert.Null(renter.CurrentMotorcycleId);
            Assert.Null(renter.CurrentMotorcycle);
        }

        [Fact]
        public void UpdateCNH_Renter_Valid()
        {
            var userId = Guid.NewGuid();
            var name = "test name";
            var cnpj = "12345678901234";
            var birthdate = new DateOnly(1990, 1, 1);
            var renter = new Renter(userId, name, cnpj, birthdate);

            var cnh = new CNH(CNHCategoryType.A, "12345678901", "www.some-url.com");
            renter.UpdateCNH(cnh);
            Assert.NotNull(renter.CNH);
            Assert.Equal(cnh.Type, renter.CNH.Type);
            Assert.Equal(cnh.Number, renter.CNH.Number);
            Assert.Equal(cnh.Url, renter.CNH.Url);
        }

        [Fact]
        public void UpdateBirthdate_Renter_Valid()
        {
            var userId = Guid.NewGuid();
            var name = "test name";
            var cnpj = "12345678901234";
            var birthdate = new DateOnly(1990, 1, 1);
            var renter = new Renter(userId, name, cnpj, birthdate);

            var newBirthdate = new DateOnly(1991, 1, 1);
            renter.UpdateBirthdate(newBirthdate);

            Assert.Equal(newBirthdate, renter.Birthdate);
        }

        [Fact]
        public void Create_Renter_Invalid_Birthdate()
        {
            var userId = Guid.NewGuid();
            var name = "test name";
            var cnpj = "12345678901234";
            var birthdate = DateTime.UtcNow.AddYears(-12).ToDateOnly();
            Assert.Throws<DomainException>(() => new Renter(userId, name, cnpj, birthdate));
        }

        [Fact]
        public void UpdateBirthdate_Renter_Invalid()
        {
            var userId = Guid.NewGuid();
            var name = "test name";
            var cnpj = "12345678901234";
            var birthdate = new DateOnly(1990, 1, 1);
            var renter = new Renter(userId, name, cnpj, birthdate);

            var newBirthdate = DateTime.UtcNow.AddYears(-12).ToDateOnly();
            Assert.Throws<DomainException>(() => renter.UpdateBirthdate(newBirthdate));
        }

    }
}
