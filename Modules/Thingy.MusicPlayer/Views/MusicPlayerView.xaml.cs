using AppLib.MVVM.MessageHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thingy.API.Messages;
using Thingy.MusicPlayer.ViewModels;

namespace Thingy.MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for MusicPlayer.xaml
    /// </summary>
    public partial class MusicPlayerView : UserControl, IMusicPlayer, IMessageClient<HandleFileMessage>
    {
        private double? _dragedto;

        public MusicPlayerViewModel ViewModel
        {
            get { return DataContext as MusicPlayerViewModel; }
        }

        public Guid MessageReciverID
        {
            get { return Guid.Parse(Tag.ToString()); }
        }

        public MusicPlayerView()
        {
            InitializeComponent();
            Messager.Instance.SubScribe(this);
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

        private void DeviceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DeviceSelector.SelectedIndex;
            ViewModel?.SelectedDeviceChangedCommand.Execute(index);
        }

        public void HandleMessage(HandleFileMessage message)
        {
            if (message == null) return;
            ViewModel?.HandleFiles(message.Files.ToArray());
        }

        private void ITunesMenu_FilesProvidedEvent(object sender, IEnumerable<string> e)
        {
            ViewModel?.HandleFiles(e.ToArray());
        }
    }
}
