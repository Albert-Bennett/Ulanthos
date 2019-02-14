using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Helpers;
using Ulanthos.Math;

namespace Ulanthos.Framework.Graphics
{
    /// <summary>
    /// The names of all of the defaulted Colours.
    /// </summary>
    public enum ColourNames
    {
        Black, White, Grey, Transparent,
        Red, Green, Blue,
        DarkRed, DarkGreen, DarkBlue, DarkGrey,
        PaleRed, PaleGreen, PaleBlue,
        HalfTransparentBlack, HalfTransparentWhite,
        HalfTransparentRed, HalfTransparentGreen, HalfTransparentBlue,

        Purple, Orange, Yellow, Pink,
        Brown, Turquise, Cyan,
        DarkPurple, DarkOrange, DarkYellow, DarkPink,
        DarkBrown, DarkTurquoise, DarkCyan,
        PalePurple, PaleOrange, PaleYellow, PalePink,
        PaleBrown, PaleTurquoise, PaleCyan,
        HalfTransparentPurple, HalfTransparentOrange, HalfTransparentYellow, HalfTransparentPink,
        HalfTransparentBrown, HalfTransparentTurquoise, HalfTransparentCyan,

        Gold, Silver, Bronze,
        HalfTransparentGold, HalfTransparentSilver, HalfTransparentBronze,

        Carbon
    }

    /// <summary>
    /// Defines a colour that has a red value, 
    /// green value, blue value and an alpha value.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Colour : IEquatable<Colour>
    {
        #region Properties

        /// <summary>
        /// The size of this object in bytes.
        /// </summary>
        public const int SizeInBytes = 4;

        /// <summary>
        /// This Colours red value.
        /// </summary>
        [FieldOffset(0)]
        public byte R;

        /// <summary>
        /// This Colours green value
        /// </summary>
        [FieldOffset(1)]
        public byte G;

        /// <summary>
        /// This Colours blue value
        /// </summary>
        [FieldOffset(2)]
        public byte B;

        /// <summary>
        /// This Colours alpha value
        /// </summary>
        [FieldOffset(3)]
        public byte A;

        /// <summary>
        /// This Colour as a uint.
        /// </summary>
        public uint PackedRGBA { get { return (uint)GetHashCode(); } }

        #endregion
        #region Constructors

        /// <summary>
        /// Creates a new Colour from red, green, blue and alpha values.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <param name="a">Alpha channel value.</param>
        public Colour(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Creates a new Colour from red, green and blue values.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        public Colour(byte r, byte g, byte b) : this(r, g, b, 255) { }

        /// <summary>
        /// Creates a new Colour from red, green, blue and alpha values.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <param name="a">Alpha channel value.</param>
        public Colour(float r, float g, float b, float a)
        {
            R = (byte)MathHelper.Clamp(r, 0, 255);
            G = (byte)MathHelper.Clamp(g, 0, 255);
            B = (byte)MathHelper.Clamp(b, 0, 255);
            A = (byte)MathHelper.Clamp(a, 0, 255);
        }

        /// <summary>
        /// Creates a new Colour from red, green and blue values.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        public Colour(float r, float g, float b) : this(r, g, b, 255) { }

        /// <summary>
        /// Creates a new Colour.
        /// </summary>
        /// <param name="value">The value to use for all channels.</param>
        public Colour(float value) : this(value, value, value, value) { }

        /// <summary>
        /// Creates a new Colour.
        /// </summary>
        /// <param name="value">The value to use for the colour channels.</param>
        /// <param name="alphaValue">The alpha value for the Colour.</param>
        public Colour(float value, float alphaValue) : this(value, value, value, alphaValue) { }

        /// <summary>
        /// Creates a new Colour.
        /// </summary>
        /// <param name="value">The string value to generate the Colour from.</param>
        public Colour(BinaryReader reader)
            : this()
        {
            Load(reader);
        }

        #endregion
        #region Defaults
        #region Normal

        /// <summary>
        /// Returns the Colour (0, 0, 0, 0).
        /// </summary>
        public static readonly Colour Zero = new Colour(0, 0, 0, 0);

        /// <summary>
        /// Returns the Colour (0, 0, 0, 255).
        /// </summary>
        public static readonly Colour Black = new Colour(0, 0, 0, 255);

        /// <summary>
        /// Returns the Colour (127, 127, 127, 255).
        /// </summary>
        public static readonly Colour Grey = new Colour(127, 127, 127, 255);

        /// <summary>
        /// Returns the Colour (255, 255, 255, 255).
        /// </summary>
        public static readonly Colour White = new Colour(255, 255, 255, 255);

        /// <summary>
        /// Returns the Colour (255, 0, 0, 255).
        /// </summary>
        public static readonly Colour Red = new Colour(255, 0, 0, 255);

        /// <summary>
        /// Returns the Colour (0, 255, 0, 255).
        /// </summary>
        public static readonly Colour Green = new Colour(0, 255, 0, 255);

        /// <summary>
        /// Returns the Colour (0, 0, 255, 255).
        /// </summary>
        public static readonly Colour Blue = new Colour(0, 0, 255, 255);

        /// <summary>
        /// Returns the Colour (127, 0, 255, 255).
        /// </summary>
        public static readonly Colour Purple = new Colour(127, 0, 255, 255);

        /// <summary>
        /// Returns the Colour (255, 0, 255, 255).
        /// </summary>
        public static readonly Colour Pink = new Colour(255, 0, 255, 255);

        /// <summary>
        /// Returns the Colour (255, 127, 0, 255).
        /// </summary>
        public static readonly Colour Orange = new Colour(255, 127, 0, 255);

        /// <summary>
        /// Returns the Colour (255, 255, 0, 255).
        /// </summary>
        public static readonly Colour Yellow = new Colour(255, 255, 0, 255);

        /// <summary>
        /// Returns the Colour (0, 127, 255, 255).
        /// </summary>
        public static readonly Colour Cyan = new Colour(0, 127, 255, 255);

        /// <summary>
        /// Returns the Colour (0, 255, 255, 255).
        /// </summary>
        public static readonly Colour Turquoise = new Colour(0, 255, 255, 255);

        /// <summary>
        /// Returns the Colour (127, 64, 0, 255).
        /// </summary>
        public static readonly Colour Brown = new Colour(127, 64, 0, 255);

        /// <summary>
        /// Returns the Colour (255, 204, 0, 255).
        /// </summary>
        public static readonly Colour Gold = new Colour(255, 204, 0, 255);

        /// <summary>
        /// Returns the Colour (181, 181, 181, 255).
        /// </summary>
        public static readonly Colour Silver = new Colour(181, 181, 181, 255);

        /// <summary>
        /// Returns the Colour (167, 96, 0, 255).
        /// </summary>
        public static readonly Colour Bronze = new Colour(167, 96, 0, 255);

        #endregion
        #region Pale

        /// <summary>
        /// Returns the Colour (255, 56, 56, 255).
        /// </summary>
        public static readonly Colour PaleRed = new Colour(255, 56, 56, 255);

        /// <summary>
        /// Returns the Colour (56, 255, 56, 255).
        /// </summary>
        public static readonly Colour PaleGreen = new Colour(56, 255, 56, 255);

        /// <summary>
        /// Returns the Colour (56, 56, 255, 255).
        /// </summary>
        public static readonly Colour PaleBlue = new Colour(56, 56, 255, 255);

        /// <summary>
        /// Returns the Colour (127, 64, 255, 255).
        /// </summary>
        public static readonly Colour PalePurple = new Colour(127, 64, 255, 255);

        /// <summary>
        /// Returns the Colour (255, 127, 255, 255).
        /// </summary>
        public static readonly Colour PalePink = new Colour(255, 127, 255, 255);

        /// <summary>
        /// Returns the Colour (255, 127, 64, 255).
        /// </summary>
        public static readonly Colour PaleOrange = new Colour(255, 127, 64, 255);

        /// <summary>
        /// Returns the Colour (255, 255, 127, 255).
        /// </summary>
        public static readonly Colour PaleYellow = new Colour(255, 255, 127, 255);

        /// <summary>
        /// Returns the Colour (127, 127, 255, 255).
        /// </summary>
        public static readonly Colour PaleCyan = new Colour(127, 127, 255, 255);

        /// <summary>
        /// Returns the Colour (127, 255, 255, 255).
        /// </summary>
        public static readonly Colour PaleTurquoise = new Colour(127, 255, 255, 255);

        /// <summary>
        /// Returns the Colour (127, 64, 64, 255).
        /// </summary>
        public static readonly Colour PaleBrown = new Colour(127, 64, 64, 255);

        #endregion
        #region Darkened

        /// <summary>
        /// Returns the Colour (56, 56, 56, 255).
        /// </summary>
        public static readonly Colour DarkGrey = new Colour(56, 56, 56, 255);

        /// <summary>
        /// Returns the Colour (127, 0, 0, 255).
        /// </summary>
        public static readonly Colour DarkRed = new Colour(127, 0, 0, 255);

        /// <summary>
        /// Returns the Colour (0, 127, 0, 255).
        /// </summary>
        public static readonly Colour DarkGreen = new Colour(0, 127, 0, 255);

        /// <summary>
        /// Returns the Colour (0, 0, 127, 255).
        /// </summary>
        public static readonly Colour DarkBlue = new Colour(0, 0, 127, 255);

        /// <summary>
        /// Returns the Colour (64, 0, 127, 255).
        /// </summary>
        public static readonly Colour DarkPurple = new Colour(64, 0, 127, 255);

        /// <summary>
        /// Returns the Colour (127, 0, 127, 255).
        /// </summary>
        public static readonly Colour DarkPink = new Colour(127, 0, 127, 255);

        /// <summary>
        /// Returns the Colour (127, 64, 0, 255).
        /// </summary>
        public static readonly Colour DarkOrange = new Colour(127, 64, 0, 255);

        /// <summary>
        /// Returns the Colour (0, 127, 127, 255).
        /// </summary>
        public static readonly Colour DarkYellow = new Colour(127, 127, 0, 255);

        /// <summary>
        /// Returns the Colour (0, 64, 127, 255).
        /// </summary>
        public static readonly Colour DarkCyan = new Colour(0, 64, 127, 255);

        /// <summary>
        /// Returns the Colour (0, 127, 127, 255).
        /// </summary>
        public static readonly Colour DarkTurquoise = new Colour(0, 127, 127, 255);

        /// <summary>
        /// Returns the Colour (64, 32, 0, 255).
        /// </summary>
        public static readonly Colour DarkBrown = new Colour(64, 32, 0, 255);

        #endregion
        #region Half Transparent

        /// <summary>
        /// Returns the Colour (0, 0, 0, 127).
        /// </summary>
        public static readonly Colour HalfTransparentBlack = new Colour(0, 0, 0, 127);

        /// <summary>
        /// Returns the Colour (255, 255, 255, 127).
        /// </summary>
        public static readonly Colour HalfTransparentWhite = new Colour(255, 255, 255, 127);

        /// <summary>
        /// Returns the Colour (255, 0, 0, 127).
        /// </summary>
        public static readonly Colour HalfTransparentRed = new Colour(255, 0, 0, 127);

        /// <summary>
        /// Returns the Colour (0, 255, 0, 127).
        /// </summary>
        public static readonly Colour HalfTransparentGreen = new Colour(0, 255, 0, 127);

        /// <summary>
        /// Returns the Colour (0, 0, 255, 127).
        /// </summary>
        public static readonly Colour HalfTransparentBlue = new Colour(0, 0, 255, 127);

        /// <summary>
        /// Returns the Colour (127, 0, 255, 127).
        /// </summary>
        public static readonly Colour HalfTransparentPurple = new Colour(127, 0, 255, 127);

        /// <summary>
        /// Returns the Colour (255, 0, 255, 127).
        /// </summary>
        public static readonly Colour HalfTransparentPink = new Colour(255, 0, 255, 127);

        /// <summary>
        /// Returns the Colour (255, 127, 0, 127).
        /// </summary>
        public static readonly Colour HalfTransparentOrange = new Colour(255, 127, 0, 127);

        /// <summary>
        /// Returns the Colour (0, 255, 255, 127).
        /// </summary>
        public static readonly Colour HalfTransparentYellow = new Colour(255, 255, 0, 127);

        /// <summary>
        /// Returns the Colour (0, 127, 255, 127).
        /// </summary>
        public static readonly Colour HalfTransparentCyan = new Colour(0, 127, 255, 127);

        /// <summary>
        /// Returns the Colour (0, 255, 255, 127).
        /// </summary>
        public static readonly Colour HalfTransparentTurquoise = new Colour(0, 255, 255, 127);

        /// <summary>
        /// Returns the Colour (127, 64, 0, 127).
        /// </summary>
        public static readonly Colour HalfTransparentBrown = new Colour(127, 64, 0, 127);

        /// <summary>
        /// Returns the Colour (255, 204, 0, 127).
        /// </summary>
        public static readonly Colour HalfTransparentGold = new Colour(255, 204, 0, 127);

        /// <summary>
        /// Returns the Colour (181, 181, 181, 127).
        /// </summary>
        public static readonly Colour HalfTransparentSilver = new Colour(181, 181, 181, 127);

        /// <summary>
        /// Returns the Colour (167, 96, 0, 127).
        /// </summary>
        public static readonly Colour HalfTransparentBronze = new Colour(167, 96, 0, 127);

        #endregion
        #region Other

        /// <summary>
        /// Returns the Colour (87, 87, 87, 255).
        /// </summary>
        public static readonly Colour Carbon = new Colour(87, 87, 87, 255);

        #endregion
        #endregion
        #region Calculations

        /// <summary>
        /// Generates a Colour randomly. 
        /// </summary>
        /// <returns>The generated Colour.</returns>
        public static Colour RandomColour()
        {
            return new Colour(RandomHelper.GetRandomFloat(0, 255),
                RandomHelper.GetRandomFloat(0, 255),
                RandomHelper.GetRandomFloat(0, 255), 1);
        }

        /// <summary>
        /// Generates a Colour randomly.  
        /// </summary>
        /// <param name="min">The minimum Colour channel values.</param>
        /// <param name="max">The maximum Colour channel values.</param>
        /// <returns>The generated Colour.</returns>
        public static Colour RandomColour(Colour min, Colour max)
        {
            return new Colour
            (
                RandomHelper.GetRandomFloat(min.R, max.R),
                RandomHelper.GetRandomFloat(min.G, max.G),
                RandomHelper.GetRandomFloat(min.B, max.B),
                RandomHelper.GetRandomFloat(min.A, max.A)
            );
        }

        /// <summary>
        /// Generates a Colour randomly. 
        /// </summary>
        /// <param name="min">The minimum Colour channel values.</param>
        /// <param name="max">The maximum Colour channel values.</param>
        /// <param name="alpha">The alpha value to use for the colour.</param>
        /// <returns>The generated Colour.</returns>
        public static Colour RandomColour(Vector3 min, Vector3 max, float alpha)
        {
            return new Colour(RandomHelper.GetRandomFloat(min.X, max.X),
                RandomHelper.GetRandomFloat(min.Y, max.Y), RandomHelper.GetRandomFloat(min.Z, max.Z), alpha);
        }

        /// <summary>
        /// Generates a Colour randomly. 
        /// </summary>
        /// <param name="min">The minimum Colour channel values.</param>
        /// <param name="max">The maximum Colour channel values.</param>
        /// <param name="alphaMin">The minimum alpha channel value.</param>
        /// <param name="alphaMax">The maximum alpha channel value.</param>
        /// <returns>The generated Colour.</returns>
        public static Colour RandomColour(Vector3 min, Vector3 max,
            float alphaMin, float alphaMax)
        {
            return RandomColour(min, max,
                RandomHelper.GetRandomFloat(alphaMin, alphaMax));
        }

        /// <summary>
        /// Preforms a linear interpolation between two Colour's.
        /// </summary>
        /// <param name="value1">Origional Colour.</param>
        /// <param name="value2">The Colour to lerp to.</param>
        /// <param name="precentage">The precentage to lerp by.</param>
        /// <returns>The result of the interpolation.</returns>
        public static Colour Lerp(Colour value1, Colour value2, float precentage)
        {
            return new Colour
                (
                    MathHelper.Lerp(value1.R, value2.R, precentage),
                    MathHelper.Lerp(value1.G, value2.G, precentage),
                    MathHelper.Lerp(value1.B, value2.B, precentage),
                    MathHelper.Lerp(value1.A, value2.A, precentage)
                );
        }

        /// <summary>
        /// Preforms a linear interpolation between two Colour's.
        /// </summary>
        /// <param name="value1">Origional Colour.</param>
        /// <param name="value2">The Colour to lerp to.</param>
        /// <param name="precentage">The precentage to lerp by.</param>
        /// <param name="result">The result of the interpolation.</param>
        public static void Lerp(ref Colour value1, ref Colour value2,
            float precentage, out Colour result)
        {

            result.R = (byte)MathHelper.Lerp(value1.R, value2.R, precentage);
            result.G = (byte)MathHelper.Lerp(value1.G, value2.G, precentage);
            result.B = (byte)MathHelper.Lerp(value1.B, value2.B, precentage);
            result.A = (byte)MathHelper.Lerp(value1.A, value2.A, precentage);
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// Saves a Colour.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
            writer.Write(A);
        }

        /// <summary>
        /// Loads a Colour in.
        /// </summary>
        /// <param name="reader">The reader to use.</param>
        public void Load(BinaryReader reader)
        {
            R = (byte)reader.ReadSingle();
            G = (byte)reader.ReadSingle();
            B = (byte)reader.ReadSingle();
            A = (byte)reader.ReadSingle();
        }

        /// <summary>
        /// A check to see if two Colours are equal.
        /// </summary>
        /// <param name="other">The Colour to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public bool Equals(Colour other)
        {
            return this == other;
        }

        /// <summary>
        /// A check to see if this Colour equals an object to check.
        /// </summary>
        /// <param name="other">The object to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Colour) && Equals((Colour)obj);
        }

        /// <summary>
        /// Gets the hash code for this Colour.
        /// </summary>
        /// <returns>This Colour's hash code.</returns>
        public override int GetHashCode()
        {
            return R ^ G ^ B ^ A;
        }

        /// <summary>
        /// Converts the Colour to a string.
        /// </summary>
        /// <returns>The Colour as a string.</returns>
        public override string ToString()
        {
            return "{R = " + R.ToString() + ", G = " + G.ToString() +
                ", B = " + B.ToString() + ", A = " + A.ToString() + "}";
        }

        /// <summary>
        /// Generates a Colour from a given string.
        /// </summary>
        /// <param name="value">The string value to generate the Colour from.</param>
        /// <returns>The generated Colour.</returns>
        public static Colour GetFromString(string value)
        {
            value = value.Replace("(", "");
            value = value.Replace(")", "");
            value = value.Replace("{", "");
            value = value.Replace("}", "");

            string[] values = value.Split(new[]
			{
				',', ' '
			}, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 3)
                return new Colour(Convert.ToByte(values[0]),
                    Convert.ToByte(values[1]), Convert.ToByte(values[2]));
            else if (values.Length == 4)
                return new Colour(Convert.ToByte(values[0]),
                    Convert.ToByte(values[1]), Convert.ToByte(values[2]),
                    Convert.ToByte(values[3]));

            return Zero;
        }

        /// <summary>
        /// Converts a given Colour into a loadable formated string.
        /// </summary>
        /// <param name="value">The Colour to convert.</param>
        /// <returns>The Colour as a formatted string.</returns>
        public static string ToFormattedString(Colour value)
        {
            return (StringHelper.NumberToString(value.R) + ", " +
                StringHelper.NumberToString(value.G) + ", " +
                StringHelper.NumberToString(value.B) + ", " +
                StringHelper.NumberToString(value.A));
        }

        /// <summary>
        /// Converts the current Colour into a loadable format.
        /// </summary>
        /// <returns>This Colour as a formatted string.</returns>
        public string ToFormattedString()
        {
            return ToFormattedString(this);
        }

        #endregion
        #region  Operators

        /// <summary>
        /// A check to see if two Colour's are equal.
        /// </summary>
        /// <param name="value1">Colour 1.</param>
        /// <param name="value2">Colour 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Colour value1, Colour value2)
        {
            return (value1.R == value2.R &&
                value1.G == value2.G && value1.B == value2.B &&
                value1.A == value2.A);
        }

        /// <summary>
        /// A check to see if two Colour's aren't equal.
        /// </summary>
        /// <param name="value1">Colour 1.</param>
        /// <param name="value2">Colour 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Colour value1, Colour value2)
        {
            return (value1.R != value2.R ||
                value1.G != value2.G || value1.B != value2.B ||
                value1.A != value2.A);
        }

        /// <summary>
        /// Converts a Vector3 into a Colour.
        /// </summary>
        /// <param name="value">The Vectro3 to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Colour(Vector3 value)
        {
            return new Colour(value.X, value.Y, value.Z, 255);
        }

        /// <summary>
        /// Multiplies the given Colour by a scale amount.
        /// </summary>
        /// <param name="value">The Colour to multiply.</param>
        /// <param name="scale">The amount to multiply by.</param>
        /// <returns>The multiplied Colour.</returns>
        public static Colour operator *(Colour value, float scale)
        {
            return new Colour(
                MathHelper.Clamp(value.R * scale, 0, 255),
                MathHelper.Clamp(value.G * scale, 0, 255),
                MathHelper.Clamp(value.B * scale, 0, 255),
                MathHelper.Clamp(value.A * scale, 0, 255)
                );
        }

        /// <summary>
        /// Multiplies the given Colour by a scale amount.
        /// </summary>
        /// <param name="value">The Colour to multiply.</param>
        /// <param name="scale">The amount to multiply by.</param>
        /// <returns>The multiplied Colour.</returns>
        public static Colour operator *(Colour value, Colour scale)
        {
            return new Colour(
                MathHelper.Clamp(value.R * scale.R, 0, 255),
                MathHelper.Clamp(value.G * scale.G, 0, 255),
                MathHelper.Clamp(value.B * scale.B, 0, 255),
                MathHelper.Clamp(value.A * scale.A, 0, 255)
                );
        }

        /// <summary>
        /// Divides the given Colour by a scale amount.
        /// </summary>
        /// <param name="value">The Colour to divide.</param>
        /// <param name="scale">The amount to divide by.</param>
        /// <returns>The divided Colour.</returns>
        public static Colour operator /(Colour value, float scale)
        {
            return new Colour(
                MathHelper.Clamp(value.R / scale, 0, 255),
                MathHelper.Clamp(value.G / scale, 0, 255),
                MathHelper.Clamp(value.B / scale, 0, 255),
                MathHelper.Clamp(value.A / scale, 0, 255)
                );
        }

        /// <summary>
        /// Divides the given Colour by a scale amount.
        /// </summary>
        /// <param name="value">The Colour to divide.</param>
        /// <param name="scale">The amount to divide by.</param>
        /// <returns>The divided Colour.</returns>
        public static Colour operator /(Colour value, Colour scale)
        {
            return new Colour(
                MathHelper.Clamp(value.R / scale.R, 0, 255),
                MathHelper.Clamp(value.G / scale.G, 0, 255),
                MathHelper.Clamp(value.B / scale.B, 0, 255),
                MathHelper.Clamp(value.A / scale.A, 0, 255)
                );
        }

        /// <summary>
        /// Subtracts the given Colour by an amount.
        /// </summary>
        /// <param name="value">The Colour to subtract.</param>
        /// <param name="scale">The amount to be subtracted.</param>
        /// <returns>The modified Colour.</returns>
        public static Colour operator -(Colour value, float amount)
        {
            return new Colour(
                MathHelper.Clamp(value.R - amount, 0, 255),
                MathHelper.Clamp(value.G - amount, 0, 255),
                MathHelper.Clamp(value.B - amount, 0, 255),
                MathHelper.Clamp(value.A - amount, 0, 255)
                );
        }

        /// <summary>
        /// Subtracts the given Colour by an amount.
        /// </summary>
        /// <param name="value">The Colour to subtract.</param>
        /// <param name="scale">The amount to be subtracted.</param>
        /// <returns>The modified Colour.</returns>
        public static Colour operator -(Colour value, Colour amount)
        {
            return new Colour(
                MathHelper.Clamp(value.R - amount.R, 0, 255),
                MathHelper.Clamp(value.G - amount.G, 0, 255),
                MathHelper.Clamp(value.B - amount.B, 0, 255),
                MathHelper.Clamp(value.A - amount.A, 0, 255)
                );
        }

        /// <summary>
        /// Adds the given Colour by an amount.
        /// </summary>
        /// <param name="value">The Colour to add.</param>
        /// <param name="scale">The amount to be added.</param>
        /// <returns>The modified Colour.</returns>
        public static Colour operator +(Colour value, float amount)
        {
            return new Colour(
                MathHelper.Clamp(value.R + amount, 0, 255),
                MathHelper.Clamp(value.G + amount, 0, 255),
                MathHelper.Clamp(value.B + amount, 0, 255),
                MathHelper.Clamp(value.A + amount, 0, 255)
                );
        }

        /// <summary>
        /// Adds the given Colour by an amount.
        /// </summary>
        /// <param name="value">The Colour to add.</param>
        /// <param name="scale">The amount to be added.</param>
        /// <returns>The modified Colour.</returns>
        public static Colour operator +(Colour value, Colour amount)
        {
            return new Colour(
                MathHelper.Clamp(value.R + amount.R, 0, 255),
                MathHelper.Clamp(value.G + amount.G, 0, 255),
                MathHelper.Clamp(value.B + amount.B, 0, 255),
                MathHelper.Clamp(value.A + amount.A, 0, 255)
                );
        }

        #endregion
    }
}
