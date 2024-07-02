using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.API.Core.Security;
using Core.Application.Commands.Motorcycle;
using Core.Application.DTOs.Motorcycle;
using Core.Application.Query.Motorcycle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("api/v1/[area]/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
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
        public async Task<IActionResult> Get([FromQuery] SearchMotorcycleRequest searchMotorcycleRequest)
        {
            return Ok(await _mediator.Send(new SearchMotorcycleQuery
            {
                Page = searchMotorcycleRequest.Page,
                PageSize = searchMotorcycleRequest.PageSize,
                Plate = searchMotorcycleRequest.Plate
            }));
        }

        [HttpPost]
        [Authorize(Policy = Policies.Roles.Admin.Write)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] RegisterMotorcycleRequest registerMotorcycleDTO)
        {
            await _mediator.Send(new RegisterMotorcycleCommand
            {
                Model = registerMotorcycleDTO.Model,
                Plate = registerMotorcycleDTO.Plate,
                Year = registerMotorcycleDTO.Year
            });
            return Created();
        }

        [HttpPatch("{id}/plate")]
        [Authorize(Policy = Policies.Roles.Admin.Write)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] UpdateMotorcyclePlateRequest updateMotorcyclePlateRequest)
        {
            await _mediator.Send(new UpdateMotorcycleCommand
            {
                Id = id,
                Plate = updateMotorcyclePlateRequest.Plate
            });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Roles.Admin.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new RemoveMotorcycleCommand { Id = id });
            return NoContent();
        }
    }
}
