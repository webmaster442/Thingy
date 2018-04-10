using AppLib.MVVM;
using Thingy.MusicPlayer.ViewModels;

namespace Thingy.MusicPlayer
{
    public enum MusicPlayerTabs
    {
        Player = 0,
        Playlist
    }

    public interface IMusicPlayer : IView<MusicPlayerViewModel>
    {
        void SwithToTab(MusicPlayerTabs tab);
    }
}
