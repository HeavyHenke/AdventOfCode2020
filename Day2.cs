using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    internal class Day2
    {
        public int CalcA()
        {
            var parseRegEx = new Regex(@"(?<min>\d+)-(?<max>\d+) (?<char>\w): (?<password>\w*)", RegexOptions.Compiled);

            var lines = File.ReadAllLines("Day02.txt");
            int numValid = 0;
            foreach(var line in lines)
            {
                var match = parseRegEx.Match(line);
                if (match.Success == false)
                    throw new Exception("Could not parse line: " + line);

                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                char chr = match.Groups["char"].Value[0];
                string password = match.Groups["password"].Value;

                var numChr = password.Count(c => c == chr);

                if (numChr >= min && numChr <= max)
                    numValid++;
            }

            return numValid;
        }

        public int CalcB()
        {
            var parseRegEx = new Regex(@"(?<min>\d+)-(?<max>\d+) (?<char>\w): (?<password>\w*)", RegexOptions.Compiled);

            var lines = File.ReadAllLines("Day02.txt");
            int numValid = 0;
            foreach (var line in lines)
            {
                var match = parseRegEx.Match(line);
                if (match.Success == false)
                    throw new Exception("Could not parse line: " + line);

                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                char chr = match.Groups["char"].Value[0];
                string password = match.Groups["password"].Value;

                bool valid1 = min >= 1 && min <= password.Length && password[min - 1] == chr;
                bool valid2 = max >= 1 && max <= password.Length && password[max - 1] == chr;
                if (valid1 ^ valid2)
                    numValid++;
            }

            return numValid;
        }
    }
}