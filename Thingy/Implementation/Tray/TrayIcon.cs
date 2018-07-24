using AppLib.Common;
using System;
using System.Drawing;
using System.Windows.Forms;
using Thingy.API;

namespace Thingy.Implementation.Tray
{
    public sealed class TrayIcon : IDisposable
    {
        private NotifyIcon _NotifyIcon;
        private ContextMenuStrip _TrayMenu;
        private ToolStripItem[] _MenuItems;
        private KeyboardHook _keyboardHook;
        private IApplication _app;

        public TrayIcon(IApplication app)
        {
            _app = app;
            _TrayMenu = new ContextMenuStrip();
            CreateAndSetupMenu();
            _TrayMenu.Items.AddRange(_MenuItems);
            _NotifyIcon = new NotifyIcon
            {
                ContextMenuStrip = _TrayMenu,
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Info,
                Icon = GetMainIcon()
            };
            _NotifyIcon.DoubleClick += OpenProgramHandler;
            _keyboardHook = new KeyboardHook();
            if (ReadActivatorKey(out Keys activator) && ReadActivatorModifiers(out ModifierKeys modifier))
            {
                try
                {
                    _keyboardHook.RegisterHotKey(modifier, activator);
                    _keyboardHook.KeyPressed += _keyboardHook_KeyPressed;
                }
                catch (Exception)
                {
                    MessageBox.Show(Properties.Resources.ActivatorKeyRegisterError,
                                    Properties.Resources.ActivatorKeyResiterErrorTitle, 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.ActivatorKeyInvalidSetting,
                                Properties.Resources.ActivatorKeyResiterErrorTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _keyboardHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            OpenProgramHandler(sender, null);
        }

        private bool ReadActivatorModifiers(out ModifierKeys modifier)
        {
            ModifierKeys @out = ModifierKeys.None;
            if (Enum.TryParse(_app.Settings.Get(SettingsKeys.ActivatorModifierKey1, "Alt"), out @out))
            {
                if (!string.IsNullOrEmpty(_app.Settings.Get(SettingsKeys.ActivatorModifierKey2, "None")))
                {
                    ModifierKeys out2 = ModifierKeys.None;
                    if (Enum.TryParse(_app.Settings.Get(SettingsKeys.ActivatorModifierKey2, "None"), out out2))
                    {
                        modifier = @out | out2;
                        return true;
                    }
                    else
                    {
                        modifier = ModifierKeys.None;
                        return false;
                    }
                }
                else
                {
                    modifier = @out;
                    return true;
                }
            }
            else
            {
                modifier = ModifierKeys.None;
                return false;
            }
        }

        private bool ReadActivatorKey(out Keys activator)
        {
            return Enum.TryParse(_app.Settings.Get(SettingsKeys.ActivatorKey, "F12"), out activator);
        }

        private Icon GetMainIcon()
        {
            var current = System.Diagnostics.Process.GetCurrentProcess();
            return Icon.ExtractAssociatedIcon(current.MainModule.FileName);
        }

        private void CreateAndSetupMenu()
        {
            _MenuItems = new ToolStripItem[]
            {
                new ToolStripMenuItem("Open Program", Properties.Resources.icons8_advertisement_page, OpenProgramHandler),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Exit Program", Properties.Resources.icons8_exit_sign, ExitProgramHandler)
            };

        }

        private void OpenProgramHandler(object sender, EventArgs e)
        {
            App.Current.MainWindow.Visibility = System.Windows.Visibility.Visible;
            App.Current.MainWindow.Show();
            App.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            App.Current.MainWindow.Activate();
        }

        private void ExitProgramHandler(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        public void Dispose()
        {
            if (_MenuItems != null)
            {
                for (int i = 0; i < _MenuItems.Length; i++)
                {
                    if (_MenuItems[i] != null)
                    {
                        _MenuItems[i].Dispose();
                        _MenuItems[i] = null;
                    }
                }
            }
            if (_TrayMenu != null)
            {
                _TrayMenu.Dispose();
                _TrayMenu = null;
            }
            if (_NotifyIcon != null)
            {
                _NotifyIcon.Dispose();
                _NotifyIcon = null;
            }
            if (_keyboardHook != null)
            {
                _keyboardHook.Dispose();
                _keyboardHook = null;
            }
        }
    }
}
