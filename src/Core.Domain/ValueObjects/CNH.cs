using BuildingBlocks.Domain.Base;
using Core.Domain.Enums;

namespace Core.Domain.ValueObjects
{
    public class CNH : ValueObject
    {
        protected CNH() { }
        public CNH(CNHCategoryType type, string number, string url)
        {
            Type = type;
            Number = number;
            Url = url;
        }
        public string Number { get; private set; }
        public CNHCategoryType Type { get; private set; }
        public string Url { get; private set; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return Number;
            yield return Url;
        }
    }
}
