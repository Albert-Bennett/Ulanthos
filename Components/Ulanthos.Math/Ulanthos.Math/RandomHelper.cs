namespace Ulanthos.Math
{
    /// <summary>
    /// A helper for random calculations.
    /// </summary>
    public static class RandomHelper
    {
        static System.Random rand = new System.Random();

        ///<summary>
        ///gets a random float from a given set of floats
        ///</summary>
        public static float GetRandomFloat(float min, float max)
        {
            return (((float)rand.NextDouble() * (max - min)) + min);
        }

        ///<summary>
        ///Get a random Vector3 from a given set of Vector3's
        ///</summary>
        public static Vector3 GetRandomVector3(Vector3 min, Vector3 max)
        {
            return (new Vector3(
                GetRandomFloat(min.X, max.X),
                GetRandomFloat(min.Y, max.Y),
                GetRandomFloat(min.Z, max.Z)));
        }

        /// <summary>
        /// Returns a random int.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>A random int.</returns>
        public static int GetRandomInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        /// <summary>
        /// Find a random double.
        /// </summary>
        /// <returns>Returns a random double between 0.0 and 1.0.</returns>
        public static double NextRandomDouble()
        {
            return rand.NextDouble();
        }

        /// <summary>
        /// Randomizes a value with a variation.
        /// </summary>
        /// <param name="val">The value to randomize.</param>
        /// <param name="variation">The variation of the number between generations.</param>
        /// <returns>A randomized number.</returns>
        public static float Randomize(float val, float variation)
        {
            return val + (float)(rand.NextDouble() * 2 - 1) * variation;
        }

        /// <summary>
        /// Gets a random number between two numbers.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>A number between the min and max values given.</returns>
        public static float RandomBetween(float min, float max)
        {
            return min + (float)rand.NextDouble() * (min - max);
        }
    }
}
