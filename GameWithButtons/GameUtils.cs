using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWithButtons
{
    static class GameUtils
    {
        public static Tuple<int, int> CoordinatesOf<T>(T[,] matrix, T value)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }

        public static bool IsShapeSizeCorrect(Tuple<int, int> firstIndexes, Tuple<int, int> secondIndexes, Tuple<int, int> size)
        {
            return (Math.Abs(firstIndexes.Item1 - secondIndexes.Item1) + 1 == size.Item1
                            && Math.Abs(firstIndexes.Item2 - secondIndexes.Item2) + 1 == size.Item2)
                   ||
                   (Math.Abs(firstIndexes.Item1 - secondIndexes.Item1) + 1 == size.Item2
                            && Math.Abs(firstIndexes.Item2 - secondIndexes.Item2) + 1 == size.Item1);
        }
    }
}
