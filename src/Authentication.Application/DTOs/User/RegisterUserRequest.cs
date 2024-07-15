namespace Authentication.Application.DTOs.User
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string Password { get; set; }
        public DateOnly? BirthDay { get; set; }
    }
}
