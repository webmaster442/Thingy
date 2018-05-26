using System.Windows.Threading;

namespace Thingy.API
{
    /// <summary>
    /// Command Host Interface
    /// </summary>
    public interface ICmdHost
    {
        /// <summary>
        /// Current working directory
        /// </summary>
        string CurrentDirectory { get; set; }

        /// <summary>
        /// Clear terminal screen
        /// </summary>
        void Clear();

        /// <summary>
        /// Host's dispatcher Needed to acces host API
        /// </summary>
        Dispatcher HostDispatcher { get; }
    }
}
