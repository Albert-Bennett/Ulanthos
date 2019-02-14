using System;
using System.Runtime.InteropServices;
using System.IO;
using Ulanthos.Math.Bounding;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines a Plane.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Plane : IEquatable<Plane>, IIntersect
    {
        #region Properties

        /// <summary>
        /// The Plane's normal.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// The distance from the origin.
        /// </summary>
        public float Distance;

        #endregion
        #region Constructors

        /// <summary>
        /// Creates a new Plane.
        /// </summary>
        /// <param name="normal">The Plane's normal.</param>
        /// <param name="distance">The distance from the origin that this Plane is at.</param>
        public Plane(Vector3 normal, float distance)
        {
            if (!normal.IsNormalized)
                normal.Normalize();

            Normal = normal;
            Distance = distance;
        }

        /// <summary>
        /// Creates a new Plane.
        /// </summary>
        /// <param name="normalX">The Plane's normal X co-ordinate.</param>
        /// <param name="normalY">The Plane's normal Y co-ordinate.</param>
        /// <param name="normalZ">The Plane's normal Z co-ordinate.</param>
        /// <param name="distance">The distance from the origin that this Plane is at.</param>
        public Plane(float normalX, float normalY,
            float normalZ, float distance) :
            this(new Vector3(normalX, normalY, normalZ), distance) { }

        /// <summary>
        /// Creates a new Plane.
        /// </summary>
        /// <param name="position1">Corner position 1.</param>
        /// <param name="position2">Corner position 2.</param>
        /// <param name="position3">Corner position 3.</param>
        public Plane(Vector3 position1, Vector3 position2, Vector3 position3)
        {
            Vector3 dif1 = position2 - position1;
            Vector3 dif2 = position3 - position1;

            Normal = new Vector3(
                (dif1.Y * dif2.Z) - (dif1.Z * dif2.Y),
                (dif1.Z * dif2.X) - (dif1.X * dif2.Z),
                (dif1.X * dif2.Y) - (dif1.Y * dif2.X));

            if (!Normal.IsNormalized)
                Normal.Normalize();

            Distance = -Vector3.Dot(Normal, position1);
        }

        #endregion
        #region Calculations

        /// <summary>
        /// Finds out the shortest distance between this Plane and the given Vector3.
        /// </summary>
        /// <param name="position">The Vector3 to check.</param>
        /// <returns>The distance between the objects.</returns>
        public float ShortestDistance(Vector3 position)
        {
            return Vector3.Dot(Normal, position) + Distance;
        }

        /// <summary>
        /// Calculates the dot product of the Plane and the given Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to use.</param>
        /// <returns>The dot product.</returns>
        public float Dot(Vector3 value)
        {
            float res = 0;
            Vector3.Dot(ref Normal, ref value, out res);
            return res + Distance;
        }

        /// <summary>
        /// Calculates the dot product of the Plane and the given Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to use.</param>
        /// <param name="result">The dot product.</param>
        public void Dot(ref Vector3 value, out float result)
        {
            Vector3.Dot(ref Normal, ref value, out result);
            result += Distance;
        }

        /// <summary>
        /// A check to see if a Vector3 intersects with the Plane.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>The type of intersection.</returns>
        public IntersectionType Intersect(Vector3 position)
        {
            float dist = Vector3.Dot(Normal, position);
            dist += Distance;

            if (dist < 0)
                return IntersectionType.Back;
            else if (dist > 0)
                return IntersectionType.Front;
            else
                return IntersectionType.Intersect;
        }

        /// <summary>
        /// Normalizes the Plane.
        /// </summary>
        public void Normalize()
        {
            float dist = Normal.LenghtSquared;

            if (dist != 0)
            {
                float inverseDist = 1 / MathHelper.Sqrt(dist);
                Normal.X *= inverseDist;
                Normal.Y *= inverseDist;
                Normal.Z *= inverseDist;
                Distance *= inverseDist;
            }
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// A check to see if this Plane equals an object.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>The result of the check.</returns>
        public override bool Equals(object obj)
        {
            return obj is Plane ? Equals((Plane)obj) : base.Equals(obj);
        }

        /// <summary>
        /// Gets this Plane's hash code.
        /// </summary>
        /// <returns>This Plane's hash code.</returns>
        public override int GetHashCode()
        {
            return Normal.GetHashCode() ^ Distance.GetHashCode();
        }

        /// <summary>
        /// Converts this Plane into a string.
        /// </summary>
        /// <returns>This Plane as a string.</returns>
        public override string ToString()
        {
            return "Normal = " + Normal.ToString() +
                ", Distance = " + Distance.ToString();
        }

        /// <summary>
        /// A check to see if two Planes are equal.
        /// </summary>
        /// <param name="other">The Plane to check.</param>
        /// <returns>The result of the check.</returns>
        public bool Equals(Plane other)
        {
            return this == other;
        }

        /// <summary>
        /// Saves this Plane.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        public void Save(BinaryWriter writer)
        {
            Normal.Save(writer);
            writer.Write(Distance);
        }

        /// <summary>
        /// Loads in a Plane.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            Normal.Load(reader);
            Distance = reader.ReadSingle();
        }

        /// <summary>
        /// A check to see if two objects intersect.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check against.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(BoundingSphere sphere)
        {
            float dist = Vector3.Dot(Normal, sphere.Center);
            dist += Distance;

            if (dist > sphere.Radius)
                return false;
            else
                return true;
        }

        /// <summary>
        /// A check to see if two objects intersect.
        /// </summary>
        /// <param name="box">The BoundingBox to check against.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(BoundingBox box)
        {
            Vector3 min = Vector3.Zero;
            Vector3 max = Vector3.Zero;

            if (Normal.X <= 0)
            {
                min.X = box.Min.X;
                max.X = box.Max.X;
            }
            else
            {
                min.X = box.Max.X;
                max.X = box.Min.X;
            }

            if (Normal.Y <= 0)
            {
                min.Y = box.Min.Y;
                max.Y = box.Max.Y;
            }
            else
            {
                min.Y = box.Max.Y;
                max.Y = box.Min.Y;
            }

            if (Normal.Z <= 0)
            {
                min.Z = box.Min.Z;
                max.Z = box.Max.Z;
            }
            else
            {
                min.Z = box.Max.Z;
                max.Z = box.Min.Z;
            }

            float dist = Vector3.Dot(Normal, min);

            if (dist < Distance)
                return false;
            else
            {
                dist = Vector3.Dot(Normal, max);

                if (dist > Distance)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// A check to see if two objects intersect.
        /// </summary>
        /// <param name="ray">The Ray to check.</param>
        /// <param name="pointOfContact">The point of contact.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(Ray ray, out Vector3 pointOfContact)
        {
            return ray.Intersects(this, out pointOfContact);
        }

        /// <summary>
        /// A check to see if two objects intersect.
        /// </summary>
        /// <param name="plane">The Ray to check.</param>
        /// <param name="pointOfContact">The point of contact.</param>
        /// <returns>The result of the check.</returns>
        public bool Intersects(Plane plane, out Vector3 pointOfContact)
        {
            pointOfContact = Vector3.Zero;
            Vector3 direct = Vector3.Cross(Normal, plane.Normal);

            float denom = Vector3.Dot(direct, direct);

            if (MathHelper.Abs(denom) < MathHelper.Epsilon)
                return false;
            else
            {
                Vector3 v = Distance * plane.Normal - plane.Distance * Normal;
                pointOfContact = Vector3.Cross(v, direct);
                return true;
            }
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two Planes are equal.
        /// </summary>
        /// <param name="plane1">Plane 1.</param>
        /// <param name="plane2">Plane 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Plane plane1, Plane plane2)
        {
            return plane1.Distance == plane2.Distance &&
                plane1.Normal == plane2.Normal;
        }

        /// <summary>
        /// A check to see if two Planes are not equal.
        /// </summary>
        /// <param name="plane1">Plane 1.</param>
        /// <param name="plane2">Plane 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Plane plane1, Plane plane2)
        {
            return plane1.Distance != plane2.Distance ||
                plane1.Normal != plane2.Normal;
        }

        #endregion
    }
}
