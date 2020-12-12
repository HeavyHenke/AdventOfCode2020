using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day10
    {
        public int CalcA()
        {
            var lines = File.ReadAllLines("Day10.txt").Select(int.Parse).Concat(new[] {0}).OrderBy(l => l).ToList();

            int num1 = 0;
            int num3 = 1;   // The last to the computer
            for (int i = 1; i < lines.Count(); i++)
            {
                var diff = lines[i] - lines[i - 1];
                if (diff == 1)
                    num1++;
                else if (diff == 3)
                    num3++;
            }

            return num1 * num3; // 2343
        }

        public long CalcB()
        {
            var lines = File.ReadAllLines("Day10.txt").Select(int.Parse).Concat(new[] { 0 }).OrderBy(l => l).ToList();
            lines.Add(lines[^1] + 3);

            var cannotBeRemoved = new List<int> {0};
            for (int i = 1; i < lines.Count - 1; i++)
            {
                if (lines[i + 1] - lines[i - 1] > 3)
                    cannotBeRemoved.Add(i);
            }
            cannotBeRemoved.Add(lines.Count - 1);

            long product = 1;
            for (int i = 1; i < cannotBeRemoved.Count; i++)
            {
                if(cannotBeRemoved[i-1] == cannotBeRemoved[i] + 1)
                    continue;

                var linePart = new int[cannotBeRemoved[i] - cannotBeRemoved[i-1] + 1];
                lines.CopyTo(cannotBeRemoved[i - 1], linePart, 0, linePart.Length);
                product *= NumPermutations(linePart, 1);
            }

            return product;
        }

        private static int NumPermutations(IList<int> lines, int startIx)
        {
            if (startIx == lines.Count - 1)
                return 1;

            int num = 0;
            if (lines[startIx + 1] - lines[startIx - 1] <= 3)
            {
                var lines2 = new List<int>(lines);
                lines2.RemoveAt(startIx);
                num += NumPermutations(lines2, startIx);
            }

            num += NumPermutations(lines, startIx + 1);
            return num;
        }
    }
}