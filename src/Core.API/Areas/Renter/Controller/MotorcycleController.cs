using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.API.Core.Security;
using Core.Application.DTOs.Motorcycle;
using Core.Application.Query.Motorcycle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Areas.Renter.Controller
{
    [Area("Renter")]
    [ApiController]
    [Route("api/v1/[area]/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MotorcycleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<MotorcycleDTO>), StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.Roles.Admin.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ListMotorcycleRequest listMotorcycleRequest)
        {
            return Ok(await _mediator.Send(new ListMotorcycleQuery
            {
                Page = listMotorcycleRequest.Page,
                PageSize = listMotorcycleRequest.PageSize,
            }));
        }
    }
}
