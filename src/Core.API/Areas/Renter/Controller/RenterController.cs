using BuildingBlocks.Identity.Services;
using BuildingBlocks.Security.Authorization;
using Core.Application.Commands.Renter;
using Core.Application.DTOs.Renter;
using Core.Application.Query.Renter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Areas.Renter.Controller
{
    [Area("Renter")]
    [ApiController]
    [Route("api/v1/[area]/[controller]")]
    [Authorize(Roles = SystemRoles.Renter)]
    public class RenterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public RenterController(
            IMediator mediator,
            IIdentityService identityService
            )
        {
            ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
            ArgumentNullException.ThrowIfNull(identityService, nameof(identityService));
            _mediator = mediator;
            _identityService = identityService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(RenterDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetRenterQuery
            {
                UserId = _identityService.GetUserId()
            });
            if (result == null) return NoContent();
            return Ok(result);
        }

        [HttpGet("cnh")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCNH()
        {
            var result = await _mediator.Send(new GetRenterCNHQuery
            {
                UserId = _identityService.GetUserId()
            });
            if (result == null) return NoContent();
            return Ok(new { cnhUrl = result });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterRenterRequest registerRenterRequest)
        {
            await _mediator.Send(new RegisterRenterCommand
            {
                UserId = _identityService.GetUserId(),
                Name = registerRenterRequest.Name,
                CNPJ = registerRenterRequest.CNPJ,
                BirthDay = registerRenterRequest.BirthDay
            });
            return Created();
        }

        [HttpPatch("cnh")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCNH([FromForm] UpdateRenterCNHRequest registerRenterRequest)
        {
            await _mediator.Send(new UpdateRenterCNHCommand
            {
                UserId = _identityService.GetUserId(),
                CNHFile = registerRenterRequest.CNHFile,
                CNHType = registerRenterRequest.CNHType,
                Number = registerRenterRequest.Number
            });
            return NoContent();
        }
    }
}
