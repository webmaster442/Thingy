using AppLib.MVVM.MessageHandler;
using System;
using System.Windows.Controls;
using Thingy.API.Capabilities;
using Thingy.API.Messages;
using Thingy.MediaModules.ViewModels;

namespace Thingy.MediaModules.Views
{
    /// <summary>
    /// Interaction logic for MpvPlayerView.xaml
    /// </summary>
    public partial class MpvPlayerView : UserControl, IHaveCloseTask, IMessageClient<HandleFileMessage>
    {
        public MpvPlayerView()
        {
            InitializeComponent();
        }

        public MpvPlayerViewModel ViewModel
        {
            get { return DataContext as MpvPlayerViewModel; }
        }

        public Action ClosingTask
        {
            get
            {
                if (ViewModel != null)
                {
                    return ViewModel.StartPlayer;
                }
                return null;
            }
        }

        public bool CanExecuteAsync
        {
            get { return false; }
        }

        public Guid MessageReciverID
        {
            get { return Guid.Parse(Tag.ToString()); }
        }

        public void HandleMessage(HandleFileMessage message)
        {
            if (message.Files.Count > 0)
            {
                var file = message.Files[0];
                if (ViewModel != null)
                {
                    ViewModel.File = file;
                }
            }
        }
    }
}
