using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    internal class Day04
    {
        public int CalcA()
        {
            var lines = File.ReadAllLines("Day04.txt");

            int numValid = 0;
            foreach (var pass in ParsePassports(lines))
            {
                pass.Remove("cid");
                if (pass.Count == 7)
                    numValid++;
            }

            return numValid;
        }

        public int CalcB()
        {
            var lines = File.ReadAllLines("Day04.txt");

            bool ValidateField(Dictionary<string, string> dict, string key, int numDigits, int min, int max)
            {
                if (dict.TryGetValue(key, out var val) == false || val.Length != numDigits)
                    return false;

                var intVal = int.Parse(val);
                return intVal >= min && intVal <= max;
            }

            var validEcl = new HashSet<string>{"amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            int numValid = 0;
            foreach (var pass in ParsePassports(lines))
            {
                pass.Remove("cid");

                if(ValidateField(pass, "byr", 4, 1920, 2002) == false)
                    continue;
                if (ValidateField(pass, "iyr", 4, 2010, 2020) == false)
                    continue;
                if (ValidateField(pass, "eyr", 4, 2020, 2030) == false)
                    continue;

                if (pass.TryGetValue("hgt", out var hgt) == false || (hgt.EndsWith("cm") == false && hgt.EndsWith("in") == false))
                    continue;
                var hgtVal = int.Parse(new string(hgt.TakeWhile(char.IsNumber).ToArray()));
                if(hgt.EndsWith("cm") && (hgtVal < 150 | hgtVal > 193))
                    continue;
                if (hgt.EndsWith("in") && (hgtVal < 59| hgtVal > 76))
                    continue;

                if(pass.TryGetValue("hcl", out var hcl) == false || Regex.IsMatch(hcl, "#[0-9a-f]{6}") == false)
                    continue;

                if (pass.TryGetValue("ecl", out var ecl) == false || validEcl.Contains(ecl) == false)
                    continue;

                if (pass.TryGetValue("pid", out var pid) == false || Regex.IsMatch(pid, @"^\d{9}$") == false)
                    continue;


                if (pass.Count == 7)
                    numValid++;
            }

            return numValid;
        }

        private static IEnumerable<Dictionary<string, string>> ParsePassports(IEnumerable<string> input)
        {
            var hash = new Dictionary<string, string>();
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    yield return hash;
                    hash = new Dictionary<string, string>();
                }

                var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var k in splits)
                {
                    var parts = k.Split(":");
                    hash.Add(parts[0], parts[1]);
                }
            }

            if (hash.Count > 0)
                yield return hash;
        }
    }
}