using System;
using System.Collections.Generic;

namespace BookSmasher.src.controller
{
    // Help with randomness related activities.
    public static class RandomUtil
    {
        // https://stackoverflow.com/a/22668974/10576762
        // Randomize the order of entries in an array.
        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            for (var i = list.Count; i > 0; i--)
                list.Swap(0, rnd.Next(0, i));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
