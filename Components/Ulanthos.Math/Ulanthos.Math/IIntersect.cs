using Ulanthos.Math.Bounding;

namespace Ulanthos.Math
{
    /// <summary>
    /// The different types of intersection's that can occur.
    /// </summary>
    public enum IntersectionType
    {
        None,
        Intersect,
        Front,
        Back
    }

    /// <summary>
    /// An interface used to manage intersecting objects.
    /// </summary>
    public interface IIntersect
    {
        /// <summary>
        /// A check to see if two objects have intersected.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check.</param>
        /// <returns>The result of the check.</returns>
        bool Intersects(BoundingSphere sphere);

        /// <summary>
        /// A check to see if two objects have intersected.
        /// </summary>
        /// <param name="box">The BoundingBox to check.</param>
        /// <returns>The result of the check.</returns>
        bool Intersects(BoundingBox box);

        /// <summary>
        /// A check to see if two objects have intersected.
        /// </summary>
        /// <param name="ray">The Ray to check.</param>
        /// <param name="pointOfContact">The point at which the two object have collided at.</param>
        /// <returns>The result of the check.</returns>
        bool Intersects(Ray ray, out Vector3 pointOfContact);

        /// <summary>
        /// A check to see if two objects have intersected.
        /// </summary>
        /// <param name="plane">The Plane to check.</param>
        /// <param name="pointOfContact">The point at which the two object have collided at.</param>
        /// <returns>The result of the check.</returns>
        bool Intersects(Plane plane, out Vector3 pointOfContact);
    }
}
