using LiteDB;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Thingy.Db.Implementation
{
    internal class StoredFiles: IStoredFiles
    {
        private LiteDatabase _db;

        public StoredFiles(LiteDatabase db)
        {
            _db = db;
        }

        private string GetID(Folders folder, string filename)
        {
            string f = "";
            switch (folder)
            {
                case Folders.AlbumCovers:
                    f ="AlbumCovers";
                    break;
                case Folders.AlbumsCache:
                    f = "AlbumCache";
                    break;
            }
            string toCompute = Path.Combine(f, filename);

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(toCompute));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public void Delete(Folders folder, string filename)
        {
            var id = GetID(folder, filename);
             _db.FileStorage.Delete(id);
        }

        public bool Exists(Folders folder, string filename)
        {
            var id = GetID(folder, filename);
            return _db.FileStorage.Exists(id);
        }

        public Stream OpenRead(Folders folder, string filename)
        {
            var id = GetID(folder, filename);
            return _db.FileStorage.OpenRead(id);
        }

        public Stream OpenWrite(Folders folder, string filename)
        {
            var id = GetID(folder, filename);
            return _db.FileStorage.OpenWrite(id, id);
        }
    }
}
