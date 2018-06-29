using LiteDB;
using System.Linq;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class Programs: ImplementationBase<LauncherProgram>, IPrograms
    {
        public Programs(LiteCollection<LauncherProgram> collection) : base(collection)
        {
        }

        public void Save(LauncherProgram program)
        {
            EntityCollection.Insert(program);
        }

        public void Delete(string name)
        {
            EntityCollection.Delete(p => p.Name == name);
        }

        public void UpdateLauncherProgramByName(string oldname, LauncherProgram newdata)
        {
            var existing = EntityCollection.Find(f => f.Name == oldname).FirstOrDefault();
            if (existing != null)
            {
                EntityCollection.Delete(p => p == existing);
                EntityCollection.Insert(newdata);
            }
        }

        public LauncherProgram GetByKey(string key)
        {
            return EntityCollection.Find(item => item.Name == key).FirstOrDefault();
        }
    }
}
