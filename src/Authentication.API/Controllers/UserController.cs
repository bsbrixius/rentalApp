using Authentication.API.Application.Commands.User.PreRegisterUser;
using Authentication.API.Application.Commands.User.RegisterUser;
using Authentication.API.Application.DTO.User;
using Authentication.API.Application.Queries.User;
using BuildingBlocks.API.Core.Data.Pagination;
using BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoLabs.Security.Authorization;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _userQueries;

        public UserController(
            IMediator mediator,
            IUserQueries userQueries
            )
        {
            _mediator = mediator;
            _userQueries = userQueries;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [AccessAuthorize]
        public async Task<IActionResult> Get([FromQuery] PaginatedRequest paginatedRequest)
        {
            var result = await _userQueries.GetPaginated(paginatedRequest);
            if (result == null) return NoContent();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [AccessAuthorize]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
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
        [AccessAuthorize(SystemRoles.Admin)]
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
