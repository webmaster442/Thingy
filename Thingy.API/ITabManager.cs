using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Thingy.API
{
    public interface ITabManager
    {
        /// <summary>
        /// Set the current tab content
        /// </summary>
        /// <param name="Title">Tab title</param>
        /// <param name="control">Tab content</param>
        void SetCurrentTabContent(string Title, UserControl control);
        /// <summary>
        /// Create a new tab
        /// </summary>
        /// <param name="Title">Tab title</param>
        /// <param name="control">Tab content</param>
        void CreateNewTabContent(string Title, UserControl control);
        /// <summary>
        /// Gets the tab index based on title
        /// </summary>
        /// <param name="Title">Tab title</param>
        /// <returns>Tab index, -1 if not found</returns>
        int GetTabIndexByTitle(string Title);
        /// <summary>
        /// Focus Tab by index
        /// </summary>
        /// <param name="index">Tab index to focus to</param>
        void FocusTabByIndex(int index);
        /// <summary>
        /// Start a module
        /// </summary>
        /// <param name="module">Module to start</param>
        /// <returns>Awaitable module GUID</returns>
        Task<Guid> StartModule(IModule module);
        void ModuleClosed(Guid ModuleId);
    }
}
