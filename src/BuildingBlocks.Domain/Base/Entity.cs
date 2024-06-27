namespace BuildingBlocks.Domain.Base
{
    public interface IEntity
    {
    }

    public abstract class Entity : IEntity
    {
        Guid _Id;
        public virtual Guid Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }

        protected Entity()
        {
            _Id = Guid.NewGuid();
        }
    }
}
