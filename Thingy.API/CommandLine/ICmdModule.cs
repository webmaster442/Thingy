using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thingy.API
{
    /// <summary>
    /// Command Module
    /// </summary>
    public interface ICmdModule
    {
        /// <summary>
        /// Help Location
        /// </summary>
        string HelpLocation { get; }
        /// <summary>
        /// Command Module main entry point
        /// </summary>
        ///<param name="arguments">Argument passed to command</param>
        /// <returns>Output strings collection</returns>
        Task<IEnumerable<string>> Run(CmdArguments arguments);
        /// <summary>
        /// Command name
        /// </summary>
        string InvokeName { get; }
    }
}
