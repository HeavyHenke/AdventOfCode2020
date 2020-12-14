using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    internal class Day07
    {
        public int CalcA()
        {
            var bagToContent = ParseInput();

            var search = new Queue<string>();
            search.Enqueue("shiny gold");
            var visited = new HashSet<string>();

            while (search.Count > 0)
            {
                var n = search.Dequeue();
                if (visited.Add(n) == false)
                    continue;

                foreach (var kvp in bagToContent)
                {
                    if (kvp.Value.ContainsKey(n) && visited.Contains(kvp.Key) == false)
                        search.Enqueue(kvp.Key);
                }
            }

            return visited.Count - 1;
        }

        public int CalcB()
        {
            var bagToContent = ParseInput();

            int GetBags(string bag)
            {
                var content = bagToContent[bag];
                int num = 1;
                foreach (var subBag in content)
                    num += subBag.Value * GetBags(subBag.Key);
                return num;
            }

            return GetBags("shiny gold") - 1;   // shiny gold does not contain itself
        }

        private Dictionary<string, Dictionary<string, int>> ParseInput()
        {
            var rulesText = File.ReadAllLines("Day07.txt");
            var bagToContent = new Dictionary<string, Dictionary<string, int>>();

            var firstRegex = new Regex("(.*) bags");
            var bagRegex = new Regex(@"(\d|no) (\D+) bags?");
            foreach (var rule in rulesText)
            {
                var firstAndRest = rule.Split(" contain ");
                var bag = firstRegex.Match(firstAndRest[0]).Groups[1].Value;
                var content = new Dictionary<string, int>();
                foreach (var contentLine in firstAndRest[1].Split(','))
                {
                    var m = bagRegex.Match(contentLine);
                    if (m.Success == false)
                        throw new Exception("No match!");
                    var color = m.Groups[2].Value;
                    if (m.Groups[1].Value != "no")
                        content.Add(color, int.Parse(m.Groups[1].Value));
                }

                bagToContent.Add(bag, content);
            }

            return bagToContent;
        }
    }
}