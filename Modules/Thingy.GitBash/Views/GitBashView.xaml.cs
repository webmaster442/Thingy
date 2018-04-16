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

        public void Dispose()
        {
            GitControl.Dispose();
        }

        public void SendText(string text)
        {
            GitControl.SendText(text);
        }
    }
}
