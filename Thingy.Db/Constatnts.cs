namespace Thingy.Db
{
    internal static class CollectionNames
    {
        public const string Todo = "TodoCollection";
        public const string FavoriteFolders = "FavoriteFoldersCollection";
        public const string VirtualFolders = "VirtualFoldersCollection";
        public const string Programs = "ProgramsCollection";
        public const string Notes = "NotesCollection";
        public const string Alarms = "AlarmsCollection";
        public const string Songs = "SongsCollection";
        public const string Radios = "RadiosCollection";
        public const string Queries = "QueryCollection";
        public const string Podcasts = "PodcastCollection";
    }

    public enum Folders
    {
        AlbumCovers,
        AlbumsCache,
    }
}
