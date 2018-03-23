namespace Thingy.API
{
    /// <summary>
    /// Type resolver
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Resolve a type
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Resolved type</returns>
        T Resove<T>();
    }
}
