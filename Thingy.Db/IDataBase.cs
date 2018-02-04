namespace Thingy.Db
{
    public interface IDataBase
    {
        ITodo Todo { get; }
        IFavoriteFolders FavoriteFolders { get; }
        IVirtualFolders VirtualFolders { get; }
        IPrograms Programs { get; }
        INotes Notes { get; }
        IAlarms Alarms { get; }
        IStoredFiles StoredFiles { get; }
    }
}
