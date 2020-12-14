using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day09
    {
        public long CalcA()
        {
            var input = File.ReadAllLines("Day09.txt").Select(long.Parse).ToList();
            const int numPreamble = 25;

            for (int k = numPreamble; k < input.Count; k++)
            {
                if (IsSumInRange(input, k - numPreamble, k - 1, input[k]) == false)
                    return input[k];
            }

            throw new Exception("Not found");
        }

        private bool IsSumInRange(List<long> list, int start, int stop, long sum)
        {
            for (int i = start; i <= stop-1; i++)
            {
                var toFind = sum - list[i];
                if(toFind < 0)
                    continue;
                for(int j = i+1; j <= stop; j++)
                    if (list[j] == toFind)
                        return true;
            }

            return false;
        }

        public long CalcB()
        {
            var input = File.ReadAllLines("Day09.txt").Select(long.Parse).ToList();

            var goal = CalcA();

            for (int i = 0; i < input.Count - 1; i++)
            {
                long sum = input[i];
                for (int j = i + 1; j < input.Count; j++)
                {
                    sum += input[j];
                    if(sum == goal)
                    {
                        var range = input.Skip(i).Take(j - i + 1).ToList();
                        return range.Min() + range.Max();
                    }

                    if (sum > goal)
                        break;
                }
            }

            throw new Exception("Not found.");
        }
    }
}