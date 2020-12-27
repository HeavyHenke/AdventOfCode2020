using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            var sw = new Stopwatch();
            sw.Start();
            string result = new Day21().CalcB().ToString();
            sw.Stop();

            Console.WriteLine("It took " + sw.Elapsed);

            WindowsClipboard.SetText(result);
            Console.WriteLine(result);
        }
    }

    class Day20
    {
        public long CalcA()
        {
            var totalSides = new Dictionary<string, int>();
            var tiles = new Dictionary<int, List<string>>();

            var lines = File.ReadAllLines("Day20.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                int tileId = int.Parse(lines[i].Substring(5, lines[i].Length - 5 - 1));
                var left = new char[10];
                var right = new char[10];
                i++;
                var firstRow = lines[i];
                for (int row = 0; row < 10; row++)
                {
                    left[row] = lines[i][0];
                    right[row] = lines[i][9];
                    i++;
                }

                var sides = new List<string>
                {
                    new string(left), new string(right), firstRow, lines[i-1]
                };

                tiles.Add(tileId, sides);
                foreach (var side in sides)
                {
                    if (totalSides.ContainsKey(side))
                        totalSides[side]++;
                    else
                        totalSides.Add(side, 1);
                    var side2 = new string(side.Reverse().ToArray());
                    if (totalSides.ContainsKey(side2))
                        totalSides[side2]++;
                    else
                        totalSides.Add(side2, 1);
                }
            }

            var product = tiles.Select(t => (t.Key, numNonMatchingSides: t.Value.Count(v => totalSides[v] == 1)))
                .Where(t => t.numNonMatchingSides == 2)
                .Select(t => t.Key)
                .Aggregate(1L, (current, tileId) => current * tileId);

            return product;
        }

        public long CalcB()
        {
            var totalSides = new Dictionary<string, List<int>>();
            var tiles = new Dictionary<int, List<string>>();

            var lines = File.ReadAllLines("Day20.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                int tileId = int.Parse(lines[i].Substring(5, lines[i].Length - 5 - 1));
                var left = new char[10];
                var right = new char[10];
                i++;
                var firstRow = lines[i];
                for (int row = 0; row < 10; row++)
                {
                    left[row] = lines[i][0];
                    right[row] = lines[i][9];
                    i++;
                }

                var sides = new List<string>
                {
                    new string(left), new string(right), firstRow, lines[i-1]
                };

                tiles.Add(tileId, sides);
                foreach (var side in sides)
                {
                    if (totalSides.ContainsKey(side))
                        totalSides[side].Add(tileId);
                    else
                        totalSides.Add(side, new List<int> { tileId });
                    var side2 = new string(side.Reverse().ToArray());
                    if (totalSides.ContainsKey(side2))
                        totalSides[side2].Add(tileId);
                    else
                        totalSides.Add(side2, new List<int> { tileId });
                }
            }

            return -1;
        }
    }
}
