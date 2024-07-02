namespace Core.Application.DTOs.Renter
{
    public record RegisterRenterRequest
    {
        public string Name { get; init; }
        public string CNPJ { get; init; }
        public DateOnly BirthDay { get; init; }
    }
}
