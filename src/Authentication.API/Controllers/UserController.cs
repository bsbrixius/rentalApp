using Authentication.API.Application.Commands.PreRegisterUser;
using Authentication.API.Application.Commands.RegisterUser;
using Authentication.API.Application.DTO.User;
using Authentication.API.Domain;
using Authentication.API.Domain.Utils;
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

        public UserController(
            IMediator mediator,
            SignInManager<User> signInManager,
            UserManager<User> userManager
            )
        {
            _mediator = mediator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var result = await _mediator.Send(new RegisterUserCommand
            {
                FirstName = registerUserDTO.FirstName,
                LastName = registerUserDTO.LastName,
                Username = registerUserDTO.Username,
                Email = registerUserDTO.Email,
                Password = registerUserDTO.Password,
                BirthDay = registerUserDTO.BirthDay
            });
            if (result == null) return NoContent();

            return Ok();
            //var user = await _userManager.FindByIdAsync(id);
            //if (user is null) throw new NotFoundException();

            //var roles = await _userManager.GetRolesAsync(user);

            //return Ok(UserDTO.From(user, roles.First()));
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
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> PreRegisterAsync([FromBody] PreRegisterUserDTO preRegisterUserDTO)
        {
            var result = await _mediator.Send(new PreRegisterUserCommand
            {
                Username = preRegisterUserDTO.Username,
                Role = preRegisterUserDTO.Role
            });
            return CreatedAtRoute(nameof(GetByIdAsync), new { id = result });
        }
    }
}
