using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    static class Match3
    {
        public static bool[,] CheckDelete(GameField gameField)
        {
            return CheckDelete(gameField.GetCellsByIndex());
        }

        public static bool[,] CheckDelete(int[,] cells)
        {
            bool[,] deleteElements = new bool[cells.GetLength(0), cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    int currentIDelement = cells[i, j];

                    if (currentIDelement == -1)
                        continue;

                    int xCount = 1;
                    int yCount = 1;

                    if (i + 1 < cells.GetLength(0))
                    {
                        if (cells[i + 1, j] == currentIDelement)
                        {
                            xCount++;
                            while (i + xCount < cells.GetLength(0) && cells[i + xCount, j] == currentIDelement)
                            {
                                xCount++;
                            }
                        }
                    }

                    if (j + 1 < cells.GetLength(1))
                    {
                        if (cells[i, j + 1] == currentIDelement)
                        {
                            yCount++;
                            while (j + yCount < cells.GetLength(1) && cells[i, j + yCount] == currentIDelement)
                            {
                                yCount++;
                            }
                        }
                    }

                    if (xCount > 2)
                    {
                        for (int m = 0; m < xCount; m++)
                        {
                            deleteElements[i + m, j] = true;
                        }
                    }

                    if (yCount > 2)
                    {
                        for (int m = 0; m < yCount; m++)
                        {
                            deleteElements[i, j + m] = true;
                        }
                    }
                }
            }

            return deleteElements;
        }

        public static bool IsWithoutDelete(GameField gameField)
        {
            return IsWithoutDelete(gameField.GetCellsByIndex());
        }

        public static bool IsWithoutDelete(int[,] cells)
        {
            bool[,] deleteElements = CheckDelete(cells);

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (deleteElements[i, j] == true) return false;
                }
            }

            return true;
        }

        public static int[,] MaskRandomize(GameField gameField, bool[,] mask, int minValue, int maxValue)
        {
            return MaskRandomize(gameField.GetCellsByIndex(), mask, minValue, maxValue);
        }

        public static int[,] MaskRandomize(int[,] cells, bool[,] mask, int minValue, int maxValue)
        {
            Random random = new Random();

            int[,] newCellsByIndex = new int[cells.GetLength(0), cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (mask[i, j] == false)
                    {
                        newCellsByIndex[i, j] = cells[i, j];
                    }
                    else
                    {
                        newCellsByIndex[i, j] = random.Next(minValue, maxValue);
                    }
                }
            }

            return newCellsByIndex;
        }

        public static int[,] Randomize(Vector2i GameFieldSize, int minValue, int maxValue)
        {
            Random random = new Random();

            int[,] newCellsByIndex = new int[GameFieldSize.X, GameFieldSize.Y];

            for (int i = 0; i < GameFieldSize.X; i++)
            {
                for (int j = 0; j < GameFieldSize.Y; j++)
                {
                    newCellsByIndex[i, j] = random.Next(minValue, maxValue);
                }
            }

            return newCellsByIndex;
        }

        public static int[,] ElementsFall(GameField gameField)
        {
            return ElementsFall(gameField.GetCellsByIndex());
        }

        public static int[,] ElementsFall(int[,] cells)
        {
            Vector2i cellSize = new Vector2i(cells.GetLength(0), cells.GetLength(1));
            bool[,] cellNotEmpty = new bool[cellSize.X, cellSize.Y];
            int[,] elementsFall = new int[cellSize.X, cellSize.Y];

            for (int i = 0; i < cellSize.X; i++)
            {
                for (int j = 0; j < cellSize.Y; j++)
                {
                    if (cells[i, j] != -1)
                    {
                        int mincell = cellSize.Y - 1;

                        for (int jy = cellSize.Y - 1; jy > -1; jy--)
                        {
                            if (!cellNotEmpty[i, jy] && jy < mincell)
                            {
                                mincell = jy;
                            }
                        }

                        cellNotEmpty[i, mincell] = true;
                        elementsFall[i, j] = (j - mincell);
                    }
                }
            }

            return elementsFall;
        }

        public static (bool status, bool[,] empty) CheckEmptyCells(GameField gameField)
        {
            return CheckEmptyCells(gameField.GetCellsByIndex());
        }

        public static (bool status, bool[,] empty) CheckEmptyCells(int[,] cells)
        {
            bool status = false;
            bool[,] empty = new bool[cells.GetLength(0), cells.GetLength(1)];

            for(int i = 0; i < cells.GetLength(0); i++)
            {
                for(int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j] == -1)
                    {
                        status = true;
                        empty[i, j] = true;
                    }
                }
            }

            return (status, empty);
        }
    }
}
