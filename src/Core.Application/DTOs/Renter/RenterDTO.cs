namespace Core.Application.DTOs.Renter
{
    public class RenterDTO
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateOnly Birthdate { get; set; }
        public string CNH { get; set; }//ValueObject CNH
        public string CNHCategory { get; set; }//ValueObject CNH
        public string CNHUrl { get; set; }//ValueObject CNH


        public static RenterDTO? From(Domain.Aggregates.Renter.Renter? renter)
        {
            if (renter == null) return null;
            return new RenterDTO
            {
                Name = renter.Name,
                CNPJ = renter.CNPJ,
                Birthdate = renter.Birthdate,
                CNH = renter.CNH,
                CNHCategory = renter.CNHCategory,
                CNHUrl = renter.CNHUrl
            };
        }
    }
}
