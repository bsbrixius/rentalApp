namespace BuildingBlocks.Security.Authorization
{
    public class UserRole
    {
        public static UserRole Admin => new UserRole(1, nameof(Admin));
        public static UserRole CustomerService => new UserRole(2, nameof(CustomerService));
        public static UserRole Driver => new UserRole(3, nameof(Driver));

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string NormalizedName => Name.ToLower();

        private UserRole(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return NormalizedName;
        }
    }
}
