using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Ulanthos.Math.Bounding
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct BoundingBox : IEquatable<BoundingBox>, IContainable
    {
        #region Properties

        /// <summary>
        /// Minimum corner of this BoundingBox. 
        /// Aka bottom left corner.
        /// </summary>
        public Vector3 Min;

        /// <summary>
        /// Maximum corner of this BoundingBox.
        /// Aka top right corner.
        /// </summary>
        public Vector3 Max;

        /// <summary>
        /// The center of this BoundingBox.
        /// </summary>
        public Vector3 Center
        {
            get
            {
                return new Vector3((Min.X + Max.X) / 2,
                    (Min.Y + Max.Y) / 2, (Min.Z + Max.Z) / 2);
            }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Creates a new BoundingBox.
        /// </summary>
        /// <param name="min">Minimum values for this.</param>
        /// <param name="max">Maximum values for this.</param>
        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Creates a new BoundingBox.
        /// </summary>
        /// <param name="min">Minimum values for this.</param>
        /// <param name="max">Maximum values for this.</param>
        public BoundingBox(float min, float max) : this(new Vector3(min), new Vector3(max)) { }

        /// <summary>
        /// Creates a new BoundingBox from a BinaryReader.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public BoundingBox(BinaryReader reader)
            : this()
        {
            Load(reader);
        }

        /// <summary>
        /// Creates a new BoundingBox from a formatted string. 
        /// </summary>
        /// <param name="value">The string to use.</param>
        /// <returns>The generated BoundingBox.</returns>
        public BoundingBox CreateFromFormattedString(string value)
        {
            value = value.Replace("(", "");
            value = value.Replace(")", "");
            value = value.Replace("{", "");
            value = value.Replace("}", "");

            string[] values = value.Split(new[]
			{
				',', ' '
			},
                StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 6)
                return new BoundingBox(new Vector3(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]),
                    Convert.ToInt32(values[2])), new Vector3(Convert.ToInt32(values[3]), Convert.ToInt32(values[4]),
                    Convert.ToInt32(values[5])));

            return new BoundingBox(0, 0);
        }

        /// <summary>
        /// Creates a BoundingBox from a set of positions.
        /// </summary>
        /// <param name="positions">The collection of positions to use.</param>
        /// <returns>The generated BoundingBox.</returns>
        public BoundingBox CreateFrom(IList<Vector3> positions)
        {
            Vector3 min = positions[0];
            Vector3 max = positions[0];

            for (int i = 1; i < positions.Count; i++)
            {
                Vector3 pos = positions[i];

                if (pos.X < min.X)
                    min.X = pos.X;
                if (pos.Y < min.Y)
                    min.Y = pos.Y;
                if (pos.Z < min.Z)
                    min.Z = pos.Z;

                if (pos.X > max.X)
                    max.X = pos.X;
                if (pos.Y > max.Y)
                    max.Y = pos.Y;
                if (pos.Z > max.Z)
                    max.Z = pos.Z;
            }

            if (min.X == max.X)
            {
                min.X -= MathHelper.Epsilon;
                max.X += MathHelper.Epsilon;
            }
            if (min.Y == max.Y)
            {
                min.Y -= MathHelper.Epsilon;
                max.Y += MathHelper.Epsilon;
            }
            if (min.Z == max.Z)
            {
                min.Z -= MathHelper.Epsilon;
                max.Z += MathHelper.Epsilon;
            }

            return new BoundingBox(min, max);
        }

        #endregion
        #region Other

        /// <summary>
        /// Adds / replaces a point on the BoundingBox.
        /// </summary>
        /// <param name="point">The Vector3 to add / replace.</param>
        public void AddPoint(ref Vector3 point)
        {
            Vector3.Min(ref Min, ref point, out Min);
            Vector3.Max(ref Max, ref point, out Max);
        }

        /// <summary>
        /// Adds / replaces a point on the BoundingBox.
        /// </summary>
        /// <param name="point">The Vector3 to add / replace.</param>
        public void AddPoint(Vector3 point)
        {
            AddPoint(ref point);
        }

        /// <summary>
        /// Transforms this BoundingBox by a Matrix.
        /// </summary>
        /// <param name="transform">The matrix to use for the transformation.</param>
        public void Transform(ref Matrix transform)
        {
            Vector3 half = (Max - Min) / 2;
            Vector3 center = (Max + Min) / 2;

            Vector3.Transform(ref center, ref transform, out center);

            Matrix abs;
            MathHelper.Abs(ref transform, out abs);

            Vector3.Transform(ref half, ref abs, out half);

            Min = center - half;
            Max = center + half;
        }

        /// <summary>
        /// Merges two BoundingBoxes togeather.
        /// </summary>
        /// <param name="other">The BoundingBox to merge with.</param>
        public void CreateMerged(BoundingBox other)
        {
            if (other.Min.X < Min.X)
                Min.X = other.Min.X;
            if (other.Max.X > Max.X)
                Max.X = other.Max.X;
            if (other.Min.Y < Min.Y)
                Min.Y = other.Min.Y;
            if (other.Max.Y > Max.Y)
                Max.Y = other.Max.Y;
            if (other.Min.Z < Min.Z)
                Min.Z = other.Min.Z;
            if (other.Max.Z > Max.Z)
                Max.Z = other.Max.Z;
        }

        /// <summary>
        /// Creates a new BoundingSphere from this.
        /// </summary>
        /// <returns>The generated BoundingSphere.</returns>
        public BoundingSphere ConvertToBoundingSphere()
        {
            float radius = ((Max - Min) / 2).Lenght;

            return new BoundingSphere(Center, radius);
        }

        /// <summary>
        /// Finds the corners of this BoundingBox.
        /// </summary>
        /// <returns>The corners of this BoundingBox.</returns>
        public Vector3[] GetCorners()
        {
            Vector3[] corners = new Vector3[]
            {
                new Vector3(Max.X, Max.Y, Max.Z),
                new Vector3(Min.X, Max.Y, Max.Z),
                new Vector3(Max.X, Min.Y, Max.Z),
                new Vector3(Min.X, Min.Y, Max.Z),
                new Vector3(Min.X, Max.Y, Min.Z),
                new Vector3(Max.X, Max.Y, Min.Z),
                new Vector3(Max.X, Min.Y, Min.Z),
                new Vector3(Min.X, Min.Y, Min.Z)
            };

            return corners;
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// Converts the BoundingBox into a formatted string safe for loading in.
        /// </summary>
        /// <returns>The BoundingBox as a formatted string.</returns>
        public string ToFormattedString()
        {
            return Min.ToFormattedString() + ", " + Max.ToFormattedString();
        }

        /// <summary>
        /// A check to see if an object is equal to this BoundingBox.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>The Result.</returns>
        public override bool Equals(object obj)
        {
            return obj is BoundingBox ? Equals((BoundingBox)obj) : base.Equals(obj);
        }

        /// <summary>
        /// Gets the hash code for this BoundingBox.
        /// </summary>
        /// <returns>The BoundingBox's hash code.</returns>
        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        /// <summary>
        /// Converts this BoundingBox into a string.
        /// </summary>
        /// <returns>This BoundingBox as a string.</returns>
        public override string ToString()
        {
            return "Min = " + Min.ToString() + " Max = " + Max.ToString();
        }

        /// <summary>
        /// A check to see if two BoundingBox's are equal. 
        /// </summary>
        /// <param name="other">The other BoundingBox.</param>
        /// <returns>The Result.</returns>
        public bool Equals(BoundingBox other)
        {
            return this == other;
        }

        /// <summary>
        /// Saves this BoundingBox.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        public void Save(BinaryWriter writer)
        {
            Min.Save(writer);
            Max.Save(writer);
        }

        /// <summary>
        /// Loads in a BoundingBox.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            Min.Load(reader);
            Max.Load(reader);
        }

        /// <summary>
        /// A check to see if the given Vector3 is inside this BoundingBox.
        /// </summary>
        /// <param name="sphere">The Vector3 to check.</param>
        /// <returns>The result of the check.</returns>
        public ContainableTypes Contains(Vector3 position)
        {
            if (Min.X <= position.X && Max.X >= position.X &&
                Min.Y <= position.Y && Max.Y >= position.Y &&
                Min.Z <= position.Z && Max.Z >= position.Z)
                return ContainableTypes.Fully;
            else
                return ContainableTypes.None;
        }

        /// <summary>
        /// A check to see if the given BoundingSphere is inside this BoundingBox.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check.</param>
        /// <returns>The result of the check.</returns>
        public ContainableTypes Contains(BoundingSphere sphere)
        {
            Vector3 closestPos = Vector3.Clamp(sphere.Center, Min, Max);
            float dist = Vector3.DistanceSquared(sphere.Center, closestPos);

            if (dist > sphere.Radius * sphere.Radius)
                return ContainableTypes.None;
            else if (Min.X + sphere.Radius <= sphere.Center.X &&
                sphere.Center.X <= Max.X - sphere.Radius &&
                Max.X - Min.X > sphere.Radius &&
                Min.Y + sphere.Radius <= sphere.Center.Y &&
                sphere.Center.Y <= Max.Y - sphere.Radius &&
                Max.Y - Min.Y > sphere.Radius &&
                Min.Z + sphere.Radius <= sphere.Center.Z &&
                sphere.Center.Z <= Max.Z - sphere.Radius &&
                Max.Z - Min.Z > sphere.Radius)
                return ContainableTypes.Fully;
            else
                return ContainableTypes.Partial;
        }

        /// <summary>
        /// A check to see if the given BoundingBox is inside this BoundingBox.
        /// </summary>
        /// <param name="sphere">The BoundingBox to check.</param>
        /// <returns>The result of the check.</returns>
        public ContainableTypes Contains(BoundingBox box)
        {
            if (Max.X < box.Min.X || Min.X > box.Max.X ||
                Max.Y < box.Min.Y || Min.Y > box.Max.Y ||
                Max.Z < box.Min.Z || Min.Z > box.Max.Z)
                return ContainableTypes.None;

            else if (Min.X <= box.Min.X && Max.X >= box.Max.X &&
                Min.Y <= box.Min.Y && Max.Y >= box.Max.Y &&
                Min.Z <= box.Min.Z && Max.Z >= box.Max.Z)
                return ContainableTypes.Fully;

            return ContainableTypes.Partial;
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two BoundingBoxes are equal.
        /// </summary>
        /// <param name="sphere1">BoundingBox 1.</param>
        /// <param name="sphere2">BoundingBox 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(BoundingBox box1, BoundingBox box2)
        {
            return box1.Min == box2.Min && box1.Max == box2.Max;
        }

        /// <summary>
        /// A check to see if two BoundingBoxes are not equal.
        /// </summary>
        /// <param name="sphere1">BoundingBox 1.</param>
        /// <param name="sphere2">BoundingBox 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(BoundingBox box1, BoundingBox box2)
        {
            return box1.Min != box2.Min || box1.Max != box2.Max;
        }

        #endregion
    }
}
