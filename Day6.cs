using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020
{
    internal class Day6
    {
        public int CalcA()
        {
            var lines = File.ReadAllLines("Day06.txt");
            int sum = 0;
            var hash = new HashSet<char>();

            foreach (var line in lines)
            {
                if (line == "")
                {
                    sum += hash.Count;
                    hash.Clear();
                }

                foreach (var s in line)
                    hash.Add(s);
            }

            sum += hash.Count;

            return sum;
        }

        public int CalcB()
        {
            var lines = File.ReadAllLines("Day06.txt");
            int sum = 0;
            var groupHash = new HashSet<char>();

            bool first = true;
            foreach (var line in lines)
            {
                if (line == "")
                {
                    sum += groupHash.Count;
                    groupHash.Clear();
                    first = true;
                    continue;
                }

                if (first)
                {
                    foreach (var s in line)
                        groupHash.Add(s);
                    first = false;
                }
                else
                    groupHash.IntersectWith(line);
            }

            sum += groupHash.Count;

            return sum;
        }
    }
}