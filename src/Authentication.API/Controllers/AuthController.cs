using Authentication.API.Domain;
using BuildingBlocks.Identity.Services;
using BuildingBlocks.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService<User> _userService;
        private readonly ILoginService<User> _loginService;
        private readonly JwtBuilder<User> _jwtUtils;
        private readonly JwtValidator _jwtValidator;

        public AuthController(
            IMediator mediator,
            IUserService<User> userService,
            ILoginService<User> loginService,
            JwtBuilder<User> jwtUtils,
            JwtValidator jwtValidator
            )
        {
            _mediator = mediator;
            _userService = userService;
            _loginService = loginService;
            _jwtUtils = jwtUtils;
            _jwtValidator = jwtValidator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> RegisterAsync([FromBody] Application.Data.Auth.LoginRequest loginRequest)
        {
            var result = _loginService.PasswordSignIn(loginRequest.Email, loginRequest.Password);

            if (!result)
            {
                return Unauthorized();
            }

            return Ok(await _jwtUtils.CreateAccessAsync(loginRequest.Email));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAsync([FromBody] string refreshToken)
        {
            var result = await _jwtValidator.ValidateTokenAsync(refreshToken);

            if (!result.IsValid)
            {
                return Unauthorized();
            }

            var email = result.Claims[JwtRegisteredClaimNames.Email]?.ToString();
            if (email == null)
            {
                return Unauthorized();
            }
            var user = await _userService.FindByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }
            var claims = await _userService.GetClaimsAsync(user);

            if (!claims.Any(c => c.Type == CustomClaim.LastRefreshToken && c.Value == result.Claims[JwtRegisteredClaimNames.Jti].ToString()))
            {
                return Unauthorized();
            }

            return Ok(await _jwtUtils.CreateAccessAsync(email));
        }
    }


}
