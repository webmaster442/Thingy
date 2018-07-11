using AppLib.MVVM.MessageHandler;
using System;
using System.Threading.Tasks;
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
            Messager.Instance.SubScribe(this);
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

        public async void HandleMessage(HandleFileMessage message)
        {
            if (message?.Files != null)
            {
                await Task.Delay(100); //wait for process to start
                var path = message.Files[0].Replace(@":", "").Replace(@"\", "/");
                SendText($"cd /{path}");
                SendText($"clear");
            }
        }

        public void SendText(string text)
        {
            GitControl.SendText(text);
        }
    }
}
