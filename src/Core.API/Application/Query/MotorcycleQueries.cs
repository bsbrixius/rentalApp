using BuildingBlocks.API.Core.Data.Pagination;
using Core.API.Application.Data.DTOs.Motorcycle;
using Core.API.Application.Query.Motorcycle;
using Core.API.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Core.API.Application.Query
{
    public interface IMotorcycleQueries
    {
        Task<PaginatedResult<MotorcycleDTO>> SearchMotorcycleAsync(SearchMotorcycleQuery searchMotorcycleQuery);
    }
    //TODO: Implement IMotorcycleQueryRepository
    public class MotorcycleQueries : IMotorcycleQueries
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        public MotorcycleQueries(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<PaginatedResult<MotorcycleDTO>> SearchMotorcycleAsync(SearchMotorcycleQuery queryRequest)
        {
            var query = _motorcycleRepository.QueryNoTrack;
            if (string.IsNullOrEmpty(queryRequest.Plate))
            {
                query = query.Where(x => EF.Functions.Like(x.Plate, $"{queryRequest.Plate}"));
            }
            var paginatedUsers = await query.PaginateAsync(queryRequest.Page, queryRequest.PageSize, MotorcycleDTO.From);
            return paginatedUsers;
        }
    }
}
