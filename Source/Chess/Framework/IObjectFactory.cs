namespace Chess.Framework
{
    /// <summary>
    /// Interface for implementations of an object factory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectFactory<T>
    {
        /// <summary>
        /// Returns an object from the factory. Each object should be freed using 
        /// <see cref="FreeObject"/> when no longer in use.
        /// </summary>
        /// <returns>The cached object. Returns null if there are no free objects.</returns>
        T GetObject();

        /// <summary>
        /// Fetches an object of the provided type from the factory. 
        /// Each object should be freed using <see cref="FreeObject"/> when no longer in use.
        /// </summary>
        /// <typeparam name="ObjT">The type of the requested object.</typeparam>
        /// <returns>
        /// The cached object. Returns null if there are no free objects of the provided type.
        /// </returns>
        T GetObject<ObjT>() where ObjT : T;

        /// <summary>
        /// Frees the provided object for future use. 
        /// Throws if the object is not part of this factory.
        /// </summary>
        /// <param name="cachedObject">The object to free.</param>
        void FreeObject(T cached);
    }
}
