using BuildingBlocks.API.Core.Security;
using BuildingBlocks.Identity.Services;
using BuildingBlocks.Security.Authorization;
using Core.Application.Commands.Rent;
using Core.Application.DTOs.Rent;
using Core.Application.Query.Rent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Areas.Renter.Controller
{
    [Area("Renter")]
    [ApiController]
    [Route("api/v1/[area]/[controller]")]
    [Authorize(Roles = SystemRoles.Renter)]
    public class RentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public RentController(
            IMediator mediator,
            IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }

        [HttpGet("plans")]
        [Authorize(Policy = Policies.Roles.Renter.Read)]
        [ProducesResponseType(typeof(List<RentPlanDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlans()
        {
            return Ok(await _mediator.Send(new GetRentPlansQuery()));
        }

        [HttpGet("information")]
        [Authorize(Policy = Policies.Roles.Renter.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Rent([FromQuery] GetRentInformationRequest getRentInformationRequest)
        {
            return Ok(await _mediator.Send(new GetRentInformationQuery
            {
                StartAt = getRentInformationRequest.StartAt,
                EndAt = getRentInformationRequest.EndAt,
                ExpectedReturnAt = getRentInformationRequest.ExpectedReturnAt,
            }));
        }

        [HttpPost]
        [Authorize(Policy = Policies.Roles.Renter.Write)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Rent([FromBody] RentMotorcycleRequest rentMotorcycleRequest)
        {
            await _mediator.Send(new RentMotorcycleCommand
            {
                UserId = _identityService.GetUserId(),
                //StartAt = rentMotorcycleRequest.StartAt,
                EndAt = rentMotorcycleRequest.EndAt,
                ExpectedReturnAt = rentMotorcycleRequest.ExpectedReturnAt,
            });
            return Ok();
        }

    }
}
