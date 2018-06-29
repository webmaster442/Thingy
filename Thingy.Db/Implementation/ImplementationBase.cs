using LiteDB;
using System.Collections.Generic;

namespace Thingy.Db.Implementation
{
    internal abstract class ImplementationBase<TEntity>
    {
        protected LiteCollection<TEntity> EntityCollection { get; private set; }

        public ImplementationBase(LiteCollection<TEntity> collection)
        {
            EntityCollection = collection;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return EntityCollection.FindAll();
        }

        public virtual void Save(IEnumerable<TEntity> entities)
        {
            EntityCollection.InsertBulk(entities);
        }

        public virtual void DeleteAll()
        {
            EntityCollection.Delete(item => true);
        }

    }
}
