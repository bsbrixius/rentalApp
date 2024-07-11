using Core.Domain.Enums;

namespace Core.Application.DTOs.Renter
{
    public class RenterDTO
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateOnly Birthdate { get; set; }
        public string? CNH { get; set; }
        public CNHCategoryType? CNHType { get; set; }
        public string? CNHUrl { get; set; }


        public static RenterDTO? From(Domain.Aggregates.Renter.Renter? renter)
        {
            if (renter == null) return null;
            return new RenterDTO
            {
                Name = renter.Name,
                CNPJ = renter.CNPJ,
                Birthdate = renter.Birthdate,
                CNH = renter.CNH?.Number,
                CNHType = renter.CNH?.Type,
                //CNHUrl = renter.CNH?.Url
            };
        }
    }
}
