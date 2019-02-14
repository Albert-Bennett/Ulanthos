using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Helpers;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines an object which has a height and a width.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Size : IEquatable<Size>
    {
        #region Feilds

        /// <summary>
        /// The height.
        /// </summary>
        [FieldOffset(0)]
        public float Height;

        /// <summary>
        /// The width.
        /// </summary>
        [FieldOffset(1)]
        public float Width;

        /// <summary>
        /// Creates a new Size.
        /// </summary>
        /// <param name="value">The size.</param>
        public Size(float value) : this(value, value) { }

        /// <summary>
        /// Creates a new Size.
        /// </summary>
        /// <param name="width">The width of the Size.</param>
        /// <param name="height">The height of the Size.</param>
        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new Size.
        /// </summary>
        /// <param name="reader">The BinaryReader to read the Size from.</param>
        public Size(BinaryReader reader)
            : this()
        {
            Load(reader);
        }

        /// <summary>
        /// Creates a new Size.
        /// </summary>
        /// <param name="value">The string value to generate the Size from.</param>
        public Size(string value)
        {
            Size s = GetFromString(value);

            Height = s.Height;
            Width = s.Width;
        }

        #endregion
        #region Defaults

        /// <summary>
        /// Returns a Size of one.
        /// </summary>
        public static readonly Size One = new Size(1);

        /// <summary>
        /// Returns a Size of 0.5.
        /// </summary>
        public static readonly Size Half = new Size(0.5f);

        /// <summary>
        /// Returns a Size of 0.25.
        /// </summary>
        public static readonly Size Quater = new Size(0.25f);

        /// <summary>
        /// Returns a Size of 0.0.3333334.
        /// </summary>
        public static readonly Size Third = new Size(0.3333334f);

        /// <summary>
        /// Returns a Size of 0.
        /// </summary>
        public static readonly Size Zero = new Size(0);

        /// <summary>
        /// Returns a Half the width.
        /// </summary>
        public float HalfWidth { get { return Width / 2; } }

        /// <summary>
        /// Returns a Half the height.
        /// </summary>
        public float HalfHeight { get { return Height / 2; } }

        #endregion
        #region Static Methods

        /// <summary>
        /// Finds the maxamium size of the two values.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns>The maximum size of the Size.</returns>
        public static Size Maximium(Size value1, Size value2)
        {
            return new Size(MathHelper.GreaterThan(value1.Width, value2.Width),
                MathHelper.GreaterThan(value1.Height, value2.Height));
        }

        /// <summary>
        /// Finds the minium size of the two values.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns>The minium size of the Size.</returns>
        public static Size Minimium(Size value1, Size value2)
        {
            return new Size(MathHelper.LessThan(value1.Width, value2.Width),
                MathHelper.LessThan(value1.Height, value2.Height));
        }

        #endregion
        #region Checks

        /// <summary>
        /// A check to see if the size has no values.
        /// </summary>
        public bool IsZero
        {
            get { return Width == 0 || Height == 0; }
        }

        #endregion
        #region Calculations

        /// <summary>
        /// Rounds this Sizes values.
        /// </summary>
        /// <returns>The rounded Size.</returns>
        public Size Round()
        {
            return new Size(MathHelper.Round(Width), MathHelper.Round(Height));
        }

        /// <summary>
        /// Rounds this Sizes values.
        /// </summary>
        public void LocalRound()
        {
            Width = MathHelper.Round(Width);
            Height = MathHelper.Round(Height);
        }

        #endregion
        #region String Related

        /// <summary>
        /// Generates a Size from a string.
        /// </summary>
        /// <param name="value">The string to generate the size from.</param>
        /// <returns>The generated size.</returns>
        public static Size GetFromString(string value)
        {
            return (Size)Vector2.GetFromString(value);
        }

        /// <summary>
        /// Converts the current Size into a loadable format.
        /// </summary>
        /// <returns>The Size as a formatted string.</returns>
        public string ToFormattedString()
        {
            return StringHelper.NumberToString(Width) + ", " +
                StringHelper.NumberToString(Width);
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// A check to see if this Size equals an object to check.
        /// </summary>
        /// <param name="other">The object to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public bool Equals(Size other)
        {
            if (other.Width == Width && other.Height == Height)
                return true;
            else
                return false;
        }

        /// <summary>
        /// A check to see if this Size equals an object to check.
        /// </summary>
        /// <param name="other">The object to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public override bool Equals(object other)
        {
            return (other is Size) ? Equals((Size)other) : base.Equals(other);
        }

        /// <summary>
        /// Saves a Size.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(Width);
            writer.Write(Height);
        }

        /// <summary>
        /// Loads a Size in.
        /// </summary>
        /// <param name="reader">The reader to use.</param>
        public void Load(BinaryReader reader)
        {
            Width = reader.ReadSingle();
            Height = reader.ReadSingle();
        }

        /// <summary>
        /// Converts the Size to a string.
        /// </summary>
        /// <returns>The Size as a string.</returns>
        public override string ToString()
        {
            return "Size {Widht = " + Width.ToString() +
                ", Height = " + Height.ToString() + "}";
        }

        /// <summary>
        /// Gets the hash code for this Size.
        /// </summary>
        /// <returns>This Size's hash code</returns>
        public override int GetHashCode()
        {
            return Width.GetHashCode() + Height.GetHashCode();
        }

        #endregion
        #region Operators

        /// <summary>
        /// Converts a Vector2 to a Size.
        /// </summary>
        /// <param name="vector2">The Vector2 to convert.</param>
        /// <returns>A new Vector2.</returns>
        public static explicit operator Size(Vector2 vector2)
        {
            return new Size(vector2.X, vector2.Y);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted size.</returns>
        public static Size operator -(Size value, Size subtractor)
        {
            return new Size(value.Width - subtractor.Width,
                value.Height - subtractor.Height);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Size operator +(Size value, float amount)
        {
            return new Size(value.Width + amount, value.Height + amount);
        }

        /// <summary>
        /// Adds the amount to value.
        /// </summary>
        /// <param name="value">The value to add from.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The result.</returns>
        public static Size operator +(Size value, Size amount)
        {
            return new Size(value.Width + amount.Width,
                value.Height + amount.Height);
        }

        /// <summary>
        /// Subtracts the value by subtractor.
        /// </summary>
        /// <param name="value">The value to subtract from.</param>
        /// <param name="subtractor">The amount to subtract.</param>
        /// <returns>The subtracted size.</returns>
        public static Size operator -(Size value, float subtractor)
        {
            return new Size(value.Width - subtractor, value.Height - subtractor);
        }

        /// <summary>
        /// Divides a Size by another Size.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the divisor.</returns>
        public static Size operator /(Size value, Size divisor)
        {
            return new Size(value.Width / divisor.Width,
                value.Height / divisor.Height);
        }

        /// <summary>
        /// Divides a Size by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the divisor.</returns>
        public static Size operator /(Size value, float divisor)
        {
            return new Size(value.Width / divisor, value.Height / divisor);
        }

        /// <summary>
        /// Divides a Size by a divisor.
        /// </summary>
        /// <param name="value">The value to divide.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The result of the divisor.</returns>
        public static Size operator /(float value, Size divisor)
        {
            return new Size(divisor.Width / value, divisor.Height / value);
        }

        /// <summary>
        /// Multiplies a Size by another Size.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Size operator *(Size value, Size multiplier)
        {
            return new Size(value.Width * multiplier.Width,
                value.Height * multiplier.Height);
        }

        /// <summary>
        /// Multiplies a Size by a float.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Size operator *(Size value, float multiplier)
        {
            return new Size(value.Width * multiplier, value.Height * multiplier);
        }

        /// <summary>
        /// Multiplies a float by a Size.
        /// </summary>
        /// <param name="value">The value to be multiplied.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Size operator *(float value, Size multiplier)
        {
            return new Size(multiplier.Width * value, multiplier.Height * value);
        }

        #endregion
    }
}
