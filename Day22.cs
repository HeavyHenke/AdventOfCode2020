using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day22
    {
        public long CalcA()
        {
            var lines = File.ReadAllLines("Day22.txt");
            var p1 = new List<int>();
            var p2 = new List<int>();
            var player = p1;
            foreach (var line in lines.Skip(1))
            {
                if (line.StartsWith("Player"))
                    player = p2;
                else if (line != "")
                    player.Add(int.Parse(line));
            }

            while (p1.Count > 0 && p2.Count > 0)
            {
                var c1 = p1[0];
                var c2 = p2[0];
                p1.RemoveAt(0);
                p2.RemoveAt(0);
                if (c2 > c1)
                    p2.AddRange(new[] {c2, c1});
                else
                    p1.AddRange(new[] {c1, c2});
            }

            player = p1.Count > 0 ? p1 : p2;
            long score = 0;
            long factor = 1;
            for (int i = player.Count - 1; i >= 0; i--)
            {
                score += player[i] * factor++;
            }

            return score;
        }

        public long CalcB()
        {
            var lines = File.ReadAllLines("Day22.txt");
            var p1 = new List<int>();
            var p2 = new List<int>();
            var player = p1;
            foreach (var line in lines.Skip(1))
            {
                if (line.StartsWith("Player"))
                    player = p2;
                else if (line != "")
                    player.Add(int.Parse(line));
            }

            var p1Q = new Queue<int>(p1);
            var p2Q = new Queue<int>(p2);
            if (PlayRecursiveCombatIsP1Winner(p1Q, p2Q, 0))
            {
                Console.WriteLine("Player 1 wins");
                player = p1Q.ToList();
            }
            else
            {
                Console.WriteLine("Player 2 wins");
                player = p2Q.ToList();
            }

            long score = 0;
            long factor = 1;
            for (int i = player.Count - 1; i >= 0; i--)
            {
                score += player[i] * factor++;
            }

            return score;
        }

        private static bool PlayRecursiveCombatIsP1Winner(Queue<int> p1, Queue<int> p2, int depth)
        {
            if (depth > 0)
            {
                // If P2 cant win P1´s top card then P1 will eventually win this game (maybe by the duplicate hands rule, that is why this logic doesnt work for P2)
                var p1Max = p1.Max();
                var p2Max = p2.Max();
                if (p1Max > p2Max && p1Max > (p1.Count + p2.Count))
                    return true;
            }

            var previousHandsP1 = new HashSet<string>();
            var previousHandsP2 = new HashSet<string>();

            while (p1.Count > 0 && p2.Count > 0)
            {
                var p1Hash = GetHash(p1);
                if (previousHandsP1.Add(p1Hash) == false)
                    return true;
                var p2Hash = GetHash(p2);
                if (previousHandsP2.Add(p2Hash) == false)
                    return true;

                var c1 = p1.Dequeue();
                var c2 = p2.Dequeue();

                bool isP1Winner;
                if (c1 <= p1.Count && c2 <= p2.Count)
                    isP1Winner = PlayRecursiveCombatIsP1Winner(new Queue<int>(p1.Take(c1)), new Queue<int>(p2.Take(c2)), depth + 1);
                else
                    isP1Winner = c1 > c2;

                if (isP1Winner)
                {
                    p1.Enqueue(c1);
                    p1.Enqueue(c2);
                }
                else
                {
                    p2.Enqueue(c2);
                    p2.Enqueue(c1);
                }
            }

            if (p1.Count > 0)
                return true;

            return false;
        }

        private static string GetHash(IReadOnlyCollection<int> player)
        {
            var hash = new char[player.Count];
            int ix = 0;
            foreach (var card in player)
            {
                hash[ix++] = (char) (card + '!');
            }

            return new string(hash);
        }
    }
}