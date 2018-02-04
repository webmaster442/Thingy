using System.IO;

namespace Thingy.Db
{
    public interface IStoredFiles
    {
        bool Exists(Folders folder, string filename);
        void Delete(Folders folder, string filename);
        Stream OpenRead(Folders folder, string filename);
        Stream OpenWrite(Folders foler, string filename);
    }
}
