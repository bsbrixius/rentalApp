﻿using BuildingBlocks.Domain;

namespace BuildingBlocks.Domain.Base
{
    public abstract class AuditableEntity : Entity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
