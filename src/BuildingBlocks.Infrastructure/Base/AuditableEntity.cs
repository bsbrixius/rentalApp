using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.Base
{
    //public interface IAuditableEntity
    //{
    //    public string CreatedBy { get; set; }
    //    public string UpdatedBy { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public DateTime? UpdatedAt { get; set; }
    //}
    public abstract class AuditableEntity : Entity
    {
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
