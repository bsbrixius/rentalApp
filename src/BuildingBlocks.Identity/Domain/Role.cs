﻿using BuildingBlocks.Domain;
using BuildingBlocks.Security.Domain;

namespace Authentication.API.Domain
{
    public class Role : Entity
    {
        protected Role() { }
        public Role(string name)
        {
            Name = name;
            NormalizedName = name.ToLower();
        }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public virtual List<UserBase> Users { get; set; }
        public virtual List<RoleClaim> RoleClaims { get; set; }
    }
}