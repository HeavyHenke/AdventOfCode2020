using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    internal class Day16
    {
        public long CalcA()
        {
            var rules = new List<(string name, int fromA, int toA, int fromB, int toB)>();

            var lines = File.ReadAllLines("Day16.txt");
            int i = 0;

            var regEx = new Regex($"([\\w ]*): (\\d+)-(\\d+) or (\\d+)-(\\d+)", RegexOptions.Compiled);
            while (lines[i] != "")
            {
                var m = regEx.Match(lines[i]);
                rules.Add((m.Groups[1].Value, int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value)));

                i++;
            }

            i += 5;

            long invalidNum = 0;
            while (i < lines.Length)
            {
                var ticketFields = lines[i].Split(',').Select(int.Parse).ToList();
                foreach (var val in ticketFields)
                {
                    if (rules.Any(r => IsRuleValid(val, r)) == false)
                    {
                        invalidNum += val;
                    }
                }

                i++;
            }


            return invalidNum;
        }

        private bool IsRuleValid(int val, (string name, int fromA, int toA, int fromB, int toB) rule)
        {
            if (val >= rule.fromA && val <= rule.toA)
                return true;
            if (val >= rule.fromB && val <= rule.toB)
                return true;
            return false;
        }

        private static bool IsRuleValid(int val, (int fromA, int toA, int fromB, int toB) rule)
        {
            if (val >= rule.fromA && val <= rule.toA)
                return true;
            if (val >= rule.fromB && val <= rule.toB)
                return true;
            return false;
        }


        private Dictionary<string, (int fromA, int toA, int fromB, int toB)> _rules;
        private List<List<int>> _tickets;

        public long CalcB()
        {
            _rules = new Dictionary<string, (int fromA, int toA, int fromB, int toB)>();

            var lines = File.ReadAllLines("Day16.txt");
            int i = 0;

            var regEx = new Regex($"([\\w ]*): (\\d+)-(\\d+) or (\\d+)-(\\d+)", RegexOptions.Compiled);
            while (lines[i] != "")
            {
                var m = regEx.Match(lines[i]);
                _rules.Add(m.Groups[1].Value, (int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value)));
                i++;
            }

            i += 2;
            var myTicket = lines[i].Split(',').Select(int.Parse).ToList();

            i += 3;
            _tickets = new List<List<int>> { myTicket };
            while (i < lines.Length)
            {
                var ticketFields = lines[i].Split(',').Select(int.Parse).ToList();
                var isValid = ticketFields.All(val => _rules.Any(r => IsRuleValid(val, r.Value)));
                if (isValid)
                    _tickets.Add(ticketFields);
                i++;
            }

            var solution = SearchForSolution(new Dictionary<string, int>());

            long product = 1;
            foreach (var (key, value) in solution)
            {
                if (key.StartsWith("departure"))
                    product *= myTicket[value];
            }

            return product;
        }


        private Dictionary<string, int> SearchForSolution(Dictionary<string, int> fixedIndexes)
        {
            if (fixedIndexes.Count == _rules.Count)
                return fixedIndexes;

            var foundIndexes = fixedIndexes.Values.ToHashSet();

            var ruleToNotMatchingField = _rules.Keys
                .Where(r => fixedIndexes.ContainsKey(r) == false)
                .ToDictionary(rule => rule, rule => new HashSet<int>());

            foreach (var ticket in _tickets)
            {
                for (int fix = 0; fix < ticket.Count; fix++)
                {
                    if (foundIndexes.Contains(fix))
                        continue;

                    foreach (var rule in _rules)
                    {
                        if (fixedIndexes.ContainsKey(rule.Key))
                            continue;

                        if (IsRuleValid(ticket[fix], rule.Value) == false)
                        {
                            ruleToNotMatchingField[rule.Key].Add(fix);
                        }
                    }
                }
            }

            var leftToFind = _rules.Count - fixedIndexes.Count;
            if (ruleToNotMatchingField.Any(r => r.Value.Count == leftToFind))
                return null;    // Found a rule that matches no fields.

            // Use the easiest field in each level of this search
            var easiestField = MoreLinq.MoreEnumerable.MaxBy(ruleToNotMatchingField, r => r.Value.Count).Select(keyValuePair => keyValuePair.Key).First();
            var invalidIndexes = ruleToNotMatchingField[easiestField];
            invalidIndexes.UnionWith(foundIndexes);

            for (int i = 0; i < _rules.Count; i++)
            {
                if (invalidIndexes.Contains(i))
                    continue;

                var clone = new Dictionary<string, int>(fixedIndexes);
                clone.Add(easiestField, i);

                var solution = SearchForSolution(clone);
                if (solution != null)
                    return solution;
            }

            return null;
        }

    }
}