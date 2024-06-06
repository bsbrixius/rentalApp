﻿namespace BuildingBlocks.Security.Settings
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int Expiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

}