using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Helpers;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines a vector who has two components.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Vector2 : IEquatable<Vector2>
    {
        #region Properties

        public const int SizeInBytes = 2 * 4;

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
        /// Lenght Squared of the Vector2.
        /// </summary>
        public float LenghtSquared
        {
            get { return X * X + Y * Y; }
        }

        /// <summary>
        /// Lenght of the Vector2.
        /// </summary>
        public float Lenght
        {
            get { return MathHelper.Sqrt(LenghtSquared); }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Creates a new Vector2.
        /// </summary>
        /// <param name="value">Value.</param>
        public Vector2(float value) : this(value, value) { }

        /// <summary>
        /// Creates a new Vector2.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y Value</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new Vector2.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public Vector2(BinaryReader reader)
            : this()
        {
            Load(reader);
        }

        /// <summary>
        /// Creates a new Vector2 from a string.
        /// </summary>
        /// <param name="value">The string to use.</param>
        public Vector2(string value)
        {
            Vector2 p = GetFromString(value);

            X = p.X;
            Y = p.Y;
        }

        #endregion
        #region Defaults

        /// <summary>
        /// Returns the Vector2 (1, 0).
        /// </summary>
        public static readonly Vector2 UnitX = new Vector2(1, 0);

        /// <summary>
        /// Returns the Vector2 (0, 1).
        /// </summary>
        public static readonly Vector2 UnitY = new Vector2(0, 1);

        /// <summary>
        /// Returns the Vector2 (1, 1).
        /// </summary>
        public static readonly Vector2 One = new Vector2(1);

        /// <summary>
        /// Returns the Vector2 (0.5, 0.5).
        /// </summary>
        public static readonly Vector2 Half = new Vector2(0.5f);

        /// <summary>
        /// Returns the Vector2 (0, 0).
        /// </summary>
        public static readonly Vector2 Zero = new Vector2(0);

        /// <summary>
        /// Returns the Vector2 (-1, -1).
        /// </summary>
        public static readonly Vector2 InValid = new Vector2(-1, -1);

        #endregion
        #region Contains

        /// <summary>
        /// A check to see if a circle is inside a Rectangle.
        /// </summary>
        /// <param name="center">The circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="rectangle">The rectangle to check.</param>
        /// <returns>The result of the check.</returns>
        public static bool IsCircleInsideRect(ref Vector2 center,
            ref float radius, ref Rectangle rectangle)
        {
            return center.X + radius >= rectangle.X &&
                center.X - radius <= rectangle.Right &&
                center.Y + radius >= rectangle.Y &&
                center.Y - radius <= rectangle.Bottom;
        }

        /// <summary>
        /// A check to see if the given Rectangle contains the given Vector2.
        /// </summary>
        /// <param name="point">The Vector2 to check.</param>
        /// <param name="rectangle">The Rectangle to check.</param>
        /// <returns>The result of the check.</returns>
        public static bool IsInsideRect(ref Vector2 vector, ref Rectangle rectangle)
        {
            return vector.X >= rectangle.X && vector.X <= rectangle.Right &&
                vector.Y >= rectangle.Y && vector.Y <= rectangle.Bottom;
        }

        /// <summary>
        /// Returns the position of a Vector2 on a unit circle using a degrees value.
        /// </summary>
        /// <param name="degrees">The degrees to check to position at.</param>
        /// <returns>Angular position on a unit circle</returns>
        public static Vector2 GetAngularPositionOnCircle(float degrees)
        {
            return new Vector2(MathHelper.Sin(degrees), -MathHelper.Cos(degrees));
        }

        #endregion
        #region Calculations

        /// <summary>
        /// Gets the angle between two Vector2.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <returns>The angle between the Vector2 in degrees.</returns>
        public static float AngleBetween(Vector2 value1, Vector2 value2)
        {
            if (value1 == Zero || value2 == Zero)
                return 0;

            float cos = Dot(Normalize(value1), Normalize(value2));
            cos = MathHelper.Clamp(cos, -1, 1);

            float cross = MathHelper.ATan2(value1.Y, value1.X) - MathHelper.ATan2(value2.Y, value2.X);

            return cross < 0 ? 360 - MathHelper.ACos(cos) :
                MathHelper.ACos(cos);
        }

        /// <summary>
        /// Calculates the squared distance between two Vector2's.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <returns>Distance Squared.</returns>
        public static float DistanceSquared(Vector2 value1, Vector2 value2)
        {
            float res;
            DistanceSquared(ref value1, ref value2, out res);
            return res;
        }

        /// <summary>
        /// Calculates the squared distance between two Vector2's.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <param name="result">Distance Squared.</param>
        public static void DistanceSquared(ref Vector2 value1,
            ref Vector2 value2, out float result)
        {
            result = (value1.X - value2.X) * (value1.X - value2.X) +
                (value1.Y - value2.Y) * (value1.Y - value2.Y);
        }

        /// <summary>
        /// Calculates the distance between two Vector2's.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <returns>Distance.</returns>
        public static float Distance(Vector2 value1, Vector2 value2)
        {
            float res;
            DistanceSquared(ref value1, ref value2, out res);
            return MathHelper.Sqrt(res);
        }

        /// <summary>
        /// Calculates the distance between two Vector2's.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <param name="result">Distance.</param>
        public static void Distance(ref Vector2 value1,
            ref Vector2 value2, out float result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = MathHelper.Sqrt(result);
        }

        /// <summary>
        /// Preforms a linear interpolation between two Vector2's.
        /// </summary>
        /// <param name="value1">Origional Vector2.</param>
        /// <param name="value2">Vector2 to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>Result of the lerp.</returns>
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
        {
            return new Vector2(MathHelper.Lerp(value1.X, value2.X, amount),
                MathHelper.Lerp(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Preforms a linear interpolation between two Vector2's.
        /// </summary>
        /// <param name="value1">Origional Vector2.</param>
        /// <param name="value2">Vector2 to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <param name="result">Result of the lerp.</param>
        public static void Lerp(ref Vector2 value1, ref Vector2 value2,
            ref float amount, out Vector2 result)
        {
            result = new Vector2();

            MathHelper.Lerp(ref value1.X, ref value2.X, ref amount, out result.X);
            MathHelper.Lerp(ref value1.Y, ref value2.Y, ref amount, out result.Y);
        }

        /// <summary>
        /// Gets the dot product of two values.
        /// </summary>
        /// <param name="value1">First Vector2.</param>
        /// <param name="value2">Second Vector2.</param>
        /// <returns>The dot product of those two Vector2's.</returns>
        public static float Dot(Vector2 value1, Vector2 value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y;
        }

        /// <summary>
        /// Gets the dot product of two values.
        /// </summary>
        /// <param name="value1">First Vector2.</param>
        /// <param name="value2">Second Vector2.</param>
        /// <returns>The dot product of those two Vector2's.</returns>
        public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            result = value1.X * value2.X + value1.Y * value2.Y;
        }

        /// <summary>
        /// Normalizes a Vector2.
        /// </summary>
        /// <param name="value">The Vector2 to normalize.</param>
        /// <returns>The normalized Vector2.</returns>
        public static Vector2 Normalize(Vector2 value)
        {
            float lenghtSqrd = value.LenghtSquared;

            if (lenghtSqrd != 0)
            {
                float inverseLenght = 1 / MathHelper.Sqrt(lenghtSqrd);

                return new Vector2(value.X * inverseLenght,
                    value.Y * inverseLenght);
            }
            else
                return value;
        }

        /// <summary>
        /// Clamps a Vector2 in the range of min - max.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum that the value can be.</param>
        /// <param name="max">The maximum that the value can be.</param>
        /// <returns>The clamped value.</returns>
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(MathHelper.Clamp(value.X, min.X, max.X),
                MathHelper.Clamp(value.Y, min.Y, max.Y));
        }

        /// <summary>
        /// Clamps a Vector2 in the range of (0, 0) - (1, 1). 
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static Vector2 Clamp(Vector2 value)
        {
            return new Vector2(MathHelper.Clamp(value.X, 0, 1),
                MathHelper.Clamp(value.Y, 0, 1));
        }

        #endregion
        #region Transformation

        /// <summary>
        /// Finds the normal of a Vector2.
        /// </summary>
        /// <param name="vector">Vector2 to find the normal of.</param>
        /// <returns>Normal of the given Vector2.</returns>
        public static Vector2 FindNormal(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        /// <summary>
        /// Transforms a Vector2 by a Matrix.
        /// </summary>
        /// <param name="point">Vector2 to transform.</param>
        /// <param name="matrix">The matrix to transform by.</param>
        /// <param name="result">The result of the transformation.</param>
        public static void TransformPoint(ref Vector2 point,
            ref Matrix matrix, out Vector2 result)
        {
            result = new Vector2((point.X * matrix.M11) + (point.Y * matrix.M21) + matrix.M41,
                (point.X * matrix.M12) + (point.Y * matrix.M22) + matrix.M42);
        }

        /// <summary>
        /// Transforms a Vector2 by a Matrix.
        /// </summary>
        /// <param name="point">Point to transform.</param>
        /// <param name="matrix">The matrix to transform by.</param>
        /// <returns>The result of the transformation.</returns>
        public static Vector2 TransformPoint(Vector2 point, Matrix matrix)
        {
            Vector2 res = new Vector2();
            TransformPoint(ref point, ref matrix, out res);
            return res;
        }

        /// <summary>
        /// Transforms a Vector2 normal by a Matrix.
        /// </summary>
        /// <param name="point">Normal to transform.</param>
        /// <param name="matrix">The Matrix to transform by.</param>
        /// <param name="result">The result of the transformation.</param>
        public static void TransformNormal(ref Vector2 normal,
            ref Matrix matrix, out Vector2 result)
        {
            result = new Vector2((normal.X * matrix.M11) + (normal.Y * matrix.M21),
               (normal.X * matrix.M12) + (normal.Y * matrix.M22));
        }

        /// <summary>
        /// Transforms a Vector2 normal by a Matrix.
        /// </summary>
        /// <param name="point">Normal to transform.</param>
        /// <param name="matrix">The Matrix to transform by.</param>
        /// <returns>The result of the transformation.</returns>
        public static Vector2 TransformNormal(Vector2 normal, Matrix matrix)
        {
            Vector2 res = new Vector2();
            TransformNormal(ref normal, ref matrix, out res);
            return res;
        }

        #endregion
        #region Random

        /// <summary>
        /// Gets a new random Vector2 int the range of 0 - 1.
        /// </summary>
        /// <returns>Random Vector2 between 0 and 1.</returns>
        public static Vector2 RandomPoint()
        {
            return RandomPoint(0, 1);
        }

        /// <summary>
        /// Gets a random Vector2 from a minimum and maximum values.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>Random Vector2 between the minimum and maximum values.</returns>
        public static Vector2 RandomPoint(float min, float max)
        {
            return new Vector2(RandomHelper.GetRandomFloat(min, max),
                RandomHelper.GetRandomFloat(min, max));
        }

        #endregion
        #region Other Calculations

        /// <summary>
        /// Resizes the Vector2.
        /// </summary>
        /// <param name="amount">The amount to resize by.</param>
        /// <returns>The resized Vector2.</returns>
        public Vector2 Resize(float amount)
        {
            return Normalize(this) * amount;
        }

        /// <summary>
        /// Scales up the value of Vector2.
        /// </summary>
        /// <param name="amount">The amount to scale by.</param>
        /// <returns>The scaled Vector2.</returns>
        public Vector2 Scale(float amount)
        {
            return new Vector2(X * amount, Y * amount);
        }

        /// <summary>
        /// Rotates the Vector2 by an angle.
        /// </summary>
        /// <param name="angle">The angle to rotate by in degrees.</param>
        /// <returns>The rotated Vector2.</returns>
        public Vector2 Rotate(float angle)
        {
            float cos = MathHelper.Cos(-angle);
            float sin = MathHelper.Sin(-angle);

            float alt = X * cos + Y * sin;
            Y = -X * sin + Y * cos;
            X = alt;

            return this;
        }

        /// <summary>
        /// Returns the minimum value of the two Vector2.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <returns>Minimum Vector2 from the two Vector2.</returns>
        public static Vector2 Min(Vector2 value1, Vector2 value2)
        {
            return new Vector2(MathHelper.LessThan(value1.X, value2.X),
                MathHelper.LessThan(value1.X, value2.Y));
        }

        /// <summary>
        /// Returns the maximum value of the two Vector2.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <returns>Maximum Vector2 from the two Vector2.</returns>
        public static Vector2 Max(Vector2 value1, Vector2 value2)
        {
            return new Vector2(MathHelper.GreaterThan(value1.X, value2.X),
                MathHelper.GreaterThan(value1.X, value2.Y));
        }

        #endregion
        #region String Related

        /// <summary>
        /// Generates a new Vector2 from a given string.
        /// </summary>
        /// <param name="value">The string to generate the Vector2 from.</param>
        /// <returns>A new Vector2.</returns>
        public static Vector2 GetFromString(string value)
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

            if (values.Length == 2)
                return new Vector2(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));

            return Zero;
        }

        /// <summary>
        /// Converts the current Vector2 into a loadable format.
        /// </summary>
        /// <returns>The Vector2 as a formatted string.</returns>
        public string ToFormattedString()
        {
            return (StringHelper.NumberToString(X) + ", " +
                StringHelper.NumberToString(Y));
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// Saves a Vector2.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }

        /// <summary>
        /// Loads in a Vector2.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        /// <summary>
        /// A check to see if two Vector2's are equal. 
        /// </summary>
        /// <param name="other">The other Vector2.</param>
        /// <returns>The Result.</returns>
        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        /// <summary>
        /// A check to see if two Vector2's are equal. 
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>The Result.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Vector2) && Equals((Vector2)obj);
        }

        /// <summary>
        /// Gets the hash code for this Size.
        /// </summary>
        /// <returns>This object's hash code</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Returns this Vector2 as a string.
        /// </summary>
        /// <returns>Vector2 as a string.</returns>
        public override string ToString()
        {
            return ToString('(', ')');
        }

        /// <summary>
        /// Converts this Vector2 into a string.
        /// </summary>
        /// <param name="openBrace">The char to use to signify a opening brace.</param>
        /// <param name="closedBrace">The char to use to signify a closing brace.</param>
        /// <returns>The Vector2 as a string.</returns>
        public string ToString(char openBrace, char closeBrace)
        {
            string format = "0.0000";
            string comma = ", ";

            return (openBrace + StringHelper.NumberToString(X, format) +
                comma + StringHelper.NumberToString(Y, format));
        }

        #endregion
        #region Other

        /// <summary>
        /// A check to see if the Vector2 are equal.
        /// </summary>
        /// <param name="point">The Vector2 to check aginst.</param>
        /// <returns>Result of the check.</returns>
        public bool EqualEnough(Vector2 vector)
        {
            return EqualEnough(vector, MathHelper.Epsilon);
        }

        /// <summary>
        /// A check to see if the Vector2 are equal.
        /// </summary>
        /// <param name="point">The Vector2 to check aginst.</param>
        /// <param name="threshold">The inaccuracy limit.</param>
        /// <returns>Result of the check.</returns>
        public bool EqualEnough(Vector2 vector, float threshold)
        {
            return X - threshold <= vector.X && X + threshold >= vector.X &&
                Y - threshold <= vector.Y && Y + threshold >= vector.Y;
        }

        /// <summary>
        /// Copies the current Vector2 and adds and offset to it's X co-ordinate.
        /// </summary>
        /// <param name="xOffset">The amount to offset the X co-ordinate by.</param>
        /// <returns>The copied Vector2 with the offset.</returns>
        public Vector2 CopyXOffset(float xOffset)
        {
            return new Vector2(X + xOffset, Y);
        }

        /// <summary>
        /// Copies the current Vector2 and adds and offset to it's Y co-ordinate.
        /// </summary>
        /// <param name="yOffset">The amount to offset the Y co-ordinate by.</param>
        /// <returns>The copied Vector2 with the offset.</returns>
        public Vector2 CopyYOffset(float yOffset)
        {
            return new Vector2(X, Y + yOffset);
        }

        /// <summary>
        /// Copies the current Vector2 and adds and offset to it's X and Y co-ordinates.
        /// </summary>
        /// <param name="xOffset">The amount to offset the X co-ordinate by.</param>
        /// <param name="yOffset">The amount to offset the Y co-ordinate by.</param>
        /// <returns>The copied Vector2 with the offset.</returns>
        public Vector2 CopyYOffset(float xOffset, float yOffset)
        {
            return new Vector2(X + xOffset, Y + yOffset);
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two Vector2 are equal.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Vector2 value1, Vector2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        /// <summary>
        /// A check to see if two Vector2 aren't equal.
        /// </summary>
        /// <param name="value1">Vector2 1.</param>
        /// <param name="value2">Vector2 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Vector2 value1, Vector2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        /// <summary>
        /// Inverts a Vector2.
        /// </summary>
        /// <param name="value">The value to invert.</param>
        /// <returns>The inverted Vector2.</returns>
        public static Vector2 operator -(Vector2 value)
        {
            return new Vector2(-value.X, -value.Y);
        }

        /// <summary>
        /// Converts a Size to a Vector2.
        /// </summary>
        /// <param name="point">The Size to convert.</param>
        /// <returns>A new Size.</returns>
        public static implicit operator Vector2(Size size)
        {
            return new Vector2(size.Width, size.Height);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted Vector2.</returns>
        public static Vector2 operator -(Vector2 value, Vector2 subtractor)
        {
            return new Vector2(value.X - subtractor.X,
                value.Y - subtractor.Y);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Vector2 operator +(Vector2 value, float amount)
        {
            return new Vector2(value.X + amount, value.Y + amount);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Vector2 operator +(Vector2 value, Vector2 amount)
        {
            return new Vector2(value.X + amount.X, value.Y + amount.Y);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted Vector2.</returns>
        public static Vector2 operator -(Vector2 value, float subtractor)
        {
            return new Vector2(value.X - subtractor, value.Y - subtractor);
        }

        /// <summary>
        /// Divides one Vector2 by another.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Vector2 operator /(Vector2 value, Vector2 divisor)
        {
            return new Vector2(value.X / divisor.X,
                value.Y / divisor.Y);
        }

        /// <summary>
        /// Divides a Vector2 by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Vector2 operator /(Vector2 value, float divisor)
        {
            return new Vector2(value.X / divisor, value.Y / divisor);
        }

        /// <summary>
        /// Divides a Vector2 by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Vector2 operator /(float value, Vector2 divisor)
        {
            return new Vector2(divisor.X / value, divisor.Y / value);
        }

        /// <summary>
        /// Multiplies one Vector2 by another.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector2 operator *(Vector2 value, Vector2 multiplier)
        {
            return new Vector2(value.X * multiplier.X, value.Y * multiplier.Y);
        }

        /// <summary>
        /// Multiplies a Vector2 by a float.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector2 operator *(Vector2 value, float multiplier)
        {
            return new Vector2(value.X * multiplier, value.Y * multiplier);
        }

        /// <summary>
        /// Multiplies a float by a Vector2.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Vector2 operator *(float value, Vector2 multiplier)
        {
            return new Vector2(multiplier.X * value, multiplier.Y * value);
        }

        #endregion
    }
}
