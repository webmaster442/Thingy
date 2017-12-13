using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IPrograms
    {
        IEnumerable<LauncherProgram> GetPrograms();
        void SaveLauncherProgram(LauncherProgram program);
        void UpdateLauncherProgramByName(string oldname, LauncherProgram newdata);
        void DeleteLauncherProgram(string name);
    }
}
