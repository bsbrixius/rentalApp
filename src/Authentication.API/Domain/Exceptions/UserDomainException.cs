using Authentication.API.Domain.Expections;

namespace Authentication.API.Domain.Exceptions
{
    public class UserDomainException : DomainException
    {
        public UserDomainException()
        {
        }

        public UserDomainException(string message) : base(message)
        {
        }

        public UserDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
