using System.IO;
using System.Linq;
using MoreLinq;

namespace AdventOfCode2020
{
    internal class Day13
    {
        public int CalcA()
        {
            var input = File.ReadAllLines("Day13.txt");
            var earliestDeparture = int.Parse(input[0]);
            var (id, waitTime) = input[1]
                .Split(",")
                .Where(s => s != "x")
                .Select(int.Parse)
                .Select(id => (id, waitTime: (1 + earliestDeparture / id) * id - earliestDeparture))
                .MinBy(tuple => tuple.waitTime)
                .First();

            return waitTime * id;
        }

        public long CalcB()
        {
            var input = File.ReadAllLines("Day13.txt");
            var buses = input[1]
                .Split(",")
                .Index()
                .Where(s => s.Value != "x")
                .Select(k => (index: k.Key, bus: int.Parse(k.Value)))
                .ToList();

            long time = buses[0].bus;
            long period = buses[0].bus;

            for (int i = 1; i < buses.Count; i++)
            {
                (time, period) = FindPeriodForBus(time, period, buses[i].index, buses[i].bus);
            }

            return time;
        }

        private (long time, long period) FindPeriodForBus(long time, long period, int ix, int bus)
        {
            var diff = (1L + time / bus) * bus - time;
            long firstTime = -1;

            while (ix > bus)
                ix -= bus;

            while (true)
            {
                if (diff == ix)
                {
                    if (firstTime == -1)
                    {
                        firstTime = time;
                    }
                    else
                    {
                        var newPeriod = time - firstTime;
                        var gcd = GCD(period, newPeriod);
                        period = (period / gcd) * (newPeriod / gcd) * gcd;
                        return (firstTime, period);
                    }
                }

                time += period;
                diff = (1L + time / bus) * bus - time;
            }
        }

        private static long GCD(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

    }
}