using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day5
    {
        public int CalcA()
        {
            var max = File.ReadAllLines("Day05.txt")
                .Select(GetRowCol)
                .Select(p => p.row * 8 + p.col)
                .Max();
            return max;
        }

        public int CalcB()
        {
            var takenSeats = File.ReadAllLines("Day05.txt")
                .Select(GetRowCol)
                .Select(p => p.row * 8 + p.col)
                .ToHashSet();

            for (int row = 0; row < 128; row++)
            for (int col = 0; col < 8; col++)
            {
                var id = row * 8 + col;
                if (takenSeats.Contains(id) == false && takenSeats.Contains(id - 1) && takenSeats.Contains(id + 1))
                    return id;
            }

            throw new Exception("Not found.");
        }

        private (int row, int col) GetRowCol(string pass)
        {
            int mask = 64;
            int row = 0;
            int col = 0;
            for (int i = 0; i < 7; i++)
            {
                if (pass[i] == 'B')
                    row |= mask;
                mask >>= 1;
            }

            mask = 4;
            for (int i = 0; i < 3; i++)
            {
                if (pass[i + 7] == 'R')
                    col |= mask;
                mask >>= 1;
            }

            return (row, col);
        }
    }
}