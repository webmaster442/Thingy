using AppLib.Common.Extensions;
using AppLib.MVVM.MessageHandler;
using System;
using System.Windows.Controls;
using Thingy.API.Messages;

namespace Thingy.MediaModules.Views
{
    /// <summary>
    /// Interaction logic for FFMpegGui.xaml
    /// </summary>
    public partial class FFMpegGui : UserControl, IMessageClient<HandleFileMessage>
    {
        public FFMpegGui()
        {
            InitializeComponent();
        }

        public Guid MessageReciverID
        {
            get { return Guid.Parse(Tag.ToString()); }
        }

        public void HandleMessage(HandleFileMessage message)
        {
            (DataContext as ViewModels.FFMpegGuiViewModel)?.Files?.AddRange(message.Files);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTab.SelectedIndex == MainTab.Items.Count - 2)
            {
                var extension = (DataContext as ViewModels.FFMpegGuiViewModel)?.SelectedPreset?.SudgestedExtension;
                if (string.IsNullOrEmpty(extension)) extension = "[E]";
                Renamer.ExtensionPattern = extension;
            }
            if (MainTab.SelectedIndex == MainTab.Items.Count -1)
            {
                (DataContext as ViewModels.FFMpegGuiViewModel)?.GenerateBachCommand.Execute(null);
            }
        }
    }
}
