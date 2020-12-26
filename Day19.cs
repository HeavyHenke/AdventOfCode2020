using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    class Day19
    {
        public long CalcA()
        {
            var lines = File.ReadAllLines("Day19.txt");

            int rowNo = 0;
            var rules = new Dictionary<int, string>();
            while (lines[rowNo] != "")
            {
                var split = lines[rowNo].Split(":");
                rules[int.Parse(split[0])] = split[1].Trim();
                rowNo++;
            }


            rowNo++;
            int numValid = 0;
            while (rowNo < lines.Length)
            {
                int ix = 0;
                if (IsValid(lines[rowNo], ref ix, rules[0], rules) && ix == lines[rowNo].Length)
                    numValid++;
                rowNo++;
            }

            return numValid;
        }

        private bool IsValid(string str, ref int strIx, string rule, Dictionary<int, string> rules)
        {
            if (rule.Contains('|'))
            {
                var split = rule.Split('|');
                int strIxCopy = strIx;
                if (IsValid(str, ref strIxCopy, split[0].Trim(), rules))
                {
                    strIx = strIxCopy;
                    return true;
                }

                if (IsValid(str, ref strIx, split[1].Trim(), rules))
                    return true;

                return false;
            }

            if (rule.Contains("\""))
            {
                if (strIx >= str.Length)
                    return false; 
                var chr = rule[1];
                return str[strIx++] == chr;
            }

            var ruleParts = rule.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in ruleParts)
            {
                if (IsValid(str, ref strIx, rules[int.Parse(part)], rules) == false)
                {
                    return false;
                }
            }

            return true;
        }


        public long CalcB()
        {
            var lines = File.ReadAllLines("Day19.txt");

            int rowNo = 0;
            var rules = new Dictionary<int, string>();
            while (lines[rowNo] != "")
            {
                var split = lines[rowNo].Split(":");
                rules[int.Parse(split[0])] = split[1].Trim();
                rowNo++;
            }

            rules[8] = "42 | 42 8";
            rules[11] = "42 31 | 42 11 31";

            var ruleList = rules.ToList();
            for (int i = 0; i < ruleList.Count; i++)
            {
                if(ruleList[i].Key == 0 || ruleList[i].Key == 8 || ruleList[i].Key == 42 || ruleList[i].Key == 11 || ruleList[i].Key == 31)
                    continue;
                InlineRule(ruleList[i].Key, rules);
            }



            rowNo++;
            int numValid = 0;
            string r42 = rules[42].Replace("\"", "").Replace(" ", "").Replace("(a)", "a").Replace("(b)", "b");
            string r31 = rules[31].Replace("\"", "").Replace(" ", "").Replace("(a)", "a").Replace("(b)", "b");
            string regexPart = "^(" + r42 + ")+" + "(" + r42 + "){QQQ}" + "(" + r31 + "){QQQ}$";
            const int numTries = 10;
            Regex[] regexes = new Regex[numTries];
            for (int i = 1; i < numTries+1; i++)
            {
                regexes[i-1] = new Regex(regexPart.Replace("QQQ", i.ToString()), RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            }

            while (rowNo < lines.Length)
            {
                if(regexes.Any(r => r.IsMatch(lines[rowNo])))
                {
                    Console.WriteLine("Matched : " + lines[rowNo]);
                    numValid++;
                }
                rowNo++;
            }

            return numValid;
        }

        private static void InlineRule(int ruleNo, Dictionary<int, string> rules)
        {
            var rule = " " + rules[ruleNo] + " ";
            rules.Remove(ruleNo);
            var regex = new Regex($@"(\D|^){ruleNo}(\D|$)");
            foreach (var key in rules.Keys.ToList())
            {
                var r = rules[key];
                var m = regex.Match(r);

                while (m.Success)
                {
                    int ix = m.Index, length = m.Length;

                    r = r.Substring(0, ix) + " ( " + rule + " ) " + r.Substring(ix + length);
                    r = r.Replace("  ", " ").Replace("\" \"", "").Trim();
                    rules[key] = r;

                    m = regex.Match(r);
                }
            }
        }
    }
}