using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IVirtualFolders
    {
        IEnumerable<VirtualFolder> GetVirtualFolders();
        void SaveVirtualFolder(VirtualFolder folder);
        void SaveVirtualFolders(IEnumerable<VirtualFolder> folders);
        void DeleteVirtualFolder(string folderName);
        void DeleteAll();
    }
}
