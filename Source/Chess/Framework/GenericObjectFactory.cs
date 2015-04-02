using System.Linq;

namespace Chess.Framework
{
    /// <summary>
    /// Extends the <see cref="ObjectFactory"/> class to allow for easy default
    /// construction of the initial cache.
    /// </summary>
    /// <typeparam name="T">The type to cache.</typeparam>
    public class GenericObjectFactory<T> : ObjectFactory<T> where T : new()
    {
        /// <summary>
        /// Constructs the factory and populates it with cacheSize objects.
        /// </summary>
        /// <param name="cacheSize">The count of objects to hold in the cache.</param>
        public GenericObjectFactory(int cacheSize = 128)
            : base(Enumerable.Range(0, cacheSize).Select(obj => new T()).ToArray())
        {
        }
    }
}
