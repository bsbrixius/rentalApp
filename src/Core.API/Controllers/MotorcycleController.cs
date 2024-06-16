using BuildingBlocks.Security.Authorization;
using Core.API.Application.Commands.Motorcycle.RegisterMotorcycle;
using Core.API.Application.Data.DTOs.Motorcycle;
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

        [HttpPost]
        [Authorize(Roles = nameof(SystemRoles.Admin))]
        public async Task<IActionResult> PostAsync([FromBody] RegisterMotorcycleDTO registerMotorcycleDTO)
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
