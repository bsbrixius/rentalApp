using Authentication.API.Domain;
using BuildingBlocks.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Authentication.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtBuilder<User> _jwtUtils;
        private readonly JwtValidator _jwtValidator;

        public AuthController(
            IMediator mediator,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            JwtBuilder<User> jwtUtils,
            JwtValidator jwtValidator
            )
        {
            _mediator = mediator;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtUtils = jwtUtils;
            _jwtValidator = jwtValidator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> RegisterAsync([FromBody] Application.Data.Auth.LoginRequest loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, true);

            if (result.IsLockedOut)
            {
                return Forbid();
            }

            if (!result.Succeeded)
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
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }
            var claims = await _userManager.GetClaimsAsync(user);

            if (!claims.Any(c => c.Type == "LastRefreshToken" && c.Value == result.Claims[JwtRegisteredClaimNames.Jti].ToString()))
            {
                return Unauthorized();
            }

            //if (user.LockoutEnabled)
            //    if (user.LockoutEnd < DateTime.Now)
            //        return Forbid();

            return Ok(await _jwtUtils.CreateAccessAsync(email));
        }
    }


}
