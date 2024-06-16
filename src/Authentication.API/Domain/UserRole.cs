namespace Authentication.API.Domain
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        //public User User { get; set; }
        public Guid RoleId { get; set; }
        //[ForeignKey(nameof(RoleId))]
        //public Role Role { get; set; }
    }
}
