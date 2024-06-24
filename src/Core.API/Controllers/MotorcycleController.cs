using BuildingBlocks.Security.Authorization;
using Core.API.Application.Commands.Motorcycle;
using Core.API.Application.Data.DTOs.Motorcycle;
using Core.API.Application.Query.Motorcycle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MotorcycleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = nameof(SystemRoles.Admin))]
        public async Task<IActionResult> Get([FromQuery] SearchMotorcycleRequest searchMotorcycleRequest)
        {
            await _mediator.Send(new SearchMotorcycleQuery
            {
                Page = searchMotorcycleRequest.Page,
                PageSize = searchMotorcycleRequest.PageSize,
                Plate = searchMotorcycleRequest.Plate
            });
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = nameof(SystemRoles.Admin))]
        public async Task<IActionResult> Post([FromBody] RegisterMotorcycleRequest registerMotorcycleDTO)
        {
            await _mediator.Send(new RegisterMotorcycleCommand
            {
                Model = registerMotorcycleDTO.Model,
                Plate = registerMotorcycleDTO.Plate,
                Year = registerMotorcycleDTO.Year
            });
            return Ok();
        }
    }
}
