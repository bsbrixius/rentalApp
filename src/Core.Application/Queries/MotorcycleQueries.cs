using BuildingBlocks.API.Core.Data.Pagination;
using Core.Application.Data.DTOs.Motorcycle;
using Core.Application.Query.Motorcycle;
using Core.Domain.Aggregates.Motorcycle;

namespace Core.Application.Query
{
    public interface IMotorcycleQueries
    {
        Task<PaginatedResult<MotorcycleDTO>> SearchMotorcycleAsync(SearchMotorcycleQuery searchMotorcycleQuery);
    }
    public class MotorcycleQueries : IMotorcycleQueries
    {
        private readonly IMotorcycleQueryRepository _motorcycleQueryRepository;
        public MotorcycleQueries(IMotorcycleQueryRepository motorcycleQueryRepository)
        {
            _motorcycleQueryRepository = motorcycleQueryRepository;
        }

        public async Task<PaginatedResult<MotorcycleDTO>> SearchMotorcycleAsync(SearchMotorcycleQuery queryRequest)
        {
            return await _motorcycleQueryRepository.SearchBy(queryRequest.Plate).PaginateAsync(queryRequest.Page, queryRequest.PageSize, MotorcycleDTO.From);
        }
    }
}
