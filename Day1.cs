using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day1
    {
        public string CalcA()
        {
            var values = File.ReadAllLines("Day01.txt")
                .Select(int.Parse)
                .ToHashSet();

            foreach(var val in values)
            {
                var toSeek = 2020 - val;
                if (values.Contains(toSeek))
                    return (val * toSeek).ToString();
            }

            throw new Exception("Nothing found!");
        }

        public string CalcB()
        {
            var values = File.ReadAllLines("Day01.txt")
                .Select(int.Parse)
                .ToList();
            var valuesHash = values.ToHashSet();

            for(int i = 0; i < values.Count; i++)
            {
                var left = 2020 - values[i];
                for(int j = i+1; j < values.Count; j++)
                {
                    var toSeek = left - values[j];
                    if (valuesHash.Contains(toSeek))
                        return (values[i] * values[j] * toSeek).ToString();
                }
            }

            foreach (var val in values)
            {
                var toSeek = 2020 - val;
                if (values.Contains(toSeek))
                    return (val * toSeek).ToString();
            }

            throw new Exception("Nothing found!");
        }
    }
}
