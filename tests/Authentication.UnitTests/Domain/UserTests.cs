using Authentication.Domain.Aggregates;
using BuildingBlocks.Infrastructure.Exceptions;
using BuildingBlocks.Utils;

namespace Authentication.UnitTests.Domain
{
    public class UserTests
    {

        [Fact]
        public void Create_User_Valid()
        {
            var userEmail = "test@rentalapp.com";
            var fullName = "test full name";
            var birthday = DateTime.Today.ToDateOnly().AddYears(-20);

            var user = new User(userEmail, fullName, birthday);

            Assert.Equal(userEmail, user.Email);
            Assert.Equal(fullName, user.FullName);
            Assert.Equal(birthday, user.Birthday);
            Assert.True(user.Active);
            Assert.NotEqual(Guid.Empty, user.Id);
        }


        [Fact]
        public void Create_User_InValid_Email()
        {
            var userEmail = "test@rentalapp";
            var fullName = "test full name";
            var birthday = DateTime.Today.ToDateOnly().AddYears(-20);

            Assert.Throws<DomainException>(() => new User(userEmail, fullName, birthday));
        }
    }
}
