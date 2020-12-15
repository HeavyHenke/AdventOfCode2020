using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    internal class Day15
    {
        public long CalcA()
        {
            int turn = 1;
            var numToTurn = new Dictionary<int, int>();
            int lastNum = -1;

            var input = "7,12,1,0,16,2"
                .Split(',')
                .Select(int.Parse);
            
            foreach (var i in input)
            {
                if (lastNum != -1)
                    numToTurn[lastNum] = turn-1;

                turn++;
                lastNum = i;
            }

            while (true)
            {
                int sayNum;
                if (numToTurn.TryGetValue(lastNum, out var lastTurn))
                {
                    sayNum = turn - lastTurn - 1;
                }
                else
                {
                    sayNum = 0;
                }

                if (turn == 2020)
                    return sayNum;

                numToTurn[lastNum] = turn - 1;
                lastNum = sayNum;
                turn++;
            }
        }

        public long CalcB()
        {
            int turn = 1;
            var numToTurn = new Dictionary<int, int>();
            int lastNum = -1;

            var input = "7,12,1,0,16,2"
                .Split(',')
                .Select(int.Parse);

            foreach (var i in input)
            {
                if (lastNum != -1)
                    numToTurn[lastNum] = turn - 1;

                turn++;
                lastNum = i;
            }

            while (true)
            {
                int sayNum;
                if (numToTurn.TryGetValue(lastNum, out var lastTurn))
                {
                    sayNum = turn - lastTurn - 1;
                }
                else
                {
                    sayNum = 0;
                }

                if (turn == 30000000)
                    return sayNum;

                numToTurn[lastNum] = turn - 1;
                lastNum = sayNum;
                turn++;
            }
        }

    }
}