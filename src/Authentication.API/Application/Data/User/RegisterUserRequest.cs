namespace Authentication.API.Application.Data.User
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string Password { get; set; }
        public DateOnly? BirthDay { get; set; }
    }
}
