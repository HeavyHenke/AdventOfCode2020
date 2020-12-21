using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020
{
    internal class Day18
    {
        public long CalcA()
        {
            var rows = File.ReadAllLines("Day18.txt");

            long total = 0;
            foreach (var row in rows)
            {
                int ix = 0;
                long val = CalcSum(row.Replace(" ", ""), ref ix);
                total += val;
            }

            return total;
        }

        private long CalcSum(string str, ref int ix)
        {
            long val = ReadTerm(str, ref ix);
            while(ix < str.Length)
            {
                if (str[ix] == ')')
                {
                    ix++;
                    return val;
                }

                var operation = str[ix++];
                var operand = ReadTerm(str, ref ix);
                val = operation switch
                {
                    '+' => val + operand,
                    '*' => val * operand,
                    _ => throw new Exception("tokig operation: " + operation)
                };
            }

            return val;
        }

        private long ReadTerm(string input, ref int ix)
        {
            if (char.IsNumber(input[ix]))
            {
                int val = input[ix++] - '0';
                return val;
            }

            if(input[ix] != '(')
                throw new Exception("Oväntat");

            ix++;
            return CalcSum(input, ref ix);
        }


        public long CalcB()
        {
            var rows = File.ReadAllLines("Day18.txt");

            long total = 0;
            foreach (var row in rows)
            {
                int ix = 0;
                long val = CalcSum2(row.Replace(" ", ""), ref ix);
                total += val;
            }

            return total;
        }

        private long CalcSum2(string str, ref int ix)
        {
            var operands = new List<long>();
            var operations = new List<char>();

            operands.Add(ReadTerm2(str, ref ix));

            while (ix < str.Length)
            {
                if (str[ix] == ')')
                {
                    ix++;
                    return CalcInCorrectOrder(operands, operations);
                }

                operations.Add(str[ix++]);
                operands.Add(ReadTerm2(str, ref ix));
            }

            return CalcInCorrectOrder(operands, operations);
        }

        private long CalcInCorrectOrder(List<long> operand, List<char> operation)
        {
            // Do all +
            for (int i = 0; i < operation.Count; i++)
            {
                if (operation[i] == '+')
                {
                    operand[i] = operand[i] + operand[i + 1];
                    operand.RemoveAt(i+1);
                    operation.RemoveAt(i);
                    i--;
                }
            }

            long val = 1;
            foreach (var op in operand)
                val *= op;

            return val;
        }

        private long ReadTerm2(string input, ref int ix)
        {
            if (char.IsNumber(input[ix]))
            {
                int val = input[ix++] - '0';
                return val;
            }

            if (input[ix] != '(')
                throw new Exception("Oväntat");

            ix++;
            return CalcSum2(input, ref ix);
        }
    }
}