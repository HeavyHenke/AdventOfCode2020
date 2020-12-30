using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    class Day20
    {
        public long CalcA()
        {
            var totalSides = new Dictionary<string, int>();
            var tiles = new Dictionary<int, List<string>>();

            var lines = File.ReadAllLines("Day20.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                int tileId = int.Parse(lines[i].Substring(5, lines[i].Length - 5 - 1));
                var left = new char[10];
                var right = new char[10];
                i++;
                var firstRow = lines[i];
                for (int row = 0; row < 10; row++)
                {
                    left[row] = lines[i][0];
                    right[row] = lines[i][9];
                    i++;
                }

                var sides = new List<string>
                {
                    new string(left), new string(right), firstRow, lines[i-1]
                };

                tiles.Add(tileId, sides);
                foreach (var side in sides)
                {
                    if (totalSides.ContainsKey(side))
                        totalSides[side]++;
                    else
                        totalSides.Add(side, 1);
                    var side2 = new string(side.Reverse().ToArray());
                    if (totalSides.ContainsKey(side2))
                        totalSides[side2]++;
                    else
                        totalSides.Add(side2, 1);
                }
            }

            var product = tiles.Select(t => (t.Key, numNonMatchingSides: t.Value.Count(v => totalSides[v] == 1)))
                .Where(t => t.numNonMatchingSides == 2)
                .Select(t => t.Key)
                .Aggregate(1L, (current, tileId) => current * tileId);

            return product;
        }

        public long CalcB()
        {
            var sr = new StringReader(File.ReadAllText("Day20.txt"));
            var tiles = new List<Tile>();

            var totalSides = new Dictionary<string, int>();

            while (sr.Peek() != -1)
            {
                var tile = new Tile(sr);
                tiles.Add(tile);

                foreach (var side in tile.GetCanonicalSides())
                {
                    if (totalSides.ContainsKey(side))
                        totalSides[side]++;
                    else
                        totalSides.Add(side, 1);
                }
            }

            foreach (var tile in tiles)
            {
                var sideMatches = tile.GetCanonicalSides().Count(s => totalSides[s] > 1);
                if (sideMatches == 2)
                    tile.IsCorner = true;
                if (sideMatches == 3)
                    tile.IsSide = true;
            }

            // Start with top-left corner
            var tilesPerLine = (int)Math.Sqrt(tiles.Count);
            var bigPicMatrix = new Tile[tilesPerLine, tilesPerLine];

            Tile GetTileOfNull(int y, int x)
            {
                if (y < 0 || y >= tilesPerLine || x < 0 || x >= tilesPerLine)
                    return null;
                return bigPicMatrix[y, x];
            }

            for (int y = 0; y < tilesPerLine; y++)
            {
                for (int x = 0; x < tilesPerLine; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        var corner = tiles.First(t => t.IsCorner);
                        tiles.Remove(corner);
                        while (totalSides[corner.CanonicalBottom] != 2 || totalSides[corner.CanonicalRight] != 2)
                            corner.Rotate();
                        bigPicMatrix[y, x] = corner;
                        continue;
                    }

                    var top = GetTileOfNull(y - 1, x)?.Bottom;
                    var bottom = GetTileOfNull(y + 1, x)?.Top;
                    var left = GetTileOfNull(y, x - 1)?.Right;
                    var right = GetTileOfNull(y + 1, x)?.Left;

                    Tile match;
                    if ((x == 0 && y == tilesPerLine - 1) || (x == tilesPerLine - 1 && y == 0) || (x == tilesPerLine - 1 && y == tilesPerLine - 1))
                        match = tiles.Single(c => c.IsCorner && c.MatchIfPossible(top, bottom, left, right));
                    else if (x == 0 || x == tilesPerLine - 1 || y == 0 || y == tilesPerLine - 1)
                        match = tiles.Single(s => s.IsSide && s.MatchIfPossible(top, bottom, left, right));
                    else
                        match = tiles.Single(t => t.MatchIfPossible(top, bottom, left, right));

                    bigPicMatrix[y, x] = match;
                    tiles.Remove(match);
                }
            }

            var bigTile = new Tile(bigPicMatrix);

            var monster = new[]
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #"
            };

            for (int i = 0; i < 4; i++)
            {
                bigTile.MarkMonster(monster);
                bigTile.Rotate();
            }
            bigTile.HorizontalFlip();
            for (int i = 0; i < 4; i++)
            {
                bigTile.MarkMonster(monster);
                bigTile.Rotate();
            }

            return bigTile.NumUnmarked();
        }

        class Tile
        {
            public int Id { get; }
            public char[,] Matrix { get; private set; }

            private readonly int _dimension;

            public bool IsCorner { get; set; }
            public bool IsSide { get; set; }

            public string CanonicalTop => GetCanonical(Top);
            public string CanonicalBottom => GetCanonical(Bottom);

            public string CanonicalLeft => GetCanonical(Left);
            public string CanonicalRight => GetCanonical(Right);

            public string Top => GetSlice(0, 0, 1, 0);
            public string Bottom => GetSlice(0, _dimension - 1, 1, 0);

            public string Left => GetSlice(0, 0, 0, 1);
            public string Right => GetSlice(_dimension - 1, 0, 0, 1);


            public Tile(TextReader sr)
            {
                var tileLine = sr.ReadLine();
                Id = int.Parse(tileLine.Substring(5, tileLine.Length - 6));

                var line = sr.ReadLine();
                Matrix = new char[line.Length, line.Length];
                _dimension = line.Length;
                int y = 0;
                while (!string.IsNullOrEmpty(line))
                {
                    for (int x = 0; x < line.Length; x++)
                        Matrix[y, x] = line[x];

                    y++;
                    line = sr.ReadLine();
                }
            }

            private Tile(Tile copy)
            {
                Id = copy.Id;
                Matrix = (char[,]) copy.Matrix.Clone();
                _dimension = copy._dimension;
            }

            public Tile(Tile[,] bigPicMatrix)
            {
                var len0 = bigPicMatrix.GetLength(0);
                var innerDim = bigPicMatrix[0, 0]._dimension;

                _dimension = (innerDim - 2) * len0;
                Matrix = new char[_dimension, _dimension];
                for (int bigY = 0; bigY < len0; bigY++)
                {
                    for (int bigX = 0; bigX < len0; bigX++)
                    {
                        for (int y = 1; y < innerDim - 1; y++)
                        {
                            for (int x = 1; x < innerDim - 1; x++)
                            {
                                int mx = bigX * (innerDim - 2) + x - 1;
                                int my = bigY * (innerDim - 2) + y - 1;
                                Matrix[my, mx] = bigPicMatrix[bigY, bigX].Matrix[y, x];
                            }
                        }
                    }
                }
            }

            public IEnumerable<string> GetCanonicalSides()
            {
                yield return CanonicalTop;
                yield return CanonicalBottom;
                yield return CanonicalLeft;
                yield return CanonicalRight;
            }

            private static string GetCanonical(string side)
            {
                var reverse = new string(side.Reverse().ToArray());
                if (side.CompareTo(reverse) > 0)
                    return side;
                return reverse;
            }

            private string GetSlice(int startX, int startY, int dx, int dy)
            {
                var result = new StringBuilder(_dimension);
                while (startX < _dimension && startY < _dimension)
                {
                    result.Append(Matrix[startY, startX]);
                    startY += dy;
                    startX += dx;
                }

                return result.ToString();
            }

            public void Rotate()
            {
                char[,] newMatrix = new char[Matrix.GetLength(1), Matrix.GetLength(0)];
                int newColumn, newRow = 0;
                for (int oldColumn = Matrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
                {
                    newColumn = 0;
                    for (int oldRow = 0; oldRow < Matrix.GetLength(0); oldRow++)
                    {
                        newMatrix[newRow, newColumn] = Matrix[oldRow, oldColumn];
                        newColumn++;
                    }

                    newRow++;
                }

                Matrix = newMatrix;
            }

            public void HorizontalFlip()
            {
                char[,] newMatrix = new char[Matrix.GetLength(1), Matrix.GetLength(0)];
                for (int y = 0; y < _dimension; y++)
                {
                    for (int x = 0; x < _dimension; x++)
                    {
                        newMatrix[_dimension - y - 1, x] = Matrix[y, x];
                    }
                }

                Matrix = newMatrix;
            }

            public bool MatchIfPossible(string top, string bottom, string left, string right)
            {
                var c1 = new Tile(this);
                var c2 = new Tile(this);
                c2.HorizontalFlip();
                foreach (var c in new[] {c1, c2})
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if ((left == null || left == c.Left) &&
                            (right == null || right == c.Right) &&
                            (top == null || top == c.Top) &&
                            (bottom == null || bottom == c.Bottom))
                        {
                            Matrix = c.Matrix;
                            return true;
                        }

                        c.Rotate();
                    }
                }

                return false;
            }

            public void MarkMonster(string[] monsterDef)
            {
                for (int y = 0; y < _dimension - monsterDef.Length; y++)
                {
                    for (int x = 0; x < _dimension - monsterDef[0].Length; x++)
                    {
                        if (IsMonsterAt(y, x, monsterDef))
                            MarkMonster(y, x, monsterDef);
                    }
                }
            }

            private bool IsMonsterAt(int targetY, int targetX, string[] monster)
            {
                for (int y = 0; y < monster.Length; y++)
                {
                    for (int x = 0; x < monster[y].Length; x++)
                    {
                        if (monster[y][x] == '#' && Matrix[targetY + y, targetX + x] != '#' && Matrix[targetY + y, targetX + x] != 'O')
                            return false;
                    }
                }

                return true;
            }

            private void MarkMonster(int targetY, int targetX, string[] monster)
            {
                for (int y = 0; y < monster.Length; y++)
                {
                    for (int x = 0; x < monster[y].Length; x++)
                    {
                        if (monster[y][x] == '#')
                            Matrix[targetY + y, targetX + x] = 'O';
                    }
                }
            }

            public int NumUnmarked()
            {
                int num = 0;
                for (int y = 0; y < _dimension; y++)
                {
                    for (int x = 0; x < _dimension; x++)
                    {
                        if (Matrix[y, x] == '#')
                            num++;
                    }
                }

                return num;
            }
        }
    }
}