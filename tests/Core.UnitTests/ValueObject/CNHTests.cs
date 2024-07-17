using Core.Domain.Enums;
using Core.Domain.ValueObjects;

namespace Core.UnitTests.ValueObject
{
    public class CNHTests
    {
        [Fact]
        public void Create_CNH_Valid()
        {
            var type = CNHCategoryType.A;
            var number = "12345678901";
            var url = "http://url.com";

            var cnh = new CNH(type, number, url);

            Assert.Equal(type, cnh.Type);
            Assert.Equal(number, cnh.Number);
            Assert.Equal(url, cnh.Url);
        }
    }
}
