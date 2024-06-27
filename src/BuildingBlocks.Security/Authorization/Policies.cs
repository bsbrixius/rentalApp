using BuildingBlocks.Security.Authorization;

namespace BuildingBlocks.API.Core.Security
{
    public static class Policies
    {
        public class Roles
        {
            public class Admin
            {
                private const string Name = $"{nameof(SystemRoles.Admin)}";
                public const string Read = $"{Name}_read";
                public const string Write = $"{Name}_write";
                public const string Delete = $"{Name}_delete";
            }
            public class Renter
            {
                private const string Name = $"{nameof(SystemRoles.Renter)}";
                public const string Read = $"{Name}_read";
                public const string Write = $"{Name}_write";
            }
            public class CustomerService
            {
                private const string Name = $"{nameof(SystemRoles.CustomerService)}";
                public const string Read = $"{Name}_read";
                public const string Write = $"{Name}_write";
                public const string Delete = $"{Name}_delete";
            }
        }
    }
}
