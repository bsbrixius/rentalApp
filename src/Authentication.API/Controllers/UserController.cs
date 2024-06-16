using Authentication.API.Application.Commands.User.PreRegisterUser;
using Authentication.API.Application.Commands.User.RegisterUser;
using Authentication.API.Application.DTO.User;
using Authentication.API.Application.Queries.User;
using Authentication.API.Domain;
using BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserQueries _userQueries;

        public UserController(
            IMediator mediator,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserQueries userQueries
            )
        {
            _mediator = mediator;
            _signInManager = signInManager;
            _userManager = userManager;
            _userQueries = userQueries;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var result = await _userQueries.GetByIdAsync(id);
            if (result == null) return NoContent();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDTO registerUserDTO)
        {
            await _mediator.Send(new RegisterUserCommand
            {
                FirstName = registerUserDTO.FirstName,
                LastName = registerUserDTO.LastName,
                Username = registerUserDTO.Username,
                Email = registerUserDTO.Email,
                Password = registerUserDTO.Password,
                BirthDay = registerUserDTO.BirthDay
            });
            return Ok();
        }

        [HttpPost("pre-register")]
        [Authorize(Roles = nameof(SystemRoles.Admin))]
        public async Task<IActionResult> PreRegisterAsync([FromBody] PreRegisterUserDTO preRegisterUserDTO)
        {
            var result = await _mediator.Send(new PreRegisterUserCommand
            {
                Email = preRegisterUserDTO.Email,
                Role = preRegisterUserDTO.Role
            });
            return CreatedAtRoute(nameof(GetByIdAsync), new { id = result });
        }
    }
}
