using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day11
    {
        public int CalcA()
        {
            var lines = File.ReadAllLines("Day11.txt").Select(s => s.ToCharArray()).ToArray();
            var copy = (char[][])lines.Clone();
            for (int i = 0; i < lines.Length; i++)
                copy[i] = (char[])lines[i].Clone();

            bool hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;

                for (int y = 0; y < lines.Length; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        if (lines[y][x] == '.') continue;
                        var adjacent = CountBusy(lines, y, x);
                        if (lines[y][x] == 'L' && adjacent == 0)
                        {
                            copy[y][x] = '#';
                            hasChanged = true;
                        }
                        else if (lines[y][x] == '#' && adjacent >= 4)
                        {
                            copy[y][x] = 'L';
                            hasChanged = true;
                        }
                    }
                }

                lines = copy;
                copy = (char[][])lines.Clone();
                for (int i = 0; i < lines.Length; i++)
                    copy[i] = (char[])lines[i].Clone();
            }

            var occupied = lines.SelectMany(l => l).Count(c => c == '#');
            return occupied;
        }

        private static int CountBusy(char[][] seats, int y, int x)
        {
            int numBusy = 0;
            foreach (var pos in GetAllAdjacent(y, x, seats.Length - 1, seats[y].Length - 1))
            {
                if (seats[pos.y][pos.x] == '#')
                    numBusy++;
            }

            return numBusy;
        }

        private static IEnumerable<(int y, int x)> GetAllAdjacent(int y, int x, int maxY, int maxX)
        {
            if (y > 0)
            {
                yield return (y - 1, x);
                if (x > 0)
                    yield return (y - 1, x - 1);
                if (x < maxX)
                    yield return (y - 1, x + 1);
            }
            if (x > 0)
                yield return (y, x - 1);
            if (x < maxX)
                yield return (y, x + 1);

            if (y < maxY)
            {
                yield return (y + 1, x);
                if (x > 0)
                    yield return (y + 1, x - 1);
                if (x < maxX)
                    yield return (y + 1, x + 1);
            }
        }

        public int CalcB()
        {
            var lines = File.ReadAllLines("Day11.txt").Select(s => s.ToCharArray()).ToArray();
            var copy = (char[][])lines.Clone();
            for (int i = 0; i < lines.Length; i++)
                copy[i] = (char[])lines[i].Clone();

            bool hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;

                for (int y = 0; y < lines.Length; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        if (lines[y][x] == '.')
                            continue;
                        var adjacent = CountBusy2(lines, y, x);
                        if (lines[y][x] == 'L' && adjacent == 0)
                        {
                            copy[y][x] = '#';
                            hasChanged = true;
                        }
                        else if (lines[y][x] == '#' && adjacent >= 5)
                        {
                            copy[y][x] = 'L';
                            hasChanged = true;
                        }
                    }
                }

                lines = copy;
                copy = (char[][])lines.Clone();
                for (int i = 0; i < lines.Length; i++)
                    copy[i] = (char[])lines[i].Clone();
            }

            var occupied = lines.SelectMany(l => l).Count(c => c == '#');
            return occupied;
        }

        private static int CountBusy2(char[][] seats, int y, int x)
        {
            int numBusy = 0;
            for(int dy = -1; dy <= 1; dy++)
                for(int dx = -1;dx <=1; dx++)
                    if(dx != 0 || dy != 0)
                        numBusy += NextSeatInDir(seats, y, x, dy, dx) == '#' ? 1 : 0;

            return numBusy;
        }

        private static char NextSeatInDir(char[][] seats, int y, int x, int dy, int dx)
        {
            while (true)
            {
                x += dx;
                y += dy;
                if (y < 0 || y >= seats.Length || x < 0 || x >= seats[y].Length)
                    return '.';
                if (seats[y][x] != '.')
                    return seats[y][x];
            }
        }
    }
}