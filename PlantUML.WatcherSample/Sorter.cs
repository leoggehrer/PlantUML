namespace PlantUML.WatcherSample
{
    public class Sorter
    {
        /// <summary>
        /// Sorts an array of integers using the brute force sorting algorithm.
        /// </summary>
        /// <param name="array">The array to be sorted.</param>
        public static void SortWithBruteForceSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[i] < array[j])
                    {
                        int tmp = array[i];

                        array[i] = array[j];
                        array[j] = tmp;
                    }
                }
            }
        }
    }
}