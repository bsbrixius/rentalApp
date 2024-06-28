namespace BuildingBlocks.Security.Authorization
{
    public static class Claims
    {
        public class Admin
        {
            private const string Name = $"{SystemRoles.Admin}";
            public const string ReadAccess = $"{Name}_read_access";
            public const string WriteAccess = $"{Name}_write_access";
            public const string DeleteAccess = $"{Name}_delete_access";
        }
        public class Renter
        {
            private const string Name = $"{SystemRoles.Renter}";
            public const string ReadAccess = $"{Name}_read_access";
            public const string WriteAccess = $"{Name}_write_access";
            //public const string DeleteAccess = $"{Name}_delete_access";
        }
        public class CustomerService
        {
            private const string Name = $"{SystemRoles.CustomerService}";
            public const string ReadAccess = $"{Name}_read_access";
            public const string WriteAccess = $"{Name}_write_access";
            public const string DeleteAccess = $"{Name}_delete_access";
        }
    }
}
