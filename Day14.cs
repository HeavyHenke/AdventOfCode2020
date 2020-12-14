using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    internal class Day14
    {
        public long CalcA()
        {
            var input = File.ReadAllLines("Day14.txt");

            long orMask = 0;
            long andMask = 0xFFFF_FFFF;

            var regEx = new Regex(@"mem\[(\d+)] = (\d+)", RegexOptions.Compiled);
            var memory = new Dictionary<long, long>();
            foreach (var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    var maskPart = line.Split(" = ")[1];
                    orMask = Convert.ToInt64(maskPart.Replace('X','0'), 2);
                    andMask = Convert.ToInt64(maskPart.Replace('X', '1'), 2);
                    continue;
                }

                var m = regEx.Match(line);
                var addr = long.Parse(m.Groups[1].Value);
                var val = long.Parse(m.Groups[2].Value);

                val &= andMask;
                val |= orMask;
                memory[addr] = val;
            }

            return memory.Values.Sum();
        }

        public long CalcB()
        {
            var input = File.ReadAllLines("Day14.txt");

            long orMask = 0;
            long floatingMask = 0;

            var regEx = new Regex(@"mem\[(\d+)] = (\d+)", RegexOptions.Compiled);
            var memory = new Dictionary<long, long>();
            foreach (var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    var maskPart = line.Split(" = ")[1];
                    orMask = Convert.ToInt64(maskPart.Replace('X', '0'), 2);
                    floatingMask = Convert.ToInt64(maskPart.Replace('1', '0').Replace('X', '1'), 2);
                    continue;
                }

                var m = regEx.Match(line);
                var addr = long.Parse(m.Groups[1].Value);
                var val = long.Parse(m.Groups[2].Value);

                addr |= orMask;

                foreach(var realAddr in GetAllAddresses(addr, floatingMask))
                    memory[realAddr] = val;
            }

            return memory.Values.Sum();
        }

        private IEnumerable<long> GetAllAddresses(long addr, long floatMask)
        {
            if (floatMask == 0)
            {
                yield return addr;
                yield break;
            }

            var ix = IndexOfFirstSetBit(floatMask);
            var floatMaskMinusOneBit = floatMask & (0xFFFF_FFFF_FFFF ^ (1L << ix));

            addr &= (0xFFFF_FFFF_FFFF ^ (1L << ix));
            foreach (var sub in GetAllAddresses(addr, floatMaskMinusOneBit))
                yield return sub;

            addr |= (1L << ix);
            foreach (var sub in GetAllAddresses(addr, floatMaskMinusOneBit))
                yield return sub;
        }

        private int IndexOfFirstSetBit(long test)
        {
            var binForm = Convert.ToString(test, 2);
            return binForm.Length - binForm.IndexOf('1') - 1;
        }
    }
}