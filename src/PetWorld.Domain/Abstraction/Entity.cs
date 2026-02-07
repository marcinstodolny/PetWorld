namespace PetWorld.Domain.Abstraction
{
    public abstract class Entity<TId>
    {
        protected Entity() { }
        protected Entity(TId id) => Id = id;


        public TId Id { get; protected set; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }
    }
}