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
    }
}
