using System.IO;

namespace Thingy.Db
{
    public interface IDataBaseFileStorage
    {
        void SaveIcon(string key, Stream data);
        Stream GetIcon(string key);
        void DeleteIcon(string key);
    }
}
