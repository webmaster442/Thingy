using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IDataBase
    {
        ITodo Todo { get; }
        IFavoriteFolders FavoriteFolders { get; }
        IVirtualFolders VirtualFolders { get; }
        IPrograms Programs { get; }
        IEntityTable<string, Note> Notes { get; }
        IAlarms Alarms { get; }
        IStoredFiles StoredFiles { get; }
        IMediaLibary MediaLibary { get; }
        IPodcasts Podcasts { get; }
    }
}
