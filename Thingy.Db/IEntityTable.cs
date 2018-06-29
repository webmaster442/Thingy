using System.Collections.Generic;

namespace Thingy.Db
{
    public interface IEntityTable<Tkey, TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetByKey(Tkey key);
        void Save(TEntity note);
        void Save(IEnumerable<TEntity> notes);
        void Delete(Tkey key);
        void DeleteAll();
    }
}
