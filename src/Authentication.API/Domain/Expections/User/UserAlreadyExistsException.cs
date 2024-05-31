
namespace Authentication.API.Domain.Expections.User
{
    public class UserAlreadyExistsException : AlreadyExistException
    {
        public UserAlreadyExistsException()
        {
        }

        public UserAlreadyExistsException(string message) : base(message)
        {
        }

        public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
