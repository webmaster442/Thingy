using AppLib.MVVM.MessageHandler;
using System;
using System.Linq;
using System.Windows.Controls;
using Thingy.API.Messages;
using Thingy.CoreModules.ViewModels;

namespace Thingy.CoreModules.Views
{
    /// <summary>
    /// Interaction logic for CommandLine.xaml
    /// </summary>
    public partial class CommandLine : UserControl, ICommandLineView, IMessageClient<HandleFileMessage>
    {
        public CommandLine()
        {
            InitializeComponent();
            Messager.Instance.SubScribe(this);
        }

        public CommandLineViewModel ViewModel
        {
            get { return DataContext as CommandLineViewModel; }
        }

        public Guid MessageReciverID
        {
            get { return Guid.Parse(Tag.ToString()); }
        }

        public void Close()
        {
            ViewModel?.ClosingCommand?.Execute(null);
        }

        public TextBox GetTextBox()
        {
            return Console;
        }

        public void HandleMessage(HandleFileMessage message)
        {
            ViewModel?.SetFolderCommand?.Execute(message.Files.FirstOrDefault());
        }
    }
}
