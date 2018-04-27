using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for WindowsInternals.xaml
    /// </summary>
    public partial class WindowsInternals : UserControl
    {
        private void Run(string cmd, string arg = null)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = arg;
                p.Start();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock ctrl)
            {
                string command = ctrl.Tag as string;
                if (!string.IsNullOrEmpty(command))
                {
                    if (command.Contains(" "))
                    {
                        string[] pars = command.Split(' ');
                        Run(pars[0], pars[1]);
                    }
                    else
                    {
                        Run(command);
                    }
                }
            }
        }

        public WindowsInternals()
        {
            InitializeComponent();
        }
    }
}
