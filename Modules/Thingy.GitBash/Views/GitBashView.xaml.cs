using AppLib.MVVM.MessageHandler;
using System;
using System.Windows.Controls;
using Thingy.API.Messages;

namespace Thingy.GitBash.Views
{
    /// <summary>
    /// Interaction logic for GitBashView.xaml
    /// </summary>
    public partial class GitBashView : UserControl, IGitBashView, IMessageClient<HandleFileMessage>
    {
        public GitBashView()
        {
            InitializeComponent();
        }

        public bool IsAlive
        {
            get { return GitControl.IsAlive; }
        }

        public Guid MessageReciverID
        {
            get { return Guid.Parse(Tag.ToString()); }
        }

        public void Close()
        {
            MainGrid.Children.Remove(GitControl);
            GitControl = null;
        }

        public void HandleMessage(HandleFileMessage message)
        {
            var path = message.Files[0].Replace(@":", "").Replace(@"\", "/");
            SendText($"cd /{path}");
        }

        public void SendText(string text)
        {
            GitControl.SendText(text);
        }
    }
}
