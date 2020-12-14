using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            var sw = new Stopwatch();
            sw.Start();
            string result = new Day13().CalcA().ToString();
            sw.Stop();

            Console.WriteLine("It took " + sw.Elapsed);

            WindowsClipboard.SetText(result);
            Console.WriteLine(result);
        }
    }


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
    }
}
