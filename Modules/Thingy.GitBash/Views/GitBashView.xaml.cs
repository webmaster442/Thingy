using System.Windows.Controls;

namespace Thingy.GitBash.Views
{
    /// <summary>
    /// Interaction logic for GitBashView.xaml
    /// </summary>
    public partial class GitBashView : UserControl, IGitBashView
    {
        public GitBashView()
        {
            InitializeComponent();
        }

        public bool IsAlive
        {
            get { return GitControl.IsAlive; }
        }

        public void Close()
        {
            MainGrid.Children.Remove(GitControl);
            GitControl = null;
        }

        public void SendText(string text)
        {
            GitControl.SendText(text);
        }
    }
}
