using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Math.Bounding;
using Ulanthos.Helpers;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines a rectangle which has x co-ordinates, 
    /// y co-ordinates, height and width.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Rectangle : IEquatable<Rectangle>
    {
        #region Variables

        /// <summary>
        /// The X co-ordinate.
        /// </summary>
        [FieldOffset(0)]
        public float X;

        /// <summary>
        /// The Y co-ordinate.
        /// </summary>
        [FieldOffset(1)]
        public float Y;

        /// <summary>
        /// The height.
        /// </summary>
        [FieldOffset(2)]
        public float Height;

        /// <summary>
        /// The width.
        /// </summary>
        [FieldOffset(3)]
        public float Width;

        /// <summary>
        /// The Rectangles position.
        /// </summary>
        public Vector2 Position { get { return new Vector2(X, Y); } }

        /// <summary>
        /// The Rectangles Size.
        /// </summary>
        public Size Size { get { return new Size(Width, Height); } }

        /// <summary>
        /// Returns true if height and width = 0.
        /// </summary>
        public bool IsZero { get { return Size.IsZero; } }

        #endregion
        #region  Constructors

        /// <summary>
        /// Creates a new Rectangle.
        /// </summary>
        /// <param name="position">The position of the Rectangle.</param>
        /// <param name="size">The size of the Rectangle.</param>
        public Rectangle(Vector2 position, Size size) :
            this(position.X, position.Y, size.Width, size.Height) { }

        /// <summary>
        /// Creates a new Rectangle.
        /// </summary>
        /// <param name="position">The position of the Rectangle.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Rectangle(Vector2 position, float width, float height) : this(position.X, position.Y, width, height) { }

        /// <summary>
        /// Creates a new Rectangle.
        /// </summary>
        /// <param name="x">X co-ordinate.</param>
        /// <param name="y">Y co-ordinate.</param>
        /// <param name="value">Width, Height value.</param>
        public Rectangle(float x, float y, float value) : this(x, y, value, value) { }

        /// <summary>
        /// Creates a new Rectangle.
        /// </summary>
        /// <param name="x">X co-ordinate.</param>
        /// <param name="y">Y co-ordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;

            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new Rectangle.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public Rectangle(BinaryReader reader)
            : this()
        {
            Load(reader);
        }

        #endregion
        #region Points

        /// <summary>
        /// Bottom edge of the Rectangle.
        /// </summary>
        public float Bottom
        {
            get { return Y + Height; }
            set { Y = value - Height; }
        }

        /// <summary>
        /// Right edge of the Rectangle.
        /// </summary>
        public float Right
        {
            get { return X + Width; }
            set { X = value - Width; }
        }

        /// <summary>
        /// Top left point on the Rectangle.
        /// </summary>
        public Vector2 TopLeft { get { return new Vector2(X, Y); } }

        /// <summary>
        /// Top right point on the Rectangle.
        /// </summary>
        public Vector2 TopRight { get { return new Vector2(Right, Y); } }

        /// <summary>
        /// Bottom left point on the Rectangle.
        /// </summary>
        public Vector2 BottomLeft { get { return new Vector2(X, Bottom); } }

        /// <summary>
        /// Bottom right point on the Rectangle.
        /// </summary>
        public Vector2 BottomRight { get { return new Vector2(Right, Bottom); } }

        /// <summary>
        /// Center of the Rectangle.
        /// </summary>
        public Vector2 Center
        {
            get { return new Vector2(X + (Width / 2), Y + (Height / 2)); }
        }

        #endregion
        #region Defaults

        /// <summary>
        /// A Rectangle of size zero.
        /// </summary>
        public static readonly Rectangle Zero = new Rectangle(0, 0, 0, 0);

        /// <summary>
        /// A Rectangle of size one.
        /// </summary>
        public static readonly Rectangle One = new Rectangle(0, 0, 1, 1);

        #endregion
        #region Calculations

        /// <summary>
        /// Returns a copied version of the Rectangle translated.
        /// </summary>
        /// <param name="amount">The amount to translate by.</param>
        /// <returns>Translated Rectangle.</returns>
        public Rectangle Translate(Vector2 amount)
        {
            return new Rectangle(X + amount.X, Y + amount.Y, Width, Height);
        }

        /// <summary>
        /// Returns a copied version of the Rectangle translated.
        /// </summary>
        /// <param name="amount">The amount to translate by.</param>
        /// <returns>Translated Rectangle.</returns>
        public Rectangle Translate(float amount)
        {
            return new Rectangle(X + amount, Y + amount, Width, Height);
        }

        /// <summary>
        /// Returns a copied version of the Rectangle translated.
        /// </summary>
        /// <param name="xAmount">The amount to translate by in the x axis.</param>
        /// <param name="yAmount">The amount to translate by in the y axis.</param>
        /// <returns>Translated Rectangle.</returns>
        public Rectangle Translate(float xAmount, float yAmount)
        {
            return new Rectangle(X + xAmount, Y + yAmount, Width, Height);
        }

        /// <summary>
        /// Moves the Rectangle.
        /// </summary>
        /// <param name="amount">The amount to move by.</param>
        public void MoveThis(float amount)
        {
            X += amount;
            Y += amount;
        }

        /// <summary>
        /// Moves the Rectangle.
        /// </summary>
        /// <param name="amount">The amount to move by.</param>
        public void MoveThis(Vector2 amount)
        {
            X += amount.X;
            Y += amount.Y;
        }

        /// <summary>
        /// Moves the Rectangle.
        /// </summary>
        /// <param name="xAmount">The amount to move by in the x axis.</param>
        /// <param name="yAmount">The amount to move by in the y axis.</param>
        public void MoveThis(float xAmount, float yAmount)
        {
            X += xAmount;
            Y += yAmount;
        }

        /// <summary>
        /// Returns the copied Rectangle with a new position.
        /// </summary>
        /// <param name="position">New position of the Rectangle.</param>
        /// <returns>The coied Rectangle with a new position.</returns>
        public Rectangle SetPos(Vector2 position)
        {
            return new Rectangle(position, Width, Height);
        }

        /// <summary>
        /// Returns the copied Rectangle with a new position.
        /// </summary>
        /// <param name="x">The new position's x co-ordinate.</param>
        /// <param name="y">The new position's y co-ordinate.</param>
        /// <returns>The coied Rectangle with a new position.</returns>
        public Rectangle SetPos(float x, float y)
        {
            return new Rectangle(x, y, Width, Height);
        }

        /// <summary>
        /// Sets the position of the Rectangle.
        /// </summary>
        /// <param name="position">The new position of the Rectangle.</param>
        public void SetPosThis(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
        }

        /// <summary>
        /// Sets the position of the Rectangle.
        /// </summary>
        /// <param name="x">The new position's x co-ordinate.</param>
        /// <param name="y">The new position's y co-ordinate.</param>
        public void SetPosThis(float x, float y)
        {
            X = x;
            Y = y;
        }


        /// <summary>
        /// Multiplies this Rectangle by another.
        /// </summary>
        /// <param name="relative">The Rectangle to multiply this Rectangle by.</param>
        /// <returns>Current Rectangle multiplied by the relative Rectangle.</returns>
        public Rectangle FindInnerRectangle(Rectangle relative)
        {
            return new Rectangle(X + Width * relative.X,
                Y + Height * relative.Y, Width * relative.Width,
                Height * relative.Height);
        }

        #endregion
        #region Re-Sizing

        /// <summary>
        /// Shrinks the Rectangle by an amount.
        /// </summary>
        /// <param name="amount">The amount to shrink by.</param>
        /// <returns>Shrunken Rectangle.</returns>
        public Rectangle Shrink(float amount)
        {
            return new Rectangle(X + amount, Y + amount,
                Width - (amount * 2), Height - (amount * 2));
        }

        /// <summary>
        /// Shrinks the Rectangle by an amount.
        /// </summary>
        /// <param name="amount">The amount to shrink by.</param>
        public void ShrinkThis(float amount)
        {
            X += amount;
            Y += amount;
            Width -= (amount * 2);
            Height -= (amount * 2);
        }

        /// <summary>
        /// Scales up the Rectangle by a given factor.
        /// </summary>
        /// <param name="factor">The factor to increase the scale by.</param>
        /// <returns>The copied Rectangle scaled up.</returns>
        public Rectangle Scale(float factor)
        {
            return new Rectangle(X - factor, Y - factor,
                Width + (factor * 2), Height + (factor * 2));
        }

        /// <summary>
        /// Scales up the Rectangle by a given factor.
        /// </summary>
        /// <param name="factor">The factor to increase the scale by.</param>
        public void ScaleThis(float factor)
        {
            X += factor;
            Y += factor;
            Width -= (factor * 2);
            Height -= (factor * 2);
        }

        /// <summary>
        /// Scales up a Rectangle by it's center Point.
        /// </summary>
        /// <param name="widthFactor">The factor to increase the width by.</param>
        /// <param name="heightFactor">The factor to increase the height by.</param>
        /// <returns>The scaled up Rectangle.</returns>
        public Rectangle CenterScale(float widthFactor, float heightFactor)
        {
            Size original = Size;
            Size scaled = new Size(original.Width * widthFactor, original.Height * heightFactor);
            Size offset = (original - scaled) / 2;

            return new Rectangle(Position + offset, scaled);
        }

        /// <summary>
        /// Scales up a Rectangle by it's center Point.
        /// </summary>
        /// <param name="factor">The factor to increase the scale by.</param>
        /// <returns>The scaled up Rectangle.</returns>
        public Rectangle CenterScale(float factor)
        {
            return CenterScale(factor, factor);
        }

        #endregion
        #region String Related

        /// <summary>
        /// Converts this Rectangle into a formatted string safe to be read in.
        /// </summary>
        /// <returns>Rectangle as a formatted string.</returns>
        public string GetFromString()
        {
            return (StringHelper.NumberToString(X) + "," +
                StringHelper.NumberToString(Y) + "," +
                StringHelper.NumberToString(Width) + "," +
                StringHelper.NumberToString(Height));
        }

        /// <summary>
        /// Creates a Rectangle from a string that is in the correct format i.e n,n,n,n.
        /// </summary>
        /// <param name="value">The string to use.</param>
        /// <returns>A new Rectangle created from a string.</returns>
        public static Rectangle GetFromString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] split = value.Split(new[] { ',' });

                if (split.Length == 4)
                {
                    CultureInfo culture = CultureInfo.InvariantCulture;

                    return new Rectangle(Convert.ToSingle(split[0], culture),
                        Convert.ToSingle(split[1], culture), Convert.ToSingle(split[2], culture),
                        Convert.ToSingle(split[3], culture));
                }
                else
                    throw new ArgumentOutOfRangeException(string.Format
                        ("Unable to convert to Rectangle invalid format: {0} ", value));
            }
            else
                return Zero;
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// A check to see if an object is equal to this Rectangle.
        /// </summary>
        /// <param name="obj">The object to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Rectangle) ? Equals((Rectangle)obj) : base.Equals(obj);
        }

        /// <summary>
        /// A check to see if the other Rectangle is equal to this one.
        /// </summary>
        /// <param name="other">The Rectangele to check aginst.</param>
        /// <returns>The result of the check.</returns>
        public bool Equals(Rectangle other)
        {
            return (other.X >= (X - MathHelper.Epsilon) &&
                other.X <= (X + MathHelper.Epsilon) &&
                other.Y >= (Y - MathHelper.Epsilon) &&
                other.Y <= (Y + MathHelper.Epsilon) &&
                other.Width >= (Width - MathHelper.Epsilon) &&
                other.Width <= (Width + MathHelper.Epsilon) &&
                other.Height >= (Height - MathHelper.Epsilon) &&
                other.Height <= (Height + MathHelper.Epsilon));
        }

        /// <summary>
        /// Saves the Rectangle.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Width);
            writer.Write(Height);
        }

        /// <summary>
        /// Loads in a Rectangle.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Width = reader.ReadSingle();
            Height = reader.ReadSingle();
        }

        /// <summary>
        /// Gets the hash code of the Rectangle's.
        /// </summary>
        /// <returns>The Rectangle's has code.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^
                Width.GetHashCode() ^ Height.GetHashCode();
        }

        /// <summary>
        /// Converts the Rectangle into a string.
        /// </summary>
        /// <returns>The string representation of the Rectangle.</returns>
        public override string ToString()
        {
            return "Rectangle {x = " + X + ", y = " + Y +
                ", width = " + Width + ", height = " + Height + "}";
        }

        #endregion
        #region Other

        /// <summary>
        /// A check to see if the Rectangle contains the given Point. 
        /// </summary>
        /// <param name="point">The Point to check.</param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            return point.X >= X && point.X <= Right
                && point.Y >= Y && point.Y < Bottom;
        }

        /// <summary>
        /// A check to see how much are the two Rectangles intersecting.
        /// </summary>
        /// <param name="check">The rectangle to check aginst.</param>
        /// <returns>The type of overlap that has occured.</returns>
        public ContainableTypes Contains(Rectangle check)
        {
            if (Right < check.X || X > check.Right ||
                Bottom < check.Y || Y > check.Bottom)
                return ContainableTypes.None;

            else if (X <= check.X && Right >= check.Right &&
                Y < check.Y && Bottom >= check.Bottom)
                return ContainableTypes.Fully;

            return ContainableTypes.Partial;
        }

        /// <summary>
        /// Creates a Rectangle from given corner Points.
        /// </summary>
        /// <param name="topLeft">Top left co-ordinate.</param>
        /// <param name="bottomRight">Bottom right co-ordinate.</param>
        /// <returns></returns>
        public static Rectangle CreateFromCorners(Vector2 topLeft, Vector2 bottomRight)
        {
            return new Rectangle(topLeft.X, topLeft.Y,
                bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }

        /// <summary>
        /// Creates a Rectangle from a given center point.
        /// </summary>
        /// <param name="x">Central X co-ordinate.</param>
        /// <param name="y">Central Y co-ordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <returns></returns>
        public static Rectangle CreateFromCenter(float x, float y,
            float width, float height)
        {
            return new Rectangle(x - (width / 2),
                y - (height / 2), width, height);
        }

        /// <summary>
        /// Creates a Rectangle from a given center Point.
        /// </summary>
        /// <param name="center">The center of the Rectangle.</param>
        /// <param name="value">Width, Height.</param>
        /// <returns></returns>
        public static Rectangle CreateFromCenter(Vector2 center, float value)
        {
            return CreateFromCenter(center.X, center.Y, value, value);
        }

        /// <summary>
        /// Creates a Rectangle from a given center Point.
        /// </summary>
        /// <param name="center">The center of the Rectangle.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <returns></returns>
        public static Rectangle CreateFromCenter(Vector2 center,
            float width, float height)
        {
            return CreateFromCenter(center.X, center.Y, width, height);
        }

        /// <summary>
        /// Creates a Rectangle from a given center Point.
        /// </summary>
        /// <param name="center">The center of the Rectangle.</param>
        /// <param name="dimentions">Width, Height.</param>
        /// <returns></returns>
        public static Rectangle CreateFromCenter(Vector2 center, Size dimentions)
        {
            return CreateFromCenter(center.X, center.Y, dimentions.Width, dimentions.Height);
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two rectangles are equal.
        /// </summary>
        /// <param name="value1">Rectangle 1.</param>
        /// <param name="value2">Rectangle 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Rectangle value1, Rectangle value2)
        {
            return (MathHelper.Abs(value1.X - value2.X) <= MathHelper.Epsilon &&
                 MathHelper.Abs(value1.Y - value2.Y) <= MathHelper.Epsilon &&
                  MathHelper.Abs(value1.Width - value2.Width) <= MathHelper.Epsilon &&
                   MathHelper.Abs(value1.Height - value2.Height) <= MathHelper.Epsilon);
        }

        /// <summary>
        /// A check to see if two rectangles aren't equal.
        /// </summary>
        /// <param name="value1">Rectangle 1.</param>
        /// <param name="value2">Rectangle 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Rectangle value1, Rectangle value2)
        {
            return (MathHelper.Abs(value1.X - value2.X) > MathHelper.Epsilon &&
                 MathHelper.Abs(value1.Y - value2.Y) > MathHelper.Epsilon &&
                  MathHelper.Abs(value1.Width - value2.Width) > MathHelper.Epsilon &&
                   MathHelper.Abs(value1.Height - value2.Height) > MathHelper.Epsilon);
        }

        #endregion
    }
}
