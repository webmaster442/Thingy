using AppLib.MVVM;
using System.Windows.Controls;
using Thingy.ViewModels.MusicPlayer;

namespace Thingy.Views.MusicPlayer
{
    /// <summary>
    /// Interaction logic for MusicPlayer.xaml
    /// </summary>
    public partial class MusicPlayer : UserControl, IView<MusicPlayerViewModel>
    {
        public MusicPlayerViewModel ViewModel
        {
            get
            {
                return DataContext as MusicPlayerViewModel;
            }
        }

        public MusicPlayer()
        {
            InitializeComponent();
        }

        private void SeekBar_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            ViewModel?.DragStartedCommand.Execute(null);
        }

        private void SeekBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            ViewModel?.DragCompletedCommand.Execute(SeekBar.Value);
        }
    }
}
