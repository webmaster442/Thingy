using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Thingy.API.Messages;

namespace Thingy.API
{
    /// <summary>
    /// Application Interface
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Settings
        /// </summary>
        ISettings Settings { get; }
        /// <summary>
        /// Loging
        /// </summary>
        ILog Log { get; }
        /// <summary>
        /// Message handler
        /// </summary>
        IMessager Messager { get; }
        /// <summary>
        /// Show a Dialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="content">Dialog content</param>
        /// <param name="buttons">Dialog buttons</param>
        /// <param name="hasShadow">Shadow state, optional</param>
        /// <param name="modell">ViewModel, optional</param>
        /// <returns>Dialog button result</returns>
        Task<bool> ShowDialog(string title, UserControl content, DialogButtons buttons, bool hasShadow = true, INotifyPropertyChanged modell = null);
        /// <summary>
        /// Show a real WPF Dialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="content">Dialog content</param>
        /// <param name="buttons">Dialog buttons</param>
        /// <param name="modell">ViewModel, optional</param>
        /// <returns>Dialog button result</returns>
        bool ShowRealDialog(string title, UserControl content, DialogButtons buttons, INotifyPropertyChanged modell = null);
        /// <summary>
        /// Show a message box
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="content">Dialog content</param>
        /// <param name="buttons">Dialog buttons</param>
        /// <returns>Dialog button result</returns>
        Task<bool> ShowMessageBox(string title, string content, DialogButtons buttons);
        /// <summary>
        /// Show a custom message box
        /// </summary>
        /// <param name="messageBoxContent">Message box content</param>
        /// <returns>Awaitable task</returns>
        Task ShowMessageBox(CustomDialog messageBoxContent);
        /// <summary>
        /// Close a custom message box
        /// </summary>
        /// <param name="messageBoxContent">Message box content</param>
        /// <returns>Awaitable task</returns>
        Task CloseMessageBox(CustomDialog messageBoxContent);
        /// <summary>
        /// Hide a message box
        /// </summary>
        /// <param name="messageBoxContent">Message box content</param>
        /// <returns>Awaitable task</returns>
        Task HideMessageBox(CustomDialog messageBoxContent);
        /// <summary>
        /// Tab manager
        /// </summary>
        ITabManager TabManager { get; }
        /// <summary>
        /// Close Application
        /// </summary>
        void Close();
        /// <summary>
        /// Restart Application
        /// </summary>
        void Restart();
        /// <summary>
        /// Handle files
        /// </summary>
        /// <param name="files">files</param>
        void HandleFiles(IList<string> files);
        /// <summary>
        /// Resolve a type from the IoC container
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Resolved type or exception</returns>
        T Resolve<T>();
        /// <summary>
        /// Returns true, if type can be resolved
        /// </summary>
        /// <typeparam name="T">Type to resovle</typeparam>
        /// <returns>true, if type can be resolved</returns>
        bool CanResolve<T>();
        /// <summary>
        /// Register a type to the IOC container
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="getter">Type creator delegate</param>
        void Register<T>(Func<T> getter);
        /// <summary>
        /// Show a status bar menu
        /// </summary>
        /// <param name="control">Statusbar menu content</param>
        /// <param name="title">Title</param>
        /// <param name="AutoClose">Automaticaly close or not. Default = true</param>
        /// <param name="AutoCloseTimeMs">Auto close interval. Default = 5000</param>
        void ShowStatusBarMenu(UserControl control, string title, bool AutoClose = true, int AutoCloseTimeMs = 5000);
        /// <summary>
        /// Show a user control as a flyout on the left
        /// </summary>
        /// <param name="control">Content</param>
        /// <param name="title">Title</param>
        /// <param name="AutoClose">Automaticaly close or not. Default = true</param>
        /// <param name="AutoCloseTimeMs">Auto close interval. Default = 5000</param>
        void ShowFlyoutLeft(UserControl control, string title, bool AutoClose = true, int AutoCloseTimeMs = 5000);
        /// <summary>
        /// Get the Associated Application Dispatcher
        /// </summary>
        Dispatcher CurrentDispatcher { get; }
    }
 }
