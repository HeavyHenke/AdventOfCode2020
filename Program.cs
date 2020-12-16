﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            var sw = new Stopwatch();
            sw.Start();
            string result = new Day16().CalcB().ToString();
            sw.Stop();

            Console.WriteLine("It took " + sw.Elapsed);

            WindowsClipboard.SetText(result);
            Console.WriteLine(result);
        }
    }

    internal class Day17
    {
        public long CalcA()
        {
            return -1;
        }

        public long CalcB()
        {
            return -1;
        }

    }
}
