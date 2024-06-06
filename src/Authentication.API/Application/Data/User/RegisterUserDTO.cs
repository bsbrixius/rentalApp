namespace Authentication.API.Application.DTO.User
{
    public class RegisterUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateOnly? BirthDay { get; set; }
    }
}
