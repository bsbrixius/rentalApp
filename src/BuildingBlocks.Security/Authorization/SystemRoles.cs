namespace BuildingBlocks.Security.Authorization
{
    public class SystemRoles
    {
        public static SystemRoles Admin => new SystemRoles(1, nameof(Admin));
        public static SystemRoles CustomerService => new SystemRoles(2, nameof(CustomerService));
        public static SystemRoles Driver => new SystemRoles(3, nameof(Driver));

        public int Id { get; private set; }
        public string Name { get; private set; }

        private SystemRoles(int id, string name)
        {
            Name = name;
            Id = id;
        }
    }
}
