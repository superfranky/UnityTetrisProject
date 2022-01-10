using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class MyExtensionMethods
    {
        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)
        {
            var w = matrix.GetLength(0); // width
            var h = matrix.GetLength(1); // height

            for (var x = 0; x < w; ++x)
            {
                for (var y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }

        public static Vector2Int VectorOf<T>(this T[,] matrix, T value)
        {
            var w = matrix.GetLength(0); // width
            var h = matrix.GetLength(1); // height

            for (var x = 0; x < w; ++x)
            {
                for (var y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return new Vector2Int(x, y);
                }
            }

            return new Vector2Int(-1, -1);
        }

        public static List<(int, int)> AllCoordinatesOf<T>(this T[,] matrix, T value)
        {
            var w = matrix.GetLength(0); // width
            var h = matrix.GetLength(1); // height

            var list = new List<(int, int)>();

            for (var x = 0; x < w; ++x)
            {
                for (var y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        list.Add((x, y));
                }
            }

            return list;
        }

    }
}
