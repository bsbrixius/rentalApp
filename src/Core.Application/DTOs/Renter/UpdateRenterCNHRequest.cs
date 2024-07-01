using Core.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Core.Application.DTOs.Renter
{
    public record UpdateRenterCNHRequest
    {
        public string Number { get; init; }
        public CNHCategoryType CNHType { get; init; }
        public IFormFile CNHFile { get; init; }
    }
}
