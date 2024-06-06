namespace Authentication.API.Application.Data.Auth
{
    public class LoginRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
