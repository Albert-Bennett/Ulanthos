using System;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Helpers;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines a Quaternion.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        #region Properties

        public const int SizeInBytes = 4 * 4;

        /// <summary>
        /// X value.
        /// </summary>
        [FieldOffset(0)]
        public float X;

        /// <summary>
        /// Y value.
        /// </summary>
        [FieldOffset(2)]
        public float Y;

        /// <summary>
        /// Z value.
        /// </summary>
        [FieldOffset(3)]
        public float Z;

        /// <summary>
        /// W value.
        /// </summary>
        [FieldOffset(4)]
        public float W;

        /// <summary>
        /// The squared lenght of the Quaternion.
        /// </summary>
        public float LenghtSquared
        {
            get { return X * X + Y * Y + Z * Z + W * W; }
        }

        /// <summary>
        /// The lenght of the Quaternion.
        /// </summary>
        public float Lenght
        {
            get { return MathHelper.Sqrt(LenghtSquared); }
        }

        /// <summary>
        /// The X, Y, Z components as a Vector3.
        /// </summary>
        public Vector3 Vector3
        {
            get { return new Vector3(X, Y, Z); }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Creates a new Quaternion.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        /// <param name="w">W value.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Creates a new Quaternion.
        /// </summary>
        /// <param name="value">The value to be used for all of the Quaternion's components.</param>
        public Quaternion(float value) : this(value, value, value, value) { }

        /// <summary>
        /// Creates a new Quaternion.
        /// </summary>
        /// <param name="vector">The x, y, z components as a Vector3.</param>
        /// <param name="w">W component.</param>
        public Quaternion(Vector3 vector, float w)
            : this(vector.X, vector.Y, vector.Z, w) { }

        /// <summary>
        /// Creates a new Quaternion.
        /// </summary>
        /// <param name="value">The string to get the values from.</param>
        public Quaternion(string value)
            : this()
        {
            Quaternion quat = GetFromString(value);
            X = quat.X;
            Y = quat.Y;
            Z = quat.Z;
            W = quat.W;
        }

        /// <summary>
        /// Creates a new Quaternion.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public Quaternion(BinaryReader reader)
            : this()
        {
            Load(reader);
        }

        #endregion
        #region Defaults

        /// <summary>
        /// Returns the identity Quaternion.
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        #endregion
        #region Calculations

        /// <summary>
        /// Concatenate's a Quaternion.
        /// </summary>
        /// <param name="value1">Quaternion 1.</param>
        /// <param name="value2">Quaternion 2.</param>
        /// <returns>Concatenated Quaternion.</returns>
        public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
        {
            float x = (value2.Y * value1.Z) - (value2.Z * value1.Y);
            float y = (value2.Z * value1.X) - (value2.X * value1.Z);
            float z = (value2.X * value1.Y) - (value2.Y * value1.X);
            float w = (value2.X * value1.X) + (value2.Y * value1.Y) + (value2.Z * value1.Z);

            return new Quaternion
            (
                (value2.X * value1.W) + (value1.X * value2.W) + x,
                (value2.Y * value1.W) + (value1.Y * value2.W) + y,
                (value2.Z * value1.W) + (value1.Z * value2.W) + z,
                (value2.W * value1.W) - w
            );
        }

        /// <summary>
        /// Conjugates this Quaternion.
        /// </summary>
        public void Conjugate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Finds the dot product of two Quaternion's.
        /// </summary>
        /// <param name="value1">Quaternion 1.</param>
        /// <param name="value2">Quaternion 2.</param>
        /// <returns>The dot product of the two Quaternion's.</returns>
        public static float Dot(Quaternion value1, Quaternion value2)
        {
            return ((value1.X * value2.X) + (value1.Y * value2.Y) +
                (value1.Z * value2.Z) + (value1.W * value2.W));
        }

        /// <summary>
        /// Finds the dot product of two Quaternion's.
        /// </summary>
        /// <param name="value1">Quaternion 1.</param>
        /// <param name="value2">Quaternion 2.</param>
        /// <param name="result">The dot product of the two Quaternion's.</param>
        public static void Dot(ref Quaternion value1,
            ref Quaternion value2, out float result)
        {
            result = ((value1.X * value2.X) + (value1.Y * value2.Y) +
               (value1.Z * value2.Z) + (value1.W * value2.W));
        }

        /// <summary>
        /// Inverts this Quaternion.
        /// </summary>
        public void Invert()
        {
            if (LenghtSquared > float.Epsilon)
            {
                float inverse = -1 / LenghtSquared;
                X *= inverse;
                Y *= inverse;
                Z *= inverse;
                W *= -inverse;
            }
        }

        /// <summary>
        /// Inverts the Quaternion.
        /// </summary>
        /// <param name="value">The Quaternion to invert.</param>
        /// <returns>The inverted Quaternion.</returns>
        public static Quaternion Invert(Quaternion value)
        {
            if (value.LenghtSquared > float.Epsilon)
            {
                float inverse = -1 / value.LenghtSquared;
                value.X *= inverse;
                value.Y *= inverse;
                value.Z *= inverse;
                value.W *= -inverse;
            }

            return Quaternion.Identity;
        }

        /// <summary>
        /// Preforms a linear interpolation between two Quaternion's.
        /// </summary>
        /// <param name="original">Origional Quaternion.</param>
        /// <param name="toLerpTo">Quaternion to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>Result of the lerp.</returns>
        public static Quaternion Lerp(Quaternion value1,
            Quaternion value2, float amount)
        {
            float cos = Dot(value1, value2);
            float sin = MathHelper.Sqrt(MathHelper.Abs(1 - cos * cos));

            if (MathHelper.Abs(sin) > float.Epsilon)
            {
                float angle = MathHelper.ATan2(sin, cos);
                float iSin = 1 / sin;
                float cos0 = MathHelper.Sin((1 - amount) * angle) * iSin;
                float cos1 = MathHelper.Sin(amount * angle) * iSin;

                return (value1 * cos0) + (value2 * cos1);
            }

            return value1;
        }

        /// <summary>
        /// Preforms a linear interpolation between two Quaternion's.
        /// </summary>
        /// <param name="original">Origional Quaternion.</param>
        /// <param name="toLerpTo">Quaternion to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <param name="result">Result of the lerp.</param>
        public static void Lerp(ref Quaternion value1, ref Quaternion value2,
            ref float amount, out Quaternion result)
        {
            float cos = Dot(value1, value2);
            float sin = MathHelper.Sqrt(MathHelper.Abs(1 - cos * cos));

            if (MathHelper.Abs(sin) > float.Epsilon)
            {
                float angle = MathHelper.ATan2(sin, cos);
                float iSin = 1 / sin;
                float cos0 = MathHelper.Sin((1 - amount) * angle) * iSin;
                float cos1 = MathHelper.Sin(amount * angle) * iSin;

                result = ((value1 * cos0) + (value2 * cos1));
            }
            else
                result = Identity;
        }

        /// <summary>
        /// Normalizes this Quaternion.
        /// </summary>
        public void Normalize()
        {
            if (LenghtSquared != 0)
            {
                float invertLenght = 1 / Lenght;

                X *= invertLenght;
                Y *= invertLenght;
                Z *= invertLenght;
                W *= invertLenght;
            }
        }

        /// <summary>
        /// Normalizes the Quaternion.
        /// </summary>
        /// <param name="value">The Quaternion to normalize.</param>
        /// <returns>The normalized Quaternion.</returns>
        public static Quaternion Normalize(Quaternion value)
        {
            if (value.LenghtSquared != 0)
            {
                float dist = 1 / value.Lenght;

                return new Quaternion(value.X * dist, value.Y * dist,
                    value.Z * dist, value.W * dist);
            }

            return value;
        }

        #endregion
        #region Creation

        /// <summary>
        /// Creates a new Quaternion from a given axis and angle.
        /// </summary>
        /// <param name="axis">The axis to use.</param>
        /// <param name="angle">The angle to use.</param>
        /// <returns>A Quaternion created from an axis with a rotation.</returns>
        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            return new Quaternion(axis * MathHelper.Sin(angle / 2), MathHelper.Cos(angle / 2));
        }

        /// <summary>
        /// Creates a new Quaternion from a rotational Matrix.
        /// </summary>
        /// <param name="matrix">The Matrix to use.</param>
        /// <returns>A Quaternion created from a rotational Matrix.</returns>
        public static Quaternion CreateFromRotationMatrix(Matrix matrix)
        {
            float num8 = matrix.M11 + matrix.M22 + matrix.M33;

            Quaternion quaternion = new Quaternion();

            if (num8 > 0f)
            {
                float num = MathHelper.Sqrt(num8 + 1f);
                quaternion.W = num * 0.5f;
                num = 0.5f / num;
                quaternion.X = (matrix.M23 - matrix.M32) * num;
                quaternion.Y = (matrix.M31 - matrix.M13) * num;
                quaternion.Z = (matrix.M12 - matrix.M21) * num;
                return quaternion;
            }

            if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                float num7 = MathHelper.Sqrt(1f + matrix.M11 - matrix.M22 - matrix.M33);
                float num4 = 0.5f / num7;
                quaternion.X = 0.5f * num7;
                quaternion.Y = (matrix.M12 + matrix.M21) * num4;
                quaternion.Z = (matrix.M13 + matrix.M31) * num4;
                quaternion.W = (matrix.M23 - matrix.M32) * num4;
                return quaternion;
            }

            if (matrix.M22 > matrix.M33)
            {
                float num6 = MathHelper.Sqrt(1f + matrix.M22 - matrix.M11 - matrix.M33);
                float num3 = 0.5f / num6;
                quaternion.X = (matrix.M21 + matrix.M12) * num3;
                quaternion.Y = 0.5f * num6;
                quaternion.Z = (matrix.M32 + matrix.M23) * num3;
                quaternion.W = (matrix.M31 - matrix.M13) * num3;
                return quaternion;
            }

            float num5 = MathHelper.Sqrt(1f + matrix.M33 - matrix.M11 - matrix.M22);
            float num2 = 0.5f / num5;

            quaternion.X = (matrix.M31 + matrix.M13) * num2;
            quaternion.Y = (matrix.M32 + matrix.M23) * num2;
            quaternion.Z = 0.5f * num5;
            quaternion.W = (matrix.M12 - matrix.M21) * num2;

            return quaternion;
        }

        /// <summary>
        /// Creates a new Quaternion from Yaw, Pitch, Roll values.
        /// </summary>
        /// <param name="yaw">Amount of rotation on the Y axis.</param>
        /// <param name="pitch">Amount of rotation on the X axis.</param>
        /// <param name="roll">Amount of rotation on the Z axis.</param>
        /// <returns></returns>
        public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            yaw /= 2;
            pitch /= 2;
            roll /= 2;

            float yawSin = MathHelper.Sin(yaw);
            float yawCos = MathHelper.Cos(yaw);
            float pitchSin = MathHelper.Sin(pitch);
            float pitchCos = MathHelper.Cos(pitch);
            float rollSin = MathHelper.Sin(roll);
            float rollCos = MathHelper.Cos(roll);

            return new Quaternion
            (
                ((yawCos * pitchSin) * rollCos) + ((yawSin * pitchCos) * rollSin),
                ((yawCos * pitchCos) * rollSin) - ((yawSin * pitchSin) * rollCos),
                ((yawSin * pitchCos) * rollCos) - ((yawCos * pitchSin) * rollSin),
                ((yawCos * pitchCos) * rollCos) + ((yawSin * pitchSin) * rollSin)
            );
        }

        #endregion
        #region String Related

        /// <summary>
        /// Converts the current Quaternion into a loadable format.
        /// </summary>
        /// <returns>The Quaternion as a formatted string.</returns>
        public string ToFormattedString(Quaternion value)
        {
            return (StringHelper.NumberToString(X) + ", " +
               StringHelper.NumberToString(Y) + ", " +
               StringHelper.NumberToString(Z) + ", " +
               StringHelper.NumberToString(W));
        }

        /// <summary>
        /// Generates a new Quaternion from a string.
        /// </summary>
        /// <param name="value">The string to be used.</param>
        /// <returns>The generated Quaternion.</returns>
        public Quaternion GetFromString(string value)
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

            if (values.Length == 4)
                return new Quaternion(Convert.ToInt32(values[0]),
                    Convert.ToInt32(values[1]), Convert.ToInt32(values[2]),
                    Convert.ToInt32(values[3]));

            return Identity;
        }

        #endregion
        #region Base Methods

        public override bool Equals(object obj)
        {
            return (obj is Quaternion) && Equals((Quaternion)obj);
        }

        public bool Equals(Quaternion other)
        {
            return this == other;
        }

        /// <summary>
        /// Saves this Quaternion.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(W);
        }

        /// <summary>
        /// Loads in a Quaternion.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            W = reader.ReadSingle();
        }

        /// <summary>
        /// Gets the hash code for this Quaternion.
        /// </summary>
        /// <returns>The Quaternion's hash code.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^
                Z.GetHashCode() ^ W.GetHashCode();
        }

        /// <summary>
        /// Returns this Quaternion as a string.
        /// </summary>
        /// <returns>Quaternion as a string.</returns>
        public override string ToString()
        {
            return ToString('(', ')');
        }

        /// <summary>
        /// Converts this Quaternion into a string.
        /// </summary>
        /// <param name="openBrace">The char to use to signify a opening brace.</param>
        /// <param name="closedBrace">The char to use to signify a closing brace.</param>
        /// <returns>The Quaternion as a string.</returns>
        public string ToString(char openBrace, char closeBrace)
        {
            string format = "0.0000";
            string comma = ", ";

            return (openBrace + StringHelper.NumberToString(X, format) +
                comma + StringHelper.NumberToString(Y, format) + comma +
                StringHelper.NumberToString(Z, format) + closeBrace);
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two Quaternion's are equal.
        /// </summary>
        /// <param name="value1">Quaternion 1.</param>
        /// <param name="value2">Quaternion 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Quaternion value1, Quaternion value2)
        {
            return value1.X == value2.X &&
                value1.Y == value2.Y && value1.Z == value2.Z &&
                value1.W == value2.W;
        }

        /// <summary>
        /// A check to see if two Quaternion aren't equal.
        /// </summary>
        /// <param name="value1">Quaternion 1.</param>
        /// <param name="value2">Quaternion 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Quaternion value1, Quaternion value2)
        {
            return value1.X != value2.X ||
                value1.Y != value2.Y || value1.Z != value2.Z ||
                value1.W != value2.W;
        }

        /// <summary>
        /// Inverts a Quaternion.
        /// </summary>
        /// <param name="value">The value to invert.</param>
        /// <returns>The inverted Quaternion.</returns>
        public static Quaternion operator -(Quaternion value)
        {
            return new Quaternion(-value.X, -value.Y, -value.Z, -value.W);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted Quaternion.</returns>
        public static Quaternion operator -(Quaternion value, Quaternion subtractor)
        {
            return new Quaternion(value.X - subtractor.X, value.Y - subtractor.Y,
                value.Z - subtractor.Z, value.W - subtractor.W);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Quaternion operator +(Quaternion value, float amount)
        {
            return new Quaternion(value.X + amount, value.Y + amount,
                value.Z + amount, value.W + amount);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Quaternion operator +(Quaternion value, Quaternion amount)
        {
            return new Quaternion(value.X + amount.X, value.Y + amount.Y,
                value.Z + amount.Z, value.W + amount.W);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted Quaternion.</returns>
        public static Quaternion operator -(Quaternion value, float subtractor)
        {
            return new Quaternion(value.X - subtractor, value.Y - subtractor,
                value.Z - subtractor, value.W - subtractor);
        }

        /// <summary>
        /// Divides one Quaternion by another.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Quaternion operator /(Quaternion value, Quaternion divisor)
        {
            return new Quaternion(value.X / divisor.X, value.Y / divisor.Y,
                value.Z / divisor.Z, value.W / divisor.W);
        }

        /// <summary>
        /// Divides a Quaternion by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Quaternion operator /(Quaternion value, float divisor)
        {
            return new Quaternion(value.X / divisor, value.Y / divisor,
                value.Z / divisor, value.W / divisor);
        }

        /// <summary>
        /// Divides a Quaternion by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static Quaternion operator /(float value, Quaternion divisor)
        {
            return new Quaternion(divisor.X / value, divisor.Y / value, divisor.Z / value, divisor.W / value);
        }

        /// <summary>
        /// Multiplies one Quaternion by another.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Quaternion operator *(Quaternion value, Quaternion multiplier)
        {
            return new Quaternion(value.X * multiplier.X, value.Y * multiplier.Y,
                value.Z * multiplier.Z, value.W * multiplier.W);
        }

        /// <summary>
        /// Multiplies a Quaternion by a float.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Quaternion operator *(Quaternion value, float multiplier)
        {
            return new Quaternion(value.X * multiplier, value.Y * multiplier,
                value.Z * multiplier, value.W * multiplier);
        }

        /// <summary>
        /// Multiplies a float by a Quaternion.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Quaternion operator *(float value, Quaternion multiplier)
        {
            return new Quaternion(multiplier.X * value, multiplier.Y * value,
                multiplier.Z * value, multiplier.W * value);
        }

        #endregion
    }
}
