using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Helpers;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines a vector that has three components.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Vector3 : IEquatable<Vector3>
    {
        #region Properties

        public const int SizeInBytes = 3 * 4;

        /// <summary>
        /// X co-ordinate.
        /// </summary>
        [FieldOffset(0)]
        public float X;

        /// <summary>
        /// Y co-ordinate.
        /// </summary>
        [FieldOffset(1)]
        public float Y;

        /// <summary>
        /// Z co-ordinate.
        /// </summary>
        [FieldOffset(2)]
        public float Z;

        /// <summary>
        /// Lenght Squared of the Vector3.
        /// </summary>
        public float LenghtSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        /// <summary>
        /// Lenght of the Vector2.
        /// </summary>
        public float Lenght
        {
            get { return MathHelper.Sqrt(LenghtSquared); }
        }

        /// <summary>
        /// Is the Vector3 normalized ?
        /// </summary>
        public bool IsNormalized
        {
            get
            {
                return MathHelper.Abs(LenghtSquared - 1) <
                    MathHelper.Epsilon * MathHelper.Epsilon;
            }
        }

        /// <summary>
        /// A check to see if this Vector3 is zero.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return X == 0 && Y == 0 && Z == 0;
            }
        }

        /// <summary>
        /// A check to see if this Vector3 is close enough 
        /// to zero to be counted as zero.
        /// </summary>
        public bool IsCloseToZero
        {
            get
            {
                return X <= MathHelper.Epsilon &&
                    Y <= MathHelper.Epsilon &&
                    Z <= MathHelper.Epsilon;
            }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Creates a new Vector3.
        /// </summary>
        /// <param name="x">X co-ordinate.</param>
        /// <param name="y">Y co-ordinate.</param>
        /// <param name="z">Z co-ordinate.</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a new Vector3.
        /// </summary>
        /// <param name="value">Co-ordinate's value.</param>
        public Vector3(float value) : this(value, value, value) { }

        /// <summary>
        /// Creates a new Vector3.
        /// </summary>
        /// <param name="value">X, Y co-ordinate value.</param>
        public Vector3(Vector2 value) : this(value.X, value.Y, 0) { }

        /// <summary>
        /// Creates a new Vector3.
        /// </summary>
        /// <param name="value">The string to get the co-ordinate values from.</param>
        public Vector3(string value)
        {
            Vector3 vec = GetFromString(value);
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
        }

        /// <summary>
        /// Creates a new Vector3.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public Vector3(BinaryReader reader)
            : this()
        {
            Load(reader);
        }

        #endregion
        #region Defaults

        /// <summary>
        /// Returns (1, 1, 1).
        /// </summary>
        public static readonly Vector3 One = new Vector3(1);

        /// <summary>
        /// Returns (0.5, 0.5, 0.5).
        /// </summary>
        public static readonly Vector3 Half = new Vector3(0.5f);

        /// <summary>
        /// Returns (0, 0, 0).
        /// </summary>
        public static readonly Vector3 Zero = new Vector3(0);

        /// <summary>
        /// Returns the Vector3 (1, 0, 0).
        /// </summary>
        public static readonly Vector3 UnitX = new Vector3(1.0f, 0, 0);

        /// <summary>
        /// Returns the Vector3 (0, 1, 0). 
        /// </summary>
        public static readonly Vector3 UnitY = new Vector3(0, 1.0f, 0);

        /// <summary>
        /// Returns the Vector3 (0, 0, 1).
        /// </summary>
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1.0f);

        /// <summary>
        /// Returns the Vector3 (-1, 0, 0).
        /// </summary>
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);

        /// <summary>
        /// Returns the Vector3 (1, 0, 0).
        /// </summary>
        public static readonly Vector3 Right = new Vector3(1, 0, 0);

        /// <summary>
        /// Returns the Vector3 (0, -1, 0).
        /// </summary>
        public static readonly Vector3 Up = new Vector3(0, -1, 0);

        /// <summary>
        /// Returns the Vector3 (0, 1, 0).
        /// </summary>
        public static readonly Vector3 Down = new Vector3(0, 1, 0);

        /// <summary>
        /// Returns the Vector3 (0, 0, -1).
        /// </summary>
        public static readonly Vector3 Forward = new Vector3(0, 0, -1);

        /// <summary>
        /// Returns the Vector3 (0, 0, 1).
        /// </summary>
        public static readonly Vector3 Backward = new Vector3(0, 0, 1);

        #endregion
        #region Calculations
        #region Basics

        /// <summary>
        /// Sets all components of this Vector3 to zero.
        /// </summary>
        public void SetToZero()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Changes the signs of all parts of the Vector3.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <param name="result">Vector3 to use as the result.</param>
        public static void Negate(ref Vector3 value, ref Vector3 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }

        /// <summary>
        /// Changes the signs of all parts of the Vector3.
        /// </summary>
        /// <param name="value">The value to be nagated.</param>
        /// <returns>The negated Vector3.</returns>
        public static Vector3 Negate(Vector3 value)
        {
            return new Vector3(-value.X, -value.Y, -value.Z);
        }

        /// <summary>
        /// Changes the signs of all parts of this Vector3.
        /// </summary>
        public void Negate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Divides a Vector3 by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The amount to divide by.</param>
        /// <returns>The divided Vector3.</returns>
        public static Vector3 Divide(Vector3 value, float divisor)
        {
            return new Vector3(value.X / divisor, value.Y / divisor, value.Z / divisor);
        }

        /// <summary>
        /// Divides a Vector3 by a divisor.
        /// </summary>
        /// <param name="value1">The Vector3 to be divided.</param>
        /// <param name="value2">The Vector3 to devide by.</param>
        /// <returns>The result of the division.</returns>
        public static Vector3 Divide(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X / value2.X, value1.Y / value2.Y, value1.Z / value2.Z);
        }

        /// <summary>
        /// Divides a Vector3 by a divisor.
        /// </summary>
        /// <param name="value">The Vector3 to be divided.</param>
        /// <param name="factor">The factor to divide the Vector3 by.</param>
        /// <param name="result">The result of the division.</param>
        public static void Devide(ref Vector3 value, ref float factor, out Vector3 result)
        {
            result = new Vector3(value.X / factor, value.Y / factor, value.Z / factor);
        }

        /// <summary>
        /// Multiplies a Vector3 by a factor.
        /// </summary>
        /// <param name="value">The Vector3 to multiply.</param>
        /// <param name="factor">The factor to multiply the Vector3 by.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector3 Multiply(Vector3 value, float factor)
        {
            return new Vector3(value.X * factor, value.Y * factor, value.Z * factor);
        }

        /// <summary>
        /// Multiplies a Vector3 by a factor.
        /// </summary>
        /// <param name="value1">The Vector3 to be multiplied.</param>
        /// <param name="value2">The Vector3 to multiply by.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector3 Multiply(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);
        }

        /// <summary>
        /// Multiplies a Vector3 by a factor.
        /// </summary>
        /// <param name="value">The Vector3 to multiply.</param>
        /// <param name="factor">The factor to multiply the Vector3 by.</param>
        /// <param name="result">The result of the multiplication.</param>
        public static void Multiply(ref Vector3 value, ref float factor, out Vector3 result)
        {
            result = new Vector3(value.X * factor, value.Y * factor, value.Z * factor);
        }

        /// <summary>
        /// Adds a number to a Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to added.</param>
        /// <param name="factor">The number to be added to the Vector3 by.</param>
        /// <returns>The result of the addition.</returns>
        public static Vector3 Add(Vector3 value, float factor)
        {
            return new Vector3(value.X + factor, value.Y + factor, value.Z + factor);
        }

        /// <summary>
        /// Adds two Vector3's.
        /// </summary>
        /// <param name="value1">The Vector3 to use.</param>
        /// <param name="value2">The Vector3 to be added.</param>
        /// <returns>The result of the addition.</returns>
        public static Vector3 Add(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);
        }

        /// <summary>
        /// Adds a number to a Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to added.</param>
        /// <param name="factor">The number to be added to the Vector3 by.</param>
        /// <param name="result">The result of the addition.</param>
        public static void Add(ref Vector3 value, ref float factor, out Vector3 result)
        {
            result = new Vector3(value.X + factor, value.Y + factor, value.Z + factor);
        }

        /// <summary>
        /// Adds two Vector3's.
        /// </summary>
        /// <param name="value">The Vector3 to added.</param>
        /// <param name="factor">The Vector3 to be added to the Vector3.</param>
        /// <param name="result">The result of the addition.</param>
        public static void Add(ref Vector3 value, ref Vector3 factor, out Vector3 result)
        {
            result = new Vector3(value.X + factor.X, value.Y + factor.Y,
                value.Z + factor.Z);
        }

        /// <summary>
        /// Subtracts a number from a Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to subtracted.</param>
        /// <param name="factor">The number to be subtracted from the Vector3.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Vector3 Subtract(Vector3 value, float factor)
        {
            return new Vector3(value.X - factor, value.Y - factor, value.Z - factor);
        }

        /// <summary>
        /// Subtracts two Vector3's.
        /// </summary>
        /// <param name="value1">The Vector3 to be subtracted from.</param>
        /// <param name="value2">The Vector3 to be taken away.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Vector3 Subtract(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
        }

        /// <summary>
        /// Subtracts a number to a Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to subtracted.</param>
        /// <param name="factor">The number to be subtracted from the Vector3.</param>
        /// <param name="result">The result of the subtraction..</param>
        public static void Subtract(ref Vector3 value, ref float factor, out Vector3 result)
        {
            result = new Vector3(value.X - factor, value.Y - factor,
                value.Z - factor);
        }

        /// <summary>
        /// Subtracts two Vector3's.
        /// </summary>
        /// <param name="value">The Vector3 to subtracted.</param>
        /// <param name="factor">The Vector3 to be subtracted from the Vector3.</param>
        /// <param name="result">The result of the subtraction..</param>
        public static void Subtract(ref Vector3 value, ref Vector3 factor, out Vector3 result)
        {
            result = new Vector3(value.X - factor.X, value.Y - factor.Y,
                value.Z - factor.Z);
        }

        /// <summary>
        /// Switches the values of one Vector3 onto another.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        public static void Switch(ref Vector3 value1, ref Vector3 value2)
        {
            float temp;

            temp = value2.X;
            value2.X = value1.X;
            value1.X = temp;

            temp = value2.Y;
            value2.Y = value1.Y;
            value1.Y = temp;

            temp = value2.Z;
            value2.Z = value1.Z;
            value1.Z = temp;
        }

        #endregion
        #region Essentials

        /// <summary>
        /// Normalizes this Vector3.
        /// </summary>
        public void Normalize()
        {
            float lenghtSquared = LenghtSquared;

            if (lenghtSquared != 0)
            {
                float inverseDist = 1 / MathHelper.Sqrt(lenghtSquared);
                X *= inverseDist;
                Y *= inverseDist;
                Z *= inverseDist;
            }
        }

        /// <summary>
        /// Normalizes a Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to normalize.</param>
        /// <returns>The normalized Vector3.</returns>
        public static Vector3 Normalize(Vector3 value)
        {
            float lenghtSquared = value.LenghtSquared;

            if (lenghtSquared != 0)
            {
                float dist = 1 / MathHelper.Sqrt(lenghtSquared);

                return new Vector3(value.X * dist, value.Y * dist, value.Z * dist);
            }

            return value;
        }

        /// <summary>
        /// Normalizes a Vector3.
        /// </summary>
        /// <param name="value">The Vector3 to normalize.</param>
        public static void Normalize(ref Vector3 value)
        {
            float lenghtSquared = value.LenghtSquared;

            if (lenghtSquared != 0)
            {
                float dist = 1 / MathHelper.Sqrt(lenghtSquared);

                value.X *= dist;
                value.Y *= dist;
                value.Z *= dist;
            }
        }

        /// <summary>
        /// Gets the dot product of two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>The dot product of the two Vector3's.</returns>
        public static float Dot(Vector3 value1, Vector3 value2)
        {
            return ((value1.X * value2.X) + (value1.Y *
                value2.Y) + (value1.Z * value2.Z));
        }

        /// <summary>
        /// Gets the dot product of two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <param name="result">The result of the dot product.</param>
        public static void Dot(ref Vector3 value1,
            ref Vector3 value2, out float result)
        {
            result = ((value1.X * value2.X) + (value1.Y *
                value2.Y) + (value1.Z * value2.Z));
        }

        /// <summary>
        /// Gets the cross product of two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>The cross product of the two Vector3's.</returns>
        public static Vector3 Cross(Vector3 value1, Vector3 value2)
        {
            return new Vector3(((value1.Y * value2.Z) - (value1.Z * value2.Y)),
                ((value1.Z * value2.X) - (value1.X * value2.Z)),
                ((value1.X * value2.Y) - (value1.Y * value2.X)));
        }

        /// <summary>
        /// Gets the cross product of two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <param name="result">The result of the cross product.</param>
        public static void Cross(ref Vector3 value1,
            ref Vector3 value2, out Vector3 result)
        {
            result.X = ((value1.Y * value2.Z) - (value1.Z * value2.Y));
            result.Y = ((value1.Z * value2.X) - (value1.X * value2.Z));
            result.Z = ((value1.X * value2.Y) - (value1.Y * value2.X));
        }

        /// <summary>
        /// Clamps the value of the Vector3 between minimum and maximum values. 
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">Minimum value the given value can be.</param>
        /// <param name="max">Maximum value the given value can be.</param>
        /// <returns>The clamped Vector3.</returns>
        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            float x = value.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = value.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            float z = value.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Clamps the value of the Vector3 between minimum and maximum values. 
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">Minimum value the given value can be.</param>
        /// <param name="max">Maximum value the given value can be.</param>
        /// <param name="result">The result of the clamp.</param>
        public static void Clamp(ref Vector3 value,
            ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            float x = value.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            float y = value.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            float z = value.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Returns the minimum value of the two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>Minimum Vector3 from the two Vector3's.</returns>
        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X < value2.X ? value1.X : value2.X,
                value1.Y < value2.Y ? value1.Y : value2.Y,
                value1.Z < value2.Z ? value1.Z : value2.Z);
        }

        /// <summary>
        /// Returns the minimum value of the two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <param name="result">The Vector3 to be used as the result.</param>
        public static void Min(ref Vector3 value1,
            ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
            result.Z = value1.Z < value2.Z ? value1.Z : value2.Z;
        }

        /// <summary>
        /// Returns the minimum value of the two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>Minimum Vector3 from the two Vector3's.</returns>
        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            return new Vector3(value1.X > value2.X ? value1.X : value2.X,
                value1.Y > value2.Y ? value1.Y : value2.Y,
                value1.Z > value2.Z ? value1.Z : value2.Z);
        }

        /// <summary>
        /// Returns the maximum value of the two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <param name="result">The Vector3 to be used as the result.</param>
        public static void Max(ref Vector3 value1,
            ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
            result.Z = value1.Z > value2.Z ? value1.Z : value2.Z;
        }

        /// <summary>
        /// Preforms a linear interpolation between two Vector3's.
        /// </summary>
        /// <param name="original">Origional Vector3.</param>
        /// <param name="toLerpTo">Vector3 to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>Result of the lerp.</returns>
        public static Vector3 Lerp(Vector3 original,
            Vector3 toLerpTo, float amount)
        {
            return new Vector3(MathHelper.Lerp(original.X, toLerpTo.X, amount),
                MathHelper.Lerp(original.Y, toLerpTo.Y, amount),
                MathHelper.Lerp(original.Z, toLerpTo.Z, amount));
        }

        /// <summary>
        /// Preforms a linear interpolation between two Vector3's.
        /// </summary>
        /// <param name="original">Origional Vector3.</param>
        /// <param name="toLerpTo">Vector3 to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <param name="result">Result of the lerp.</param>
        public static void Lerp(ref Vector3 original,
            ref Vector3 toLerpTo, ref float amount, out Vector3 result)
        {
            result.X = MathHelper.Lerp(original.X, toLerpTo.X, amount);
            result.Y = MathHelper.Lerp(original.Y, toLerpTo.Y, amount);
            result.Z = MathHelper.Lerp(original.Z, toLerpTo.Z, amount);
        }

        #endregion
        #region Distance

        /// <summary>
        /// Gets the angle between two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>The angle between the Vector3's in degrees.</returns>
        public static float AngleBetween(Vector3 value1, Vector3 value2)
        {
            if (value1 == Zero || value2 == Zero)
                return 0;

            float dot = Dot(Normalize(value1), Normalize(value2));
            dot = MathHelper.Clamp(dot, -1, 1);

            Vector3 cross = Cross(value1, value2);

            return cross.Z < 0 ? 360 - MathHelper.ACos(dot) :
                MathHelper.ACos(dot);
        }

        /// <summary>
        /// Calculates the squared distance between two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>Distance Squared.</returns>
        public static float DistanceSquared(Vector3 value1, Vector3 value2)
        {
            float res;
            DistanceSquared(ref value1, ref value2, out res);
            return res;
        }

        /// <summary>
        /// Calculates the squared distance between two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <param name="result">Distance Squared.</param>
        public static void DistanceSquared(ref Vector3 value1,
            ref Vector3 value2, out float result)
        {
            result = ((value1.X - value2.X) * (value1.X - value2.X) +
                (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                (value1.Z - value2.Z) * (value1.Z - value2.Z));
        }

        /// <summary>
        /// Calculates the distance between two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>Distance.</returns>
        public static float Distance(Vector3 value1, Vector3 value2)
        {
            float res;
            DistanceSquared(ref value1, ref value2, out res);
            return MathHelper.Sqrt(res);
        }

        /// <summary>
        /// Calculates the distance between two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <param name="result">Distance.</param>
        public static void Distance(ref Vector3 value1,
            ref Vector3 value2, out float result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = MathHelper.Sqrt(result);
        }

        #endregion
        #region Transformations

        /// <summary>
        /// Transforms a Vector3 by a Matrix.
        /// </summary>
        /// <param name="vector">The Vector to transform.</param>
        /// <param name="matrix">The Matrix to use.</param>
        /// <returns>The transformed Vector3.</returns>
        public static Vector3 Transform(Vector3 vector, Matrix matrix)
        {
            return new Vector3(
                (vector.X * matrix.M11) + (vector.Y * matrix.M21) +
                (vector.Z * matrix.M31) + matrix.M41,
                (vector.X * matrix.M12) + (vector.Y * matrix.M22) +
                (vector.Z * matrix.M32) + matrix.M42,
                (vector.X * matrix.M13) + (vector.Y * matrix.M23) +
                (vector.Z * matrix.M33) + matrix.M43);
        }

        /// <summary>
        /// Transforms a Vector3 by a Matrix.
        /// </summary>
        /// <param name="vector">The Vector to transform.</param>
        /// <param name="matrix">The Matrix to use.</param>
        /// <param name="result">The transformed Vector3.</param>
        public static void Transform(ref Vector3 vector,
            ref Matrix matrix, out Vector3 result)
        {
            result = new Vector3(
                (vector.X * matrix.M11) + (vector.Y * matrix.M21) +
                (vector.Z * matrix.M31) + matrix.M41,
                (vector.X * matrix.M12) + (vector.Y * matrix.M22) +
                (vector.Z * matrix.M32) + matrix.M42,
                (vector.X * matrix.M13) + (vector.Y * matrix.M23) +
                (vector.Z * matrix.M33) + matrix.M43);
        }

        /// <summary>
        /// Transforms the given normal by a Matrix.
        /// </summary>
        /// <param name="normal">Normal for transformation.</param>
        /// <param name="matrix">Matrix to use.</param>
        /// <returns>Transformed Normal.</returns>
        public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
        {
            return new Vector3((normal.X * matrix.M11) +
                (normal.Y * matrix.M21) +
                (normal.Z * matrix.M31),
                (normal.X * matrix.M12) +
                (normal.Y * matrix.M22) +
                (normal.Z * matrix.M32),
                (normal.X * matrix.M13) +
                (normal.Y * matrix.M23) +
                (normal.Z * matrix.M33));
        }

        /// <summary>
        /// Transforms the given normal by a Matrix.
        /// </summary>
        /// <param name="normal">Normal for transformation.</param>
        /// <param name="matrix">Matrix to use.</param>
        /// <param name="result">Transformed Normal.</param>
        public static void TransformNormal(ref Vector3 normal,
            ref Matrix matrix, out Vector3 result)
        {
            result = new Vector3((normal.X * matrix.M11) +
                (normal.Y * matrix.M21) +
                (normal.Z * matrix.M31),
                (normal.X * matrix.M12) +
                (normal.Y * matrix.M22) +
                (normal.Z * matrix.M32),
                (normal.X * matrix.M13) +
                (normal.Y * matrix.M23) +
                (normal.Z * matrix.M33));
        }

        /// <summary>
        /// Transforms a Vector3 by the transpose of a given Matrix.
        /// </summary>
        /// <param name="position">The Vector3 to be transformed.</param>
        /// <param name="matrix">The transformation Matrix to use.</param>
        /// <param name="result">The result of the calculation.</param>
        public static void TransposedTransform(ref Vector3 position,
            ref Matrix matrix, out Vector3 result)
        {
            result = new Vector3
                (
                    ((position.X * matrix.M11) + (position.Y * matrix.M12)) + (position.Z * matrix.M31),
                    ((position.X * matrix.M21) + (position.Y * matrix.M22)) + (position.Z * matrix.M23),
                    ((position.X * matrix.M31) + (position.Y * matrix.M32)) + (position.Z * matrix.M33)

                );
        }

        #endregion
        #endregion
        #region Convertion

        /// <summary>
        /// Converts this Vector3 into an array of size 3.
        /// </summary>
        /// <returns>The result of the conversion.</returns>
        public float[] ToArray()
        {
            return new float[]
            {
                X,Y,Z
            };
        }

        /// <summary>
        /// Converts this Vector2 into a Vector2.
        /// </summary>
        /// <returns>The result of the conversion.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        #endregion
        #region String Related

        /// <summary>
        /// Converts the current Vector3 into a loadable formated string.
        /// </summary>
        /// <returns>The Vector3 as a formatted string.</returns>
        public string ToFormattedString()
        {
            return ToFormattedString(this);
        }

        /// <summary>
        /// Converts the current Vector3 into a loadable formated string. 
        /// </summary>
        /// <param name="value">The Vector3 to convert.</param>
        /// <returns>The Vector3 as a formatted string.</returns>
        public static string ToFormattedString(Vector3 value)
        {
            return (StringHelper.NumberToString(value.X) + ", " +
                StringHelper.NumberToString(value.Y) + ", " +
                StringHelper.NumberToString(value.Z));
        }

        /// <summary>
        /// Sets co-ordinates to the values of the string.
        /// </summary>
        /// <param name="value">The string to get the values from.</param>
        /// <returns>The generated Vector3.</returns>
        public static Vector3 GetFromString(string value)
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

            if (values.Length == 3)
                return new Vector3(Convert.ToInt32(values[0]),
                    Convert.ToInt32(values[1]), Convert.ToInt32(values[2]));

            return Zero;
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// A check to see if the Vector3 are equal.
        /// </summary>
        /// <param name="point">The Vector3 to check aginst.</param>
        /// <returns>Result of the check.</returns>
        public bool EqualEnough(Vector3 vector)
        {
            return EqualEnough(vector, MathHelper.Epsilon);
        }

        /// <summary>
        /// A check to see if the Vector3 are equal.
        /// </summary>
        /// <param name="point">The Vector3 to check aginst.</param>
        /// <param name="threshold">The inaccuracy limit.</param>
        /// <returns>Result of the check.</returns>
        public bool EqualEnough(Vector3 vector, float threshold)
        {
            return X - threshold <= vector.X && X + threshold >= vector.X &&
                Y - threshold <= vector.Y && Y + threshold >= vector.Y &&
                Z - threshold <= vector.Z && Z + threshold >= vector.Z;
        }

        /// <summary>
        /// A check to see if an object is equal to this Vector3.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>The Result.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Vector3) && Equals((Vector3)obj);
        }

        /// <summary>
        /// A check to see if two Vector3's are equal. 
        /// </summary>
        /// <param name="other">The other Vector3.</param>
        /// <returns>The Result.</returns>
        public bool Equals(Vector3 other)
        {
            return this == other;
        }

        /// <summary>
        /// Saves this Vector3.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        /// <summary>
        /// Loads in a Vector3.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        /// <summary>
        /// Gets the hash code for this Vector3.
        /// </summary>
        /// <returns>The Vector3's hash code.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Converts this Vector3 into a string.
        /// </summary>
        /// <param name="openBrace">The char to use to signify a opening brace.</param>
        /// <param name="closedBrace">The char to use to signify a closing brace.</param>
        /// <returns>The Vector3 as a string.</returns>
        public string ToString(char openBrace, char closeBrace)
        {
            string format = "0.0000";
            string comma = ", ";

            return (openBrace + StringHelper.NumberToString(X, format) +
                comma + StringHelper.NumberToString(Y, format) + comma +
                StringHelper.NumberToString(Z, format) + closeBrace);
        }

        /// <summary>
        /// Converts this Vector3 into a string.
        /// </summary>
        /// <returns>The Vector3 as a string.</returns>
        public override string ToString()
        {
            return ToString('(', ')');
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two Vector3's are equal.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            return value1.X == value2.X &&
                value1.Y == value2.Y && value1.Z == value2.Z;
        }

        /// <summary>
        /// A check to see if two Vector3 aren't equal.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            return value1.X != value2.X ||
                value1.Y != value2.Y || value1.Z != value2.Z;
        }

        /// <summary>
        /// Gets the cross product of two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>The cross product of the given Vector3's.</returns>
        public static Vector3 operator %(Vector3 value1, Vector3 value2)
        {
            return Cross(value1, value2);
        }

        /// <summary>
        /// Gets the dot product of two Vector3's.
        /// </summary>
        /// <param name="value1">Vector3 1.</param>
        /// <param name="value2">Vector3 2.</param>
        /// <returns>The dot product of the given Vector3's.</returns>
        public static float operator ^(Vector3 value1, Vector3 value2)
        {
            return Dot(value1, value2);
        }

        /// <summary>
        /// Inverts a Vector3.
        /// </summary>
        /// <param name="value">The value to invert.</param>
        /// <returns>The inverted Vector3.</returns>
        public static Vector3 operator -(Vector3 value)
        {
            return new Vector3(-value.X, -value.Y, -value.Z);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted Vector3.</returns>
        public static Vector3 operator -(Vector3 value, Vector3 subtractor)
        {
            return new Vector3(value.X - subtractor.X,
                value.Y - subtractor.Y, value.Z - subtractor.Z);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Vector3 operator +(Vector3 value, float amount)
        {
            return new Vector3(value.X + amount,
                value.Y + amount, value.Z + amount);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Vector3 operator +(Vector3 value, Vector3 amount)
        {
            return new Vector3(value.X + amount.X,
                value.Y + amount.Y, value.Z + amount.Z);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted Vector3.</returns>
        public static Vector3 operator -(Vector3 value, float subtractor)
        {
            return new Vector3(value.X - subtractor,
                value.Y - subtractor, value.Z - subtractor);
        }

        /// <summary>
        /// Divides one Vector3 by another.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Vector3 operator /(Vector3 value, Vector3 divisor)
        {
            return new Vector3(value.X / divisor.X,
                value.Y / divisor.Y, value.Z / divisor.Z);
        }

        /// <summary>
        /// Divides a Vector3 by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Vector3 operator /(Vector3 value, float divisor)
        {
            return new Vector3(value.X / divisor,
                value.Y / divisor, value.Z / divisor);
        }

        /// <summary>
        /// Divides a Vector3 by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Vector3 operator /(float value, Vector3 divisor)
        {
            return new Vector3(divisor.X / value,
                divisor.Y / value, divisor.Z / value);
        }

        /// <summary>
        /// Multiplies one Vector3 by another.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector3 operator *(Vector3 value, Vector3 multiplier)
        {
            return new Vector3(value.X * multiplier.X,
                value.Y * multiplier.Y, value.Z * multiplier.Z);
        }

        /// <summary>
        /// Multiplies a Vector3 by a float.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector3 operator *(Vector3 value, float multiplier)
        {
            return new Vector3(value.X * multiplier,
                value.Y * multiplier, value.Z * multiplier);
        }

        /// <summary>
        /// Multiplies a float by a Vector2.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector3 operator *(float value, Vector3 multiplier)
        {
            return new Vector3(multiplier.X * value,
                multiplier.Y * value, multiplier.Z * value);
        }

        #endregion
    }
}
