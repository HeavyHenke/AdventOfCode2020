using System.IO;

namespace AdventOfCode2020
{
    internal class Day3
    {
        public int CalcA()
        {
            var map = File.ReadAllLines("Day03.txt");
            var numTrees = NumTrees(map, 3, 1);
            return numTrees;
        }

        private static int NumTrees(string[] map, int dx, int dy)
        {
            int mapWidth = map[0].Length;
            int x = 0, y = 0;
            int numTrees = 0;
            while (y < map.Length)
            {
                x += dx;
                y += dy;
                if (x >= mapWidth)
                    x -= mapWidth;
                if (y < map.Length && map[y][x] == '#')
                    numTrees++;
            }

            return numTrees;
        }

        public int CalcB()
        {
            var map = File.ReadAllLines("Day03.txt");

            return NumTrees(map, 1, 1) *
                   NumTrees(map, 3, 1) *
                   NumTrees(map, 5, 1) *
                   NumTrees(map, 7, 1) *
                   NumTrees(map, 1, 2);
        }
    }
}