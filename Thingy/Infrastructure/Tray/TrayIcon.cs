using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Thingy.Properties;

namespace Thingy.Infrastructure.Tray
{
    public sealed class TrayIcon: IDisposable
    {
        private NotifyIcon NotifyIcon;
        private ContextMenuStrip TrayMenu;
        private ToolStripItem[] MenuItems;

        public TrayIcon()
        {
            TrayMenu = new ContextMenuStrip();
            CreateAndSetupMenu();
            TrayMenu.Items.AddRange(MenuItems);
            NotifyIcon = new NotifyIcon
            {
                ContextMenuStrip = TrayMenu,
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Info,
                Icon = GetMainIcon()
            };

        }

        private Icon GetMainIcon()
        {
            var current = System.Diagnostics.Process.GetCurrentProcess();
            return Icon.ExtractAssociatedIcon(current.MainModule.FileName);
        }

        private void CreateAndSetupMenu()
        {
            MenuItems = new ToolStripItem[]
            {
                new ToolStripMenuItem("Open Program", Resources.icons8_advertisement_page, OpenProgramHandler),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Exit Program", Resources.icons8_exit_sign, ExitProgramHandler)
            };
            
        }

        private void OpenProgramHandler(object sender, EventArgs e)
        {
            App.Current.MainWindow.Show();
        }

        private void ExitProgramHandler(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        public void Dispose()
        {
            if (MenuItems != null)
            {
                for (int i=0; i<MenuItems.Length; i++)
                {
                    if (MenuItems[i] != null)
                    {
                        MenuItems[i].Dispose();
                        MenuItems[i] = null;
                    }
                }
            }
            if (TrayMenu != null)
            {
                TrayMenu.Dispose();
                TrayMenu = null;
            }
            if (NotifyIcon != null)
            {
                NotifyIcon.Dispose();
                NotifyIcon = null;
            }
        }
    }
}
