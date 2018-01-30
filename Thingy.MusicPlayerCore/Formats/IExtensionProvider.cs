using System.Collections.Generic;

namespace Thingy.MusicPlayerCore.Formats
{
    public enum FormatKind
    {
        Stream,
        Playlist,
        Unknown
    }

    public interface IExtensionProvider
    {
        string PlalistsFilterString { get; }
        string AllFormatsFilterString { get; }
        string AllFormatsAndPlaylistsFilterString { get; }
        IEnumerable<string> AllSupportedFormats { get; }
        FormatKind GetFormatKind(string file);
        bool IsNetworkStream(string file);
    }
}
