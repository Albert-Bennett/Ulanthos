namespace Ulanthos.Math
{
    /// <summary>
    /// A basic helper class for math equations.
    /// </summary>
    public static class MathHelper
    {
        #region General Calculations

        /// <summary>
        /// Returns 1/3.
        /// </summary>
        public static float AThird { get { return 1f / 3f; } }

        /// <summary>
        /// 45 degrees in sin form.
        /// </summary>
        public static float Sin45 { get { return 0.70710678f; } }

        /// <summary>
        /// Pi = 3.1415926535897932384626433832795 or 180 degrees.
        /// </summary>
        public static float Pi { get { return 3.141593f; } }

        /// <summary>
        /// Pi / 2 or 90 degrees.
        /// </summary>
        public static float HalfPi { get { return Pi / 2; } }

        /// <summary>
        /// Pi / 4 or 45 degrees.
        /// </summary>
        public static float QuaterPi { get { return Pi / 4; } }

        /// <summary>
        /// Pi * 2 or 360 degrees.
        /// </summary>
        public static float Pi2 { get { return Pi * 2; } }

        /// <summary>
        /// E as a float.
        /// </summary>
        public static float E { get { return 2.7182824f; } }

        /// <summary>
        /// Used for numerical inprecissions with floats.
        /// </summary>
        public static float Epsilon { get { return 1.192092896e-012f; } }

        #endregion
        #region General

        /// <summary>
        /// Finds the squared root of a number.
        /// </summary>
        /// <param name="number">The number to find the squared root of.</param>
        /// <returns>The squared root of the given number.</returns>
        public static float Sqrt(float number)
        {
            return (float)System.Math.Sqrt(number);
        }

        /// <summary>
        /// Squares a number.
        /// </summary>
        /// <param name="number">The number to be squared.</param>
        /// <returns>number * number.</returns>
        public static float Squared(float number)
        {
            return number * number;
        }

        /// <summary>
        /// Cubes a number.
        /// </summary>
        /// <param name="number">The number to be cubed.</param>
        /// <returns>number * number * number.</returns>
        public static float Cubed(float number)
        {
            return number * number * number;
        }

        /// <summary>
        /// Calculates a power of a number.
        /// </summary>
        /// <param name="number">The nuber to get the power of.</param>
        /// <param name="powerOf">The power of.</param>
        /// <returns>NumberXn. Where n is the given power.</returns>
        public static float PowerOf(float number, int powerOf)
        {
            if (powerOf == 0)
                return 0;
            else if (powerOf == 1)
                return number;
            else
            {
                float n = number;

                for (int i = 1; i <= number; i++)
                    n *= number;

                return n;
            }
        }

        /// <summary>
        /// Finds the smaller of two numbers. 
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The smaller of the two numbers.</returns>
        public static float Min(float value1, float value2)
        {
            if (value1 > value2)
                return value2;
            else
                return value1;
        }

        /// <summary>
        /// Finds the greater of two numbers. 
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The greater of the two numbers.</returns>
        public static float Max(float value1, float value2)
        {
            if (value1 > value2)
                return value1;
            else
                return value2;
        }

        /// <summary>
        /// Finds the absolute value of a number.
        /// </summary>
        /// <param name="value">The value to calculate the absolute of.</param>
        /// <returns>The absolute value of the given number.</returns>
        public static float Abs(float value)
        {
            return value < 0 ? -value : value;
        }

        /// <summary>
        /// Gets the absolute value of a Matrix.
        /// </summary>
        /// <param name="matrix">The Matrix to calculate the absolute value of.</param>
        /// <param name="result">The result of the calculation.</param>
        public static void Abs(ref Matrix matrix, out Matrix result)
        {
            result.M11 = Abs(matrix.M11);
            result.M12 = Abs(matrix.M12);
            result.M13 = Abs(matrix.M13);
            result.M14 = Abs(matrix.M14);

            result.M21 = Abs(matrix.M21);
            result.M22 = Abs(matrix.M22);
            result.M23 = Abs(matrix.M23);
            result.M24 = Abs(matrix.M24);

            result.M31 = Abs(matrix.M31);
            result.M32 = Abs(matrix.M32);
            result.M33 = Abs(matrix.M33);
            result.M34 = Abs(matrix.M34);

            result.M41 = Abs(matrix.M41);
            result.M42 = Abs(matrix.M42);
            result.M43 = Abs(matrix.M43);
            result.M44 = Abs(matrix.M44);
        }

        /// <summary>
        /// Clamps a value between 0.0f and 1.0f.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(float value)
        {
            return Clamp(value, 0, 1);
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimun value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(float value, float min, float max)
        {
            return LessThan(GreaterThan(value, min), max);
        }

        /// <summary>
        /// Clamps a value between 0 and 1.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(int value)
        {
            return Clamp(value, 0, 1);
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimun value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(int value, int min, int max)
        {
            return LessThan(GreaterThan(value, min), max);
        }

        /// <summary>
        /// Rounds up a float to an int.
        /// </summary>
        /// <param name="value">The value to be rounded up.</param>
        /// <returns>The value rounded.</returns>
        public static int Round(float value)
        {
            return (int)(value > 0 ? value + 0.5 : value - 0.5);
        }

        /// <summary>
        /// Preforms a linear interpolation between two floats.
        /// </summary>
        /// <param name="value1">Origional float.</param>
        /// <param name="value2">float to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>Result of the lerp.</returns>
        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Preforms a linear interpolation between two floats.
        /// </summary>
        /// <param name="value1">Origional float.</param>
        /// <param name="value2">float to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <param name="result">Result of the lerp.</param>
        public static void Lerp(ref float value1, ref float value2,
            ref float amount, out float result)
        {
            result = value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Preforms a linear interpolation between two ints.
        /// </summary>
        /// <param name="value1">Origional int.</param>
        /// <param name="value2">int to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>Result of the lerp.</returns>
        public static int Lerp(int value1, int value2, int amount)
        {
            return value1 + Round((value2 - value1) * amount);
        }

        /// <summary>
        /// Preforms a linear interpolation between two ints.
        /// </summary>
        /// <param name="value1">Origional int.</param>
        /// <param name="value2">int to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <param name="result">Result of the lerp.</param>
        public static void Lerp(ref int value1, ref int value2,
            ref int amount, out int result)
        {
            result = value1 + Round((value2 - value1) * amount);
        }

        /// <summary>
        /// Returns the bigger of the two values.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns></returns>
        public static float GreaterThan(float value1, float value2)
        {
            if (value1 > value2)
                return value1;
            else
                return value2;
        }

        /// <summary>
        /// Returns the smaller of the two values.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns></returns>
        public static float LessThan(float value1, float value2)
        {
            if (value1 < value2)
                return value1;
            else
                return value2;
        }

        /// <summary>
        /// Returns the bigger of the two values.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns></returns>
        public static int GreaterThan(int value1, int value2)
        {
            if (value1 > value2)
                return value1;
            else
                return value2;
        }

        /// <summary>
        /// Returns the smaller of the two values.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns></returns>
        public static int LessThan(int value1, int value2)
        {
            if (value1 < value2)
                return value1;
            else
                return value2;
        }

        /// <summary>
        /// A check to see if two floats are nearly equal.
        /// </summary>
        /// <param name="value1">Float 1.</param>
        /// <param name="value2">Float 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool EqualEnough(float value1, float value2)
        {
            float difference = Abs(value1 - value2);
            return (difference <= Epsilon);
        }

        #endregion
        #region Vector3

        /// <summary>
        /// Rotates a position about a given angle (only rotates on the x, z axis).
        /// </summary>
        /// <param name="pos">The position to rotate.</param>
        /// <param name="rotation">The rotation amount in rads</param>
        /// <returns>The rotated position</returns>
        public static Vector3 RotatePosition(Vector3 pos, float rotation)
        {
            Vector2 right = new Vector2((float)Cos(rotation), (float)Sin(rotation));
            Vector2 up = new Vector2((float)Sin(rotation), -(float)Cos(rotation));

            return new Vector3(-right.X * pos.X - up.X * pos.Z, pos.Y, -right.Y * pos.X - up.Y * pos.Z);
        }

        /// <summary>
        /// A check to see if two vectors are equal.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>Result of the check.</returns>
        public static bool IsEqualTo(Vector3 a, Vector3 b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Z == b.Z)
                return true;
            else
                return false;
        }

        /// <summary>
        /// A check to see if one vector is less than or equal to another.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>>Result of the check.</returns>
        public static bool IsLessThanOrEqualTo(Vector3 a, Vector3 b)
        {
            if (a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z)
                return true;
            else
                return IsEqualTo(a, b);
        }

        /// <summary>
        /// A check to see if one vector is greater than or equal to another.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>>Result of the check.</returns>
        public static bool IsGreaterThanOrEqualTo(Vector3 a, Vector3 b)
        {
            if (a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z)
                return true;
            else
                return IsEqualTo(a, b);
        }

        /// <summary>
        /// A check to see if a vector is less than zero.
        /// </summary>
        /// <param name="vec">Vector to check</param>
        /// <returns>Result of the check.</returns>
        public static bool VectorLessThanZero(Vector3 vec)
        {
            if (vec.X < Epsilon || vec.Y < Epsilon || vec.Z < Epsilon)
                return true;
            else
                return false;
        }

        #endregion
        #region Vector2

        /// <summary>
        /// A check to see if two vectors are equal.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>Result of the check.</returns>
        public static bool IsEqualTo(Vector2 a, Vector2 b)
        {
            if (a.X == b.X && a.Y == b.Y)
                return true;
            else
                return false;
        }

        /// <summary>
        /// A check to see if one vector is less than or equal to another.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>>Result of the check.</returns>
        public static bool IsLessThanOrEqualTo(Vector2 a, Vector2 b)
        {
            if (a.X <= b.X && a.Y <= b.Y)
                return true;
            else
                return IsEqualTo(a, b);
        }

        /// <summary>
        /// A check to see if one vector is greater than or equal to another.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>>Result of the check.</returns>
        public static bool IsGreaterThanOrEqualTo(Vector2 a, Vector2 b)
        {
            if (a.X >= b.X && a.Y >= b.Y)
                return true;
            else
                return IsEqualTo(a, b);
        }

        /// <summary>
        /// A check to see if a vector is less than zero.
        /// </summary>
        /// <param name="vec">Vector to check</param>
        /// <returns>Result of the check.</returns>
        public static bool VectorLessThanZero(Vector2 vec)
        {
            if (vec.X < Epsilon || vec.Y < Epsilon)
                return true;
            else
                return false;
        }

        #endregion
        #region Converters

        /// <summary>
        /// Converts Radians into Degrees.
        /// </summary>
        /// <param name="rads">Radians.</param>
        /// <returns>Degrees.</returns>
        public static float ToDegrees(float rads)
        {
            return rads * (180 / Pi);
        }

        /// <summary>
        /// Converts Degrees into Radians
        /// </summary>
        /// <param name="degrees">Degrees.</param>
        /// <returns>Radians.</returns>
        public static float ToRadians(float degrees)
        {
            return degrees * (Pi / 180);
        }

        #endregion
        #region Triginometery
        #region Basic

        /// <summary>
        /// Finds the sin of an angle.
        /// </summary>
        /// <param name="angle">The angle to be calculated in degrees.</param>
        /// <returns>The sin of that angle.</returns>
        public static float Sin(float angle)
        {
            if (angle == 90)
                return 1;
            else if (angle == 30)
                return 0.5f;
            else if (angle == 60)
                return Sqrt(3 / 2);
            else if (angle == 45)
                return Sqrt(2 / 2);

            else if (angle == -90)
                return -1;
            else if (angle == -30)
                return -0.5f;
            else if (angle == -60)
                return -Sqrt(3 / 2);
            else if (angle == -45)
                return -Sqrt(2 / 2);

            else if (angle == 0)
                return 0;
            else
                return 0;
        }

        /// <summary>
        /// Finds the cos of an angle.
        /// </summary>
        /// <param name="angle">The angle to be calculated in degrees.</param>
        /// <returns>The cos of that angle.</returns>
        public static float Cos(float angle)
        {
            return Sqrt(1 - (Squared(Sin(angle)) * angle));
        }

        /// <summary>
        /// Finds the tan of an angle.
        /// </summary>
        /// <param name="angle">The angle to be calculated in degrees.</param>
        /// <returns>The tan of that angle.</returns>
        public static float Tan(float angle)
        {
            return Sin(angle) / Cos(angle);
        }

        /// <summary>
        /// Returns an angle whose tangent is the given angle in degrees.
        /// </summary>
        /// <param name="angle">The angle to be given in degrees.</param>
        /// <returns>The tangent's angle.</returns>
        public static float ATan(float angle)
        {
            return ToDegrees((float)System.Math.Atan(angle));
        }

        /// <summary>
        /// Returns an angle whose tangent is the quotient of the given angles.
        /// </summary>
        /// <param name="x">Angle 1.</param>
        /// <param name="y">Angle 2.</param>
        /// <returns>The angle in degrees.</returns>
        public static float ATan2(float x, float y)
        {
            return ToDegrees((float)System.Math.Atan2(x, y));
        }

        /// <summary>
        /// Returns an angle whose cosine is the given angle in degrees.
        /// </summary>
        /// <param name="angle">The angle to be given in degrees.</param>
        /// <returns>The cosine's angle.</returns>
        public static float ACos(float angle)
        {
            return ToDegrees((float)System.Math.Acos(angle));
        }

        #endregion
        #endregion
    }
}
