using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.controller
{
    public static class ArrayUtil
    {
        public static int FindMode(List<int> x)
        {
            if (x.Count == 0)
            {
                return 0;
            }

            return x.GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
        }
    }
}
