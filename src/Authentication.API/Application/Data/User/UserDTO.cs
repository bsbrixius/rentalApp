namespace Authentication.API.Application.DTO.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? Birthday { get; set; }

        public static UserDTO From(Domain.User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };
        }
    }
}
