using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IPrograms: IEntityTable<string, LauncherProgram>
    {
        void UpdateLauncherProgramByName(string oldname, LauncherProgram newdata);
    }
}
