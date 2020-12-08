using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    internal class Day8
    {
        private int _acc;
        private int _ip;

        public int CalcA()
        {
            var program = ParseProgram(-1);
            IsInfinite(program);
            return _acc;
        }

        public int CalcB()
        {
            int changeRowNo = 0;
            while (true)
            {
                var program = ParseProgram(changeRowNo++);
                if (!IsInfinite(program))
                    return _acc;
            }
        }

        private List<Action> ParseProgram(int changeInstruction)
        {
            var progLines = File.ReadAllLines("Day08.txt");

            var program = new List<Action>();

            var parser = new Regex(@"^(\w+) \+?(-?\d+)");
            foreach (var line in progLines)
            {
                var m = parser.Match(line);

                var op = m.Groups[1].Value;
                var val = int.Parse(m.Groups[2].Value);

                if (program.Count == changeInstruction && op == "nop")
                    op = "jmp";
                else if (program.Count == changeInstruction && op == "jmp")
                    op = "nop";

                Action action = op switch
                {
                    "nop" => () => _ip++,
                    "acc" => () => { _acc += val; _ip++; },
                    "jmp" => () => _ip += val,
                    _ => throw new Exception("Unknown opcode")
                };

                program.Add(action);
            }

            return program;
        }

        private bool IsInfinite(IReadOnlyList<Action> program)
        {
            _ip = _acc = 0;

            var visited = new HashSet<int>();
            while (_ip < program.Count)
            {
                if (visited.Add(_ip) == false)
                    return true;
                program[_ip]();
            }

            return false;
        }

    }
}