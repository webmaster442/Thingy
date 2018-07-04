using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Thingy.API
{
    /// <summary>
    /// Used when module is opened as dialog
    /// </summary>
    public class OpenParameters
    {
        public DialogButtons DialogButtons { get; set; }
    }

    public interface IModule
    {
        /// <summary>
        /// Application interface, to acces various services
        /// </summary>
        IApplication App { get; set; }
        /// <summary>
        /// Module name
        /// </summary>
        string ModuleName { get; }
        /// <summary>
        /// Module icon
        /// </summary>
        ImageSource Icon { get; }
        /// <summary>
        /// Main Entry point of module
        /// </summary>
        /// <returns></returns>
        UserControl RunModule();
        /// <summary>
        /// Launcher button background
        /// </summary>
        Color TileColor { get; }
        /// <summary>
        /// Launcher button background brush
        /// By default this is used. The abstract plugin implementation
        /// Returns a SolidColorBrush. The brush color is provided by
        /// the TileColor property.
        /// Use this if you want a fancy looking launcher button background
        /// </summary>
        SolidColorBrush ColorBrush { get; }
        /// <summary>
        /// Ladability check
        /// </summary>
        bool CanLoad { get; }
        /// <summary>
        /// Module category
        /// </summary>
        string Category { get; }
        /// <summary>
        /// Dialog open parameters
        /// </summary>
        OpenParameters OpenParameters { get; }
        /// <summary>
        /// Limit app to singe instance
        /// </summary>
        bool IsSingleInstance { get; }
        /// <summary>
        /// Checks if module can handle file or extension provided
        /// </summary>
        /// <param name="pathOrExtension">path to check</param>
        /// <returns></returns>
        bool CanHadleFile(string pathOrExtension);
        /// <summary>
        /// Checks if moduel supports folder as argument
        /// </summary>
        bool SupportsFolderAsArgument { get; }
        /// <summary>
        /// Callback, when App is attached.
        /// Use this instead of ctor logic.
        /// </summary>
        void AppAttached();
    }
}
