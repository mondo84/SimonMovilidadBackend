namespace Domain.Entities
{
    public abstract class EntityBase
    {
        public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
