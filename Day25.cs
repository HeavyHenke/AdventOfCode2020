namespace AdventOfCode2020
{
    internal class Day25
    {
        public long CalcA()
        {
            const int cardKey = 8987316;
            const int doorKey = 14681524;

            int cardLoopSize = GetLoopSize(7, cardKey);

            long encryptionKey = doorKey;
            for (int i = 1; i < cardLoopSize; i++)
                encryptionKey = Transform(encryptionKey, doorKey);

            return encryptionKey;
        }

        private int GetLoopSize(int subject, long goal)
        {
            long key = subject;
            int loops = 1;
            while (key != goal)
            {
                key = Transform(key, 7);
                loops++;
            }

            return loops;
        }

        private long Transform(long input, long subject)
        {
            return (input * subject) % 20201227;
        }
    }
}