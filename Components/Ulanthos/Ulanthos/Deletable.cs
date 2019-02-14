using System;

namespace Ulanthos
{
    /// <summary>
    /// Used as a base class for all deletable item
    /// to maximize the garbage collector.
    /// </summary>
    public abstract class Deletable : IDisposable
    {
        bool disposed;

        /// <summary>
        /// Whether or not this has been disposed.
        /// </summary>
        public bool Disposed { get { return disposed; } }

        ~Deletable()
        {
            GetRidOf(false);
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public virtual void Dispose()
        {
            GetRidOf(true);

            GC.SuppressFinalize(this);
        }

        void GetRidOf(bool dispose)
        {
            if (!dispose)
            {
                if (dispose)
                    PreDispose();

                PostDispose();

                disposed = true;
            }
        }

        protected virtual void PreDispose() { }
        protected virtual void PostDispose() { }
    }
}
