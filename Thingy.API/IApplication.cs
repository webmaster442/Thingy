using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        /// Type resolver
        /// </summary>
        ITypeResolver Resolver { get; }
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
    }
 }
