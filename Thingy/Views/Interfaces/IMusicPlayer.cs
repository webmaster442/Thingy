using AppLib.MVVM;
using Thingy.ViewModels.MusicPlayer;

namespace Thingy.Views.Interfaces
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
