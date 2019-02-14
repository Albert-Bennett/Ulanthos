namespace Ulanthos.Math.Bounding
{
    /// <summary>
    /// The different types of intersections that can occur.
    /// </summary>
    public enum ContainableTypes
    {
        None, Partial, Fully
    }

    /// <summary>
    /// An interface to be used with objects 
    /// which can contain other objects.
    /// </summary>
    public interface IContainable
    {
        /// <summary>
        /// Finds out if an IContainable object contains a given position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>The type of intersection that has occured.</returns>
        ContainableTypes Contains(Vector3 position);

        /// <summary>
        /// Finds out if an IContainable object contains a given BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check.</param>
        /// <returns>The type of intersection that has occured.</returns>
        ContainableTypes Contains(BoundingSphere sphere);

        /// <summary>
        /// Finds out if an IContainable object contains a given BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check.</param>
        /// <returns>The type of intersection that has occured.</returns>
        ContainableTypes Contains(BoundingBox box);
    }
}
