using System;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            DateTime start = DateTime.Now;
            string result = new Day5().CalcB().ToString();
            DateTime stop = DateTime.Now;

            Console.WriteLine("It took " + (stop - start).TotalSeconds);

            WindowsClipboard.SetText(result);
            Console.WriteLine(result);
        }
    }
}
