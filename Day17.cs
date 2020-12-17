using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020
{
    internal class Day17
    {
        class State
        {
            private HashSet<(int x, int y, int z)> _hash;

            private int _minX = 0;
            private int _maxX = 0;
            private int _minY = 0;
            private int _maxY = 0;
            private int _minZ = 0;
            private int _maxZ = 0;

            public State(string[] initialState)
            {
                _hash = new HashSet<(int x, int y, int z)>();

                const int z = 0;
                for (int y = 0; y < initialState.Length; y++)
                {
                    for (int x = 0; x < initialState[y].Length; x++)
                    {
                        if (initialState[y][x] == '#')
                            Set((x, y, z));
                    }
                }
            }

            public State(State copy)
            {
                _hash = new HashSet<(int x, int y, int z)>(copy._hash);
                _minX = copy._minX;
                _maxX = copy._maxX;
                _minY = copy._minY;
                _maxY = copy._maxY;
                _minZ = copy._minZ;
                _maxZ = copy._maxZ;
            }

            public long NumSet => _hash.Count;

            public State Next()
            {
                var copy = new State(this);
                for (int z = _minZ - 1; z <= _maxZ + 1; z++)
                {
                    for (int y = _minY - 1; y <= _maxY + 1; y++)
                    {
                        for (int x = _minX - 1; x <= _maxX + 1; x++)
                        {
                            var coord = (x, y, z);
                            int num = GetNumNeighbors(coord);
                            var isActive = _hash.Contains(coord);
                            if (isActive && num != 2 && num != 3)
                                copy.Reset(coord);
                            else if (isActive == false && num == 3)
                                copy.Set(coord);
                        }
                    }
                }

                return copy;
            }

            private int GetNumNeighbors((int x, int y, int z) coord)
            {
                int num = 0;
                for (int z = coord.z - 1; z <= coord.z + 1; z++)
                {
                    for (int y = coord.y - 1; y <= coord.y + 1; y++)
                    {
                        for (int x = coord.x - 1; x <= coord.x + 1; x++)
                        {
                            var neighborCoord = (x, y, z);
                            if (neighborCoord == coord)
                                continue;
                            if (_hash.Contains(neighborCoord))
                                num++;
                        }
                    }
                }

                return num;
            }


            private void Set((int x, int y, int z) coord)
            {
                if (coord.x > _maxX)
                    _maxX = coord.x;
                if (coord.x < _minX)
                    _minX = coord.x;
                if (coord.y > _maxY)
                    _maxY = coord.y;
                if (coord.y < _minY)
                    _minY = coord.y;
                if (coord.z > _maxZ)
                    _maxZ = coord.z;
                if (coord.z < _minZ)
                    _minZ = coord.z;

                _hash.Add(coord);
            }

            private void Reset((int x, int y, int z) coord)
            {
                _hash.Remove(coord);
            }

            public void Print()
            {
                for (int z = _minZ; z <= _maxZ; z++)
                {
                    Console.WriteLine("z = " + z);
                    for (int y = _minY; y <= _maxY; y++)
                    {
                        for (int x = _minX; x <= _maxX; x++)
                        {
                            if (_hash.Contains((x,y,z)))
                                Console.Write("#");
                            else
                                Console.Write(".");
                        }

                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }

        }

        class State4d
        {
            private readonly HashSet<(int x, int y, int z, int w)> _hash;

            private int _minX = 0;
            private int _maxX = 0;
            private int _minY = 0;
            private int _maxY = 0;
            private int _minZ = 0;
            private int _maxZ = 0;
            private int _minW = 0;
            private int _maxW = 0;

            public State4d(string[] initialState)
            {
                _hash = new HashSet<(int x, int y, int z, int w)>();

                const int z = 0;
                const int w = 0;
                for (int y = 0; y < initialState.Length; y++)
                {
                    for (int x = 0; x < initialState[y].Length; x++)
                    {
                        if (initialState[y][x] == '#')
                            Set((x, y, z, w));
                    }
                }
            }

            public State4d(State4d copy)
            {
                _hash = new HashSet<(int x, int y, int z, int w)>(copy._hash);
                _minX = copy._minX;
                _maxX = copy._maxX;
                _minY = copy._minY;
                _maxY = copy._maxY;
                _minZ = copy._minZ;
                _maxZ = copy._maxZ;
                _minW = copy._minW;
                _maxW = copy._maxW;
            }

            public long NumSet => _hash.Count;

            public State4d Next()
            {
                var copy = new State4d(this);
                for (int w = _minW - 1; w <= _maxW + 1; w++)
                {
                    for (int z = _minZ - 1; z <= _maxZ + 1; z++)
                    {
                        for (int y = _minY - 1; y <= _maxY + 1; y++)
                        {
                            for (int x = _minX - 1; x <= _maxX + 1; x++)
                            {
                                var coord = (x, y, z, w);
                                int num = GetNumNeighbors(coord);
                                var isActive = _hash.Contains(coord);
                                if (isActive && num != 2 && num != 3)
                                    copy.Reset(coord);
                                else if (isActive == false && num == 3)
                                    copy.Set(coord);
                            }
                        }
                    }
                }

                return copy;
            }

            private int GetNumNeighbors((int x, int y, int z, int w) coord)
            {
                int num = 0;
                for (int w = coord.w - 1; w <= coord.w + 1; w++)
                for (int z = coord.z - 1; z <= coord.z + 1; z++)
                for (int y = coord.y - 1; y <= coord.y + 1; y++)
                for (int x = coord.x - 1; x <= coord.x + 1; x++)
                {
                    var neighborCoord = (x, y, z, w);
                    if (neighborCoord == coord)
                        continue;
                    if (_hash.Contains(neighborCoord))
                        num++;
                }

                return num;
            }


            private void Set((int x, int y, int z, int w) coord)
            {
                if (coord.x > _maxX)
                    _maxX = coord.x;
                if (coord.x < _minX)
                    _minX = coord.x;
                if (coord.y > _maxY)
                    _maxY = coord.y;
                if (coord.y < _minY)
                    _minY = coord.y;
                if (coord.z > _maxZ)
                    _maxZ = coord.z;
                if (coord.z < _minZ)
                    _minZ = coord.z;
                
                if (coord.w > _maxW)
                    _maxW = coord.w;
                if (coord.w < _minW)
                    _minW = coord.w;

                _hash.Add(coord);
            }

            private void Reset((int x, int y, int z, int w) coord)
            {
                _hash.Remove(coord);
            }

            public void Print()
            {

                for (int z = _minZ; z <= _maxZ; z++)
                {
                    Console.WriteLine("z = " + z);
                    for (int y = _minY; y <= _maxY; y++)
                    {
                        for (int x = _minX; x <= _maxX; x++)
                        {
                            if (_hash.Contains((x, y, z, 2)))
                                Console.Write("#");
                            else
                                Console.Write(".");
                        }

                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }

        }

        public long CalcA()
        {
            var input = File.ReadAllLines("Day17.txt");
            var state = new State(input);

            for (int i = 0; i < 6; i++)
            {
                state = state.Next();
            }

            return state.NumSet;
        }

        public long CalcB()
        {
            var input = File.ReadAllLines("Day17.txt");
            var state = new State4d(input);

            for (int i = 0; i < 6; i++)
            {
                state = state.Next();
            }

            return state.NumSet;
        }

    }
}