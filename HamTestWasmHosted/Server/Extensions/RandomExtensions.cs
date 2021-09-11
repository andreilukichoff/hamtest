using System;

namespace HamTestWasmHosted.Server.Extensions
{
    static class RandomExtensions
    {
        /// <summary>
        /// Fischer-Yates-Knuth-Durstenfeld in-place array shuffle algorithm
        /// </summary>
        public static void Shuffle<T>(this T[] array, Random random)
        {
            for (int i = array.Length-1; i >= 0; i--)
            {
                int j = random.Next(i);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}