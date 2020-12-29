using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day24
    {
        public int CalcA()
        {
            var blackTiles = new HashSet<(int q, int r)>();    // Axial coordinate system

            foreach (var line in File.ReadAllLines("Day24.txt"))
            {
                (int q, int r) pos = (0, 0);

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'e')
                    {
                        pos.q--;
                    }
                    else if (line[i] == 'w')
                    {
                        pos.q++;
                    }
                    else if (line[i] == 's')
                    {
                        pos.r++;
                        if (line[i + 1] == 'e') 
                            pos.q--;
                        i++;
                    }
                    else if (line[i] == 'n')
                    {
                        pos.r--;
                        if (line[i + 1] == 'w')
                            pos.q++;
                        i++;
                    }
                    else
                        throw new Exception("Knas");
                }

                if (blackTiles.Add(pos) == false)
                    blackTiles.Remove(pos);
            }

            return blackTiles.Count;
        }

        public int CalcB()
        {
            var blackTiles = new HashSet<(int q, int r)>();    // Axial coordinate system

            foreach (var line in File.ReadAllLines("Day24.txt"))
            {
                (int q, int r) pos = (0, 0);

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'e')
                    {
                        pos.q--;
                    }
                    else if (line[i] == 'w')
                    {
                        pos.q++;
                    }
                    else if (line[i] == 's')
                    {
                        pos.r++;
                        if (line[i + 1] == 'e')
                            pos.q--;
                        i++;
                    }
                    else if (line[i] == 'n')
                    {
                        pos.r--;
                        if (line[i + 1] == 'w')
                            pos.q++;
                        i++;
                    }
                    else
                        throw new Exception("Knas");
                }

                if (blackTiles.Add(pos) == false)
                    blackTiles.Remove(pos);
            }

            for (int day = 0; day < 100; day++)
            {
                var nextDay = new HashSet<(int q, int r)>();
                foreach (var blackTile in blackTiles)
                {
                    var neighbors = GetAdjacent(blackTile).Count(t => blackTiles.Contains(t));
                    if (neighbors == 1 || neighbors == 2)
                        nextDay.Add(blackTile);
                }

                var whitesToVisit = blackTiles.SelectMany(GetAdjacent).Where(t => blackTiles.Contains(t) == false).Distinct();
                foreach (var tile in whitesToVisit)
                {
                    var neighbors = GetAdjacent(tile).Count(t => blackTiles.Contains(t));
                    if (neighbors == 2)
                        nextDay.Add(tile);
                }

                blackTiles = nextDay;
            }

            return blackTiles.Count;
        }

        private static IEnumerable<(int q, int r)> GetAdjacent((int q, int r) pos)
        {
            yield return (pos.q - 1, pos.r);
            yield return (pos.q + 1, pos.r);

            yield return (pos.q - 1, pos.r + 1);
            yield return (pos.q + 1, pos.r - 1);

            yield return (pos.q, pos.r + 1);
            yield return (pos.q, pos.r - 1);
        }
    }
}