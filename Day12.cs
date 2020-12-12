using System;
using System.IO;

namespace AdventOfCode2020
{
    internal class Day12
    {
        private enum Direction
        {
            East = 0,
            North = 1,
            West = 2,
            South = 3
        }
        public int CalcA()
        {
            var instructionSet = File.ReadAllLines("Day12.txt");
            var direction = Direction.East;
            var (x, y) = (0, 0);

            foreach(var instruction in instructionSet)
            {
                var opCode = instruction[0];
                var value = int.Parse(instruction.Substring(1));
                switch (opCode)
                {
                    case 'N':
                        y -= value;
                        break;
                    case 'S':
                        y += value;
                        break;
                    case 'E':
                        x -= value;
                        break;
                    case 'W':
                        x += value;
                        break;
                    case 'L':
                        while(value > 0)
                        {
                            direction++;
                            if ((int)direction == 4)
                                direction = Direction.East;
                            value -= 90;
                        }
                        break;
                    case 'R':
                        while(value > 0)
                        {
                            direction--;
                            if ((int)direction == -1)
                                direction = Direction.South;
                            value -= 90;
                        }
                        break;
                    case 'F':
                        switch (direction)
                        {
                            case Direction.East:
                                x -= value;
                                break;
                            case Direction.North:
                                y -= value;
                                break;
                            case Direction.West:
                                x += value;
                                break;
                            case Direction.South:
                                y += value;
                                break;
                        }
                        break;
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }
        public int CalcB()
        {
            var instructionSet = File.ReadAllLines("Day12.txt");
            var (x, y) = (0, 0);
            var (wx, wy) = (-10, -1);

            foreach (var instruction in instructionSet)
            {
                var opCode = instruction[0];
                var value = int.Parse(instruction.Substring(1));

                if (opCode == 'R')
                {
                    opCode = 'L';
                    value = -value;
                }

                switch (opCode)
                {
                    case 'N':
                        wy -= value;
                        break;
                    case 'S':
                        wy += value;
                        break;
                    case 'E':
                        wx -= value;
                        break;
                    case 'W':
                        wx += value;
                        break;
                    case 'L':
                        var direction = Math.Atan2(wy, wx);
                        var length = Math.Sqrt(wx * wx + wy * wy);
                        var dirChangeInRads = (value / 180.0) * Math.PI;
                        direction += dirChangeInRads;
                        wy = (int) Math.Round(length * Math.Sin(direction));
                        wx = (int) Math.Round(length * Math.Cos(direction));
                        break;
                    case 'F':
                        x += wx * value;
                        y += wy * value;
                        break;
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }

    }
}