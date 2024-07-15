namespace Authentication.Application.DTOs.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public DateOnly? Birthday { get; set; }

        public static UserDTO? From(Domain.Aggregates.User? user)
        {
            if (user == null) return null;
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Birthday = user.Birthday
            };
        }
    }
}
