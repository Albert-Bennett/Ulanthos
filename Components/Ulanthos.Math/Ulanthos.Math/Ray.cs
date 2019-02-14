using System;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Math.Bounding;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines a Ray.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray : IEquatable<Ray>, IIntersect
    {
        #region Properties

        /// <summary>
        /// The direction of the Ray.
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// The position of the Ray.
        /// </summary>
        public Vector3 Position;

        #endregion
        #region Constructors

        /// <summary>
        /// Creates a new ray.
        /// </summary>
        /// <param name="position">The position of the ray.</param>
        /// <param name="direction">The diretcion of the Ray.</param>
        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// A check to see if two objects are the same.
        /// </summary>
        /// <param name="other">The object to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public override bool Equals(object obj)
        {
            return obj is Ray ? Equals((Ray)obj) : base.Equals(obj);
        }

        /// <summary>
        /// Converts this Ray into a string.
        /// </summary>
        /// <returns>This Ray as a string.</returns>
        public override string ToString()
        {
            return "Position = " + Position.ToString() +
                ", Direction = " + Direction.ToString();
        }

        /// <summary>
        /// Gets the hash code for this Ray.
        /// </summary>
        /// <returns>The Ray's hash code.</returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Direction.GetHashCode();
        }

        /// <summary>
        /// A check to see if two Rays are the same.
        /// </summary>
        /// <param name="other">The Ray to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public bool Equals(Ray other)
        {
            return this == other;
        }

        /// <summary>
        /// A check to see if two objects have intersected. 
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(BoundingSphere sphere)
        {
            Vector3 dist = Vector3.Zero;
            Vector3.Subtract(ref Position, ref sphere.Center, out dist);

            float dot1 = Vector3.Dot(dist, Direction);
            float dot2 = dot1 - (sphere.Radius * sphere.Radius);

            if (dot2 > 0 && dot1 > 0)
                return false;

            float discrim = dot1 * dot1 - dot2;

            if (discrim < 0)
                return false;

            float pointOfContact = -dot1 - MathHelper.Sqrt(discrim);

            if (pointOfContact < 0)
                return true;

            return false;
        }

        /// <summary>
        /// A check to see if two objects have intersected. 
        /// </summary>
        /// <param name="box">The BoundingBox to check.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(BoundingBox box)
        {
            float intersectionPoint = 0f;
            float tmax = float.MaxValue;

            if (System.Math.Abs(Direction.X) < MathHelper.Epsilon)
            {
                if (Position.X < box.Min.X || Position.X > box.Max.X)
                {
                    return false;
                }
            }
            else
            {
                float inverse = 1.0f / Direction.X;
                float t1 = (box.Min.X - Position.X) * inverse;
                float t2 = (box.Max.X - Position.X) * inverse;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }
                intersectionPoint = MathHelper.Max(t1, intersectionPoint);
                tmax = MathHelper.Min(t2, tmax);
                if (intersectionPoint > tmax)
                {
                    return false;
                }
            }
            if (System.Math.Abs(Direction.Y) < MathHelper.Epsilon)
            {
                if (Position.Y < box.Min.Y || Position.Y > box.Max.Y)
                {
                    return false;
                }
            }
            else
            {
                float inverse = 1.0f / Direction.Y;
                float t1 = (box.Min.Y - Position.Y) * inverse;
                float t2 = (box.Max.Y - Position.Y) * inverse;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }
                intersectionPoint = MathHelper.Max(t1, intersectionPoint);
                tmax = MathHelper.Min(t2, tmax);
                if (intersectionPoint > tmax)
                {
                    return false;
                }
            }
            if (System.Math.Abs(Direction.Z) < MathHelper.Epsilon)
            {
                if (Position.Z < box.Min.Z || Position.Z > box.Max.Z)
                {
                    return false;
                }
            }
            else
            {
                float inverse = 1.0f / Direction.Z;
                float t1 = (box.Min.Z - Position.Z) * inverse;
                float t2 = (box.Max.Z - Position.Z) * inverse;
                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }
                intersectionPoint = MathHelper.Max(t1, intersectionPoint);
                tmax = MathHelper.Min(t2, tmax);

                if (intersectionPoint > tmax)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// A check to see if two objects have intersected.
        /// </summary>
        /// <param name="ray">The Ray to check.</param>
        /// <param name="pointOfContact">The point of contact between the two objects.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(Ray ray, out Vector3 pointOfContact)
        {
            Vector3 cross = Vector3.Cross(Direction, ray.Direction);
            pointOfContact = Vector3.Zero;
            float denom = cross.Lenght;

            if (MathHelper.Abs(denom) < MathHelper.Epsilon)
            {
                if (MathHelper.Abs(ray.Position.X - Position.X) < MathHelper.Epsilon &&
                    MathHelper.Abs(ray.Position.Y - Position.Y) < MathHelper.Epsilon &&
                    MathHelper.Abs(ray.Position.Z - Position.Z) < MathHelper.Epsilon)
                    return true;
            }
            else
            {
                denom = denom * denom;

                float m11 = ray.Position.X - Position.X;
                float m12 = ray.Position.Y - Position.Y;
                float m13 = ray.Position.Z - Position.Z;
                float m21 = ray.Direction.X;
                float m22 = ray.Direction.Y;
                float m23 = ray.Direction.Z;
                float m31 = cross.X;
                float m32 = cross.Y;
                float m33 = cross.Z;

                float dets =
                    m11 * m22 * m33 +
                    m12 * m23 * m31 +
                    m13 * m21 * m32 -
                    m11 * m23 * m32 -
                    m12 * m21 * m33 -
                    m13 * m22 * m31;

                m21 = Direction.X;
                m22 = Direction.Y;
                m23 = Direction.Z;

                float dett =
                    m11 * m22 * m33 +
                    m12 * m23 * m31 +
                    m13 * m21 * m32 -
                    m11 * m23 * m32 -
                    m12 * m21 * m33 -
                    m13 * m22 * m31;

                float s = dets / denom;
                float t = dett / denom;

                Vector3 point1 = Position + (s * Direction);
                Vector3 point2 = ray.Position + (t * ray.Direction);

                if (MathHelper.Abs(point2.X - point1.X) > MathHelper.Epsilon ||
                    MathHelper.Abs(point2.Y - point1.Y) > MathHelper.Epsilon ||
                    MathHelper.Abs(point2.Z - point1.Z) > MathHelper.Epsilon)
                    return false;
                else
                    pointOfContact = point1;
            }

            return true;
        }

        /// <summary>
        /// A check to see if two objects have intersected.
        /// </summary>
        /// <param name="plane">The Plane to check.</param>
        /// <param name="pointOfContact">The point of contact between the two objects.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(Plane plane, out Vector3 pointOfContact)
        {
            pointOfContact = Vector3.Zero;
            float direct = Vector3.Dot(plane.Normal, Direction);

            if (MathHelper.Abs(direct) < MathHelper.Epsilon)
                return false;
            else
            {
                float pos = Vector3.Dot(plane.Normal, Position);
                float dist = (plane.Distance - pos) / direct;

                if (dist < 0)
                    if (dist < -MathHelper.Epsilon)
                        return false;
                    else
                        return true;
                else
                {
                    pointOfContact = Position + (Direction * dist);
                    return true;
                }
            }
        }

        /// <summary>
        /// Saves this Ray.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        public void Save(BinaryWriter writer)
        {
            Position.Save(writer);
            Direction.Save(writer);
        }

        /// <summary>
        /// Loads in a Ray.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            Position.Load(reader);
            Direction.Load(reader);
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two Ray's are equal.
        /// </summary>
        /// <param name="ray1">Ray 1.</param>
        /// <param name="ray2">Ray 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Ray ray1, Ray ray2)
        {
            return ray1.Direction == ray2.Direction &&
                ray1.Position == ray2.Position;
        }

        /// <summary>
        /// A check to see if two Ray's are not equal.
        /// </summary>
        /// <param name="ray1">Ray 1.</param>
        /// <param name="ray2">Ray 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Ray ray1, Ray ray2)
        {
            return ray1.Direction != ray2.Direction ||
                ray1.Position != ray2.Position;
        }

        #endregion
    }
}
