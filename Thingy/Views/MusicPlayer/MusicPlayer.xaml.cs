using AppLib.MVVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thingy.ViewModels.MusicPlayer;
using Thingy.Views.Interfaces;

namespace Thingy.Views.MusicPlayer
{
    /// <summary>
    /// Interaction logic for MusicPlayer.xaml
    /// </summary>
    public partial class MusicPlayer : UserControl, IMusicPlayer
    {
        private double? _dragedto;

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

        private void SeekBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ViewModel?.DragStartedCommand.Execute(null);
                var slider = (Slider)sender;
                Point position = e.GetPosition(slider);
                double d = 1.0d / slider.ActualWidth * position.X;
                _dragedto = slider.Maximum * d;
            }
        }

        private void SeekBar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_dragedto != null)
            {
                ViewModel?.DragCompletedCommand.Execute(_dragedto.Value);
                _dragedto = null;
            }
        }

        public void SwithToTab(MusicPlayerTabs tab)
        {
            Dispatcher.Invoke(() =>
            {
                MainTabs.SelectedIndex = (int)tab;
            });
        }
    }
}
