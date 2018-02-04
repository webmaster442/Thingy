using LiteDB;

namespace Thingy.Db.Implementation
{
    internal abstract class ImplementationBase<TEntity>
    {
        protected LiteCollection<TEntity> EntityCollection { get; private set; }

        public ImplementationBase(LiteCollection<TEntity> collection)
        {
            EntityCollection = collection;
        }
    }
}
