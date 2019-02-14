namespace Ulanthos.Framework
{
    /// <summary>
    /// Defines an abstract class which 
    /// updates and draws it's self.
    /// </summary>
    public abstract class DrawableUlanComponent : UlanComponent
    {
        /// <summary>
        /// Creates a new DrawableUlanComponent.
        /// </summary>
        /// <param name="name">The name of this DrawableUlanComponent.</param>
        public DrawableUlanComponent(string name) : base(name) { }

        /// <summary>
        /// Used to draw the DrawableUlanComponent.
        /// </summary>
        public virtual void Render() { }

        /// <summary>
        /// Disposes of this DrawableUlanComponent.
        /// </summary>
        public virtual void Dispose() { }
    }
}
