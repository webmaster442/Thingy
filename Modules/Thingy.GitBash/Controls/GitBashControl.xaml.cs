using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Thingy.GitBash.Native;

namespace Thingy.GitBash.Controls
{
    /// <summary>
    /// Interaction logic for GitBashControl.xaml
    /// </summary>
    public partial class GitBashControl : UserControl
    {
        private GitHost _gitHost;

        public GitBashControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            if (!GitLocator.IsGitInstalled)
            {
                MessageBox.Show($"Git is not installed or not found at the default install path.\nSearch Path was: {GitLocator.GitPath}",
                                 "Git not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (_gitHost == null)
            {
                _gitHost = new GitHost(GitLocator.GitPath);
                RootGrid.Children.Clear();
                RootGrid.Children.Add(_gitHost);
            }
        }

        public bool IsAlive
        {
            get { return _gitHost != null && _gitHost.IsAlive; }
        }

        public void SendText(string s)
        {
            _gitHost.SendText(s);
        }
    }
}
