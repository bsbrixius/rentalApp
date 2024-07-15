using Authentication.Application.Commands.User.PreRegisterUser;
using Authentication.Application.Commands.User.RegisterUser;
using Authentication.Application.DTOs.User;
using Authentication.Application.Query.User;
using BuildingBlocks.Identity.Services;
using BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Areas.Renter
{
    [Area("Renter")]
    [ApiController]
    [Route("api/v1/[area]/[controller]")]
    [Authorize(Roles = SystemRoles.Renter)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _userQueries;
        private readonly IIdentityService _identityService;

        public UserController(
            IMediator mediator,
            IUserQueries userQueries,
            IIdentityService identityService
            )
        {
            ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));
            ArgumentNullException.ThrowIfNull(userQueries, nameof(userQueries));
            ArgumentNullException.ThrowIfNull(identityService, nameof(identityService));
            _mediator = mediator;
            _userQueries = userQueries;
            _identityService = identityService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _userQueries.GetByUserIdAsync(_identityService.GetUserId());
            if (result == null) return NoContent();
            return Ok(result);
        }

        [HttpPost("pre-register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<IActionResult> PreRegisterAsync([FromBody] PreRegisterUserRenterRequest preRegisterUserDTO)
        {
            await _mediator.Send(new PreRegisterUserCommand
            {
                Email = preRegisterUserDTO.Email,
                Role = SystemRoles.Renter
            });
            return Created();
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest registerUserDTO)
        {
            await _mediator.Send(new RegisterUserCommand
            {
                FullName = registerUserDTO.FullName,
                Email = registerUserDTO.Email,
                Password = registerUserDTO.Password,
                Birthday = registerUserDTO.BirthDay
            });
            return Ok();
        }
    }
}
