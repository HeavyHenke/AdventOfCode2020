using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    class Day23
    {
        public string CalcA()
        {
            var input = "643719258";

            var cupBoard = input.Select(c => c - '0').ToList();

            int currentIx = 0;
            for (int i = 0; i < 100; i++)
            {
                CalcOne(ref currentIx, cupBoard, 9);
            }

            string result = "";
            var startIx = (cupBoard.IndexOf(1) + 1) % cupBoard.Count;
            while (cupBoard[startIx] != 1)
            {
                result += cupBoard[startIx].ToString();
                startIx = (startIx + 1) % cupBoard.Count;
            }

            return result;
        }

        private static void CalcOne(ref int currentIx, List<int> cupBoard, int size)
        {
            var hand = new int[3];
            var pickupIx = (currentIx + 1) % size;

            for (int h = 0; h < 3; h++)
            {
                hand[h] = cupBoard[pickupIx++];
                if (pickupIx >= cupBoard.Count)
                    pickupIx = 0;
            }

            pickupIx = (currentIx + 1) % size;
            if (pickupIx + 3 >= cupBoard.Count)
            {
                var toRemoveInFront = pickupIx - cupBoard.Count + 3;
                cupBoard.RemoveRange(pickupIx, 3 - toRemoveInFront);
                cupBoard.RemoveRange(0, toRemoveInFront);
                currentIx -= toRemoveInFront;
            }
            else
            {
                cupBoard.RemoveRange(pickupIx, 3);
                if (pickupIx < currentIx)
                    currentIx -= 3;
            }


            var dest = cupBoard[currentIx] - 1;
            while (dest <= 0 || hand.Contains(dest))
            {
                dest--;
                if (dest <= 0)
                    dest = cupBoard.Max();
            }


            var destIx = cupBoard.IndexOf(dest);
            cupBoard.InsertRange(destIx + 1, hand);

            if (destIx < currentIx)
                currentIx += 3;

            currentIx = (currentIx + 1) % size;
        }

        public long CalcB()
        {
            var input = "643719258".Select(i => i - '0').ToList();
            const int size = 1000000 + 1;
            var cupBoard = new int[size];
            for (int i = 1; i < input.Count; i++)
            {
                cupBoard[input[i - 1]] = input[i];
            }

            cupBoard[input.Last()] = input.Count + 1;

            for (int i = input.Count + 1; i < size - 1; i++)
            {
                cupBoard[i] = i + 1;
            }
            cupBoard[size - 1] = input[0];

            int currentIx = input[0];
            for (int i = 0; i < 10000000; i++)
            {
                CalcOne(ref currentIx, cupBoard);
            }


            long result = (long) cupBoard[1] * (long)cupBoard[cupBoard[1]];

            return result;
        }

        private static void CalcOne(ref int currentIx, int[] cupBoard)
        {
            // Pickup
            int firstPickup = cupBoard[currentIx];
            cupBoard[currentIx] = cupBoard[cupBoard[cupBoard[cupBoard[currentIx]]]];  // Close the gap

            // Calc where to put it down
            int dropLocation = currentIx - 1;
            while (dropLocation == firstPickup || dropLocation == cupBoard[firstPickup] ||
                   dropLocation == cupBoard[cupBoard[firstPickup]] || dropLocation <= 0)
            {
                dropLocation--;
                if (dropLocation <= 0)
                    dropLocation = cupBoard.Length - 1;
            }

            // Drop
            int firstAfterDrop = cupBoard[dropLocation];
            cupBoard[dropLocation] = firstPickup;
            cupBoard[cupBoard[cupBoard[firstPickup]]] = firstAfterDrop;

            currentIx = cupBoard[currentIx];
        }
    }
}