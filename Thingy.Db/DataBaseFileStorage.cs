using LiteDB;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Thingy.Db
{
    public class DataBaseFileStorage: IDataBaseFileStorage
    {
        private LiteDatabase _dbreference;
        private const string FolderIcons = "Icons";

        public DataBaseFileStorage(LiteDatabase dbref)
        {
            _dbreference = dbref;
        }

        private string FilenameToHashId(string folder, string filename)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(filename));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return Path.Combine(folder, sb.ToString());
            }
        }

        public void DeleteIcon(string key)
        {
            var id = FilenameToHashId(FolderIcons, key);
            if (_dbreference.FileStorage.Exists(id))
            {
                _dbreference.FileStorage.Delete(id);
            }
        }

        public Stream GetIcon(string key)
        {
            var id = FilenameToHashId(FolderIcons, key);
            if (_dbreference.FileStorage.Exists(id))
            {
                return _dbreference.FileStorage.OpenRead(id);
            }
            else
            {
                return null;
            }
        }

        public void SaveIcon(string key, Stream data)
        {
            var id = FilenameToHashId(FolderIcons, key);
            if (_dbreference.FileStorage.Exists(id))
            {
                _dbreference.FileStorage.Delete(id);
            }
            _dbreference.FileStorage.Upload(id, id, data);
        }
    }
}
