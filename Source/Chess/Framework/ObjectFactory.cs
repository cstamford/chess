using System;
using System.Diagnostics;

namespace Chess.Framework
{
    /// <summary>
    /// Acts a proxy for a collection of objects to facilitate releasing 
    /// and renewing said objects without the overhead of memory allocations.
    /// </summary>
    /// <typeparam name="T">The type to cache.</typeparam>
    public class ObjectFactory<T> : IObjectFactory<T>
    {
        /// <summary>
        /// Gets the count of objects to be cached.
        /// </summary>
        public int CacheSize { get { return _cacheSize; } }

        /// <summary>
        /// Gets the count of objects currently in use.
        /// </summary>
        public int UsedObjects { get { return _usedObjects; } }

        /// <summary>
        /// Gets the count of objects currently free.
        /// </summary>
        public int FreeObjects { get { return CacheSize - UsedObjects; } }

        /// <summary>
        /// Constructs the factory and populates it with the provided objects.
        /// </summary>
        /// <param name="objects">An array of objects to be owned by this factory.</param>
        public ObjectFactory(T[] objects)
        {
            Debug.Assert(objects != null && objects.Length > 0);

            _cacheSize = objects.Length;
            _cache = new ObjectFactoryCacheEntry[_cacheSize];

            for (int i = 0; i < _cacheSize; ++i)
            {
                ObjectFactoryCacheEntry cacheEntry = new ObjectFactoryCacheEntry(objects[i]);
                ResetEntry(cacheEntry);
                _cache[i] = cacheEntry;
            }
        }

        /// <summary>
        /// Fetches an object from the factory. Each object should be freed using 
        /// <see cref="FreeObject"/> when no longer in use.
        /// </summary>
        /// <returns>The cached object. Returns null if there are no free objects.</returns>
        public T GetObject()
        {
            for (int i = 0; i < _cacheSize; ++i)
            {
                ObjectFactoryCacheEntry cacheEntry = _cache[i];

                // If this element is not already used, mark it as in use and return it.
                if (!cacheEntry.Used)
                {
                    PrepareEntry(cacheEntry);
                    ++_usedObjects;
                    return cacheEntry.CachedObject;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Fetches an object of the provided type from the factory. 
        /// Each object should be freed using <see cref="FreeObject"/> when no longer in use.
        /// </summary>
        /// <typeparam name="ObjT">The type of the requested object.</typeparam>
        /// <returns>
        /// The cached object. Returns null if there are no free objects of the provided type.
        /// </returns>
        public T GetObject<ObjT>() where ObjT : T
        {
            for (int i = 0; i < _cacheSize; ++i)
            {
                ObjectFactoryCacheEntry cacheEntry = _cache[i];

                // If this element is not already used and it matches the requested type, 
                // mark it as in use and return it.
                if (!cacheEntry.Used && cacheEntry.CachedObject.GetType() == typeof(ObjT))
                {
                    PrepareEntry(cacheEntry);
                    ++_usedObjects;
                    return cacheEntry.CachedObject;
                }
            }

            return default(ObjT);
        }

        /// <summary>
        /// Frees the provided object for future use. 
        /// Throws if the object is not part of this factory.
        /// </summary>
        /// <param name="cachedObject">The object to free.</param>
        public void FreeObject(T cachedObject)
        {
            Debug.Assert(cachedObject != null);

            for (int i = 0; i < _cacheSize; ++i)
            {
                ObjectFactoryCacheEntry cacheEntry = _cache[i];

                // Check for reference equality here -- we really only care if the
                // two objects point to the same instance of the object.
                if (ReferenceEquals(cachedObject, cacheEntry.CachedObject))
                {
                    ResetEntry(cacheEntry);
                    --_usedObjects;
                    return;
                }
            }

            throw new Exception("The provided object was not part of the collection.");
        }

        /// <summary>
        /// Represents a cache entry containing one object and a flag indicating whether that 
        /// object is in use or not.
        /// </summary>
        protected class ObjectFactoryCacheEntry
        {
            /// <summary>
            /// Gets the cached object belonging to this cache entry.
            /// </summary>
            public T CachedObject { get { return _cachedObject; } }

            /// <summary>
            /// Gets or sets whether this cache entry is currently in use.
            /// </summary>
            public bool Used
            {
                get { return _used; }
                set { _used = value; }
            }

            /// <summary>
            /// Constructs a cache entry using the provided object and optionally with an 
            /// initial used state.
            /// </summary>
            /// <param name="cachedObject">The object belonging to this cache entry.</param>
            /// <param name="used">The initial used flag state.</param>
            public ObjectFactoryCacheEntry(T cachedObject, bool used = false)
            {
                Debug.Assert(cachedObject != null);
                _cachedObject = cachedObject;
                _used = used;
            }

            /// <summary>
            /// The cached object belonging to this cache entry.
            /// </summary>
            protected T _cachedObject;

            /// <summary>
            /// Whether this cache entry is currently in use.
            /// </summary>
            protected bool _used;
        }

        /// <summary>
        /// Resets a cache entry to its default state. 
        /// Should be reimplemented to reset more complex types to their default state.
        /// If reimplemented, it should never call <see cref="FreeObject"/>.
        /// </summary>
        /// <param name="cacheEntry">The entry to reset.</param>
        protected virtual void ResetEntry(ObjectFactoryCacheEntry cacheEntry)
        {
            Debug.Assert(cacheEntry != null);
            cacheEntry.Used = false;
        }

        /// <summary>
        /// Prepares a cache entry to be released to a caller.
        /// Should be reimplemented to prepare more complex types to be released.
        /// If reimplemented, it should never call <see cref="GetObject"/> or <see cref="GetObject{ObjT}"/>.
        /// </summary>
        /// <param name="cacheEntry">The entry to release.</param>
        protected virtual void PrepareEntry(ObjectFactoryCacheEntry cacheEntry)
        {
            Debug.Assert(cacheEntry != null);
            cacheEntry.Used = true;
        }

        /// <summary>
        /// The count of objects to be cached.
        /// </summary>
        private readonly int _cacheSize;

        /// <summary>
        /// The internal obejct cache array.
        /// </summary>
        private readonly ObjectFactoryCacheEntry[] _cache;

        /// <summary>
        /// The count of objects currently in use.
        /// </summary>
        private int _usedObjects;
    }
}