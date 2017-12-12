using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IPrograms
    {
        IEnumerable<LauncherProgram> GetPrograms();
        void SaveLauncherProgram(LauncherProgram program);
        void DeleteLauncherProgram(string name);
    }
}
