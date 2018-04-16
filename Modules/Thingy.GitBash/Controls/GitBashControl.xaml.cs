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
    public partial class GitBashControl : UserControl, IDisposable
    {
        private GitHost _gitHost;
        private DispatcherTimer _timer;

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

                Application.Current.Shutdown();
            }

            if (_gitHost == null)
            {
                _gitHost = new GitHost(GitLocator.GitPath);
                RootGrid.Children.Clear();
                RootGrid.Children.Add(_gitHost);
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(150);
            _timer.Tick += _timer_Tick;
            _timer.IsEnabled = true;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_gitHost == null || _gitHost.IsAlive == false)
            {
                _timer.Stop();
            }
        }

        public void SendText(string s)
        {
            _gitHost.SendText(s);
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.IsEnabled = false;
            _gitHost.Dispose();
        }
    }
}
