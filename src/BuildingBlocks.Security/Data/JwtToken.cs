namespace BuildingBlocks.Security.Data
{
    public class JwtToken
    {
        public class UserData
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; }
        }
        public UserData User { get; set; }
        public string AcessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
