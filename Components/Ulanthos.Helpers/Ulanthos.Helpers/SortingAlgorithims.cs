namespace Ulanthos.Helpers
{
    /// <summary>
    /// Define numerous ways for sorting data.
    /// </summary>
    public class SortingAlgorithims
    {
        static int[] a;
        static int n;

        /// <summary>
        /// This Mathod sorts out an array of type int
        /// </summary>
        /// <param name="a0">Array to be sorted</param>
        public static void HeapSort(int[] a0)
        {
            a = a0;
            n = a.Length;

            Sort();
        }

        static void Sort()
        {
            BuildHeap();

            while (n > 1)
            {
                n--;
                Swap(0, n);
                GoDown(0);
            }
        }

        static void BuildHeap()
        {
            for (int i = n / 2 - 1; i >= 0; i--)
                GoDown(i);
        }

        static void GoDown(int i)
        {
            int j = 2 * i + 1;

            while (j < n)
            {
                if (j + 1 < n)
                    if (a[j + 1] > a[j])
                        if (a[i] >= a[j])
                            return;

                Swap(i, j);
                i = j;
                j = 2 * i + 1;
            }
        }

        private static void Swap(int i, int j)
        {
            int k = a[i];
            a[i] = a[j];
            a[j] = k;
        }

        /// <summary>
        /// Sorts an array of type int really quick
        /// </summary>
        /// <param name="array">Array to be sorted</param>
        public static void ShellSort(int[] array)
        {
            a = array;
            int i, j, k, l, m;
            int p = a.Length;

            for (k = 0; k < a.Length; k++)
            {
                l = a[k];

                for (i = l; i < p; i++)
                {
                    m = a[i];
                    j = i;
                    while (j >= l && a[j - l] > m)
                    {
                        a[j] = a[j - l];
                        j = j - l;
                    }
                    a[j] = m;
                }
            }
        }
    }
}
