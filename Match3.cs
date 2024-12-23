﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    static class Match3
    {
        public static bool[,] CheckDelete(Cell[,] cells)
        {
            bool[,] deleteElements = new bool[cells.GetLength(0), cells.GetLength(1)];

            for(int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    int currentIDelement = cells[i, j].element.ID;

                    int xCount = 1;
                    int yCount = 1;

                    if(i + 1 < cells.GetLength(0))
                    {
                        if (cells[i + 1, j].element.ID == currentIDelement)
                        {
                            xCount++;
                            while (i + xCount < cells.GetLength(0) && cells[i + xCount, j].element.ID == currentIDelement)
                            {
                                xCount++;
                            }
                        }
                    }
                    
                    if (j + 1 < cells.GetLength(1))
                    {
                        if (cells[i, j + 1].element.ID == currentIDelement)
                        {
                            yCount++;
                            while (j + yCount < cells.GetLength(1) && cells[i, j + yCount].element.ID == currentIDelement)
                            {
                                yCount++;
                            }
                        }
                    }

                    if(xCount > 2)
                    {
                        for(int m = 0; m < xCount; m++)
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

        public static bool[,] CheckDelete(int[,] cells)
        {
            bool[,] deleteElements = new bool[cells.GetLength(0), cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    int currentIDelement = cells[i, j];

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

        public static bool IsWithoutDelete(Cell[,] cells)
        {
            bool[,] deleteElements = CheckDelete(cells);

            for(int i = 0; i < cells.GetLength(0); i++)
            {
                for(int j = 0; j < cells.GetLength(1); j++)
                {
                    if (deleteElements[i, j] == true) return false;
                }
            }

            return true;
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

        public static int[,] MaskRandomize(Cell[,] cells, bool[,] mask, int minValue, int maxValue)
        {
            Random random = new Random();

            int[,] newCellsByIndex = new int[cells.GetLength(0), cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (mask[i, j] == false)
                    {
                        newCellsByIndex[i, j] = cells[i, j].element.ID;
                    }
                    else
                    {
                        newCellsByIndex[i, j] = random.Next(minValue, maxValue);
                    }
                }
            }

            return newCellsByIndex;
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

        public static int[,] Randomize(Cell[,] cells, int minValue, int maxValue)
        {
            Random random = new Random();

            int[,] newCellsByIndex = new int[cells.GetLength(0), cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    newCellsByIndex[i, j] = random.Next(minValue, maxValue);
                }
            }

            return newCellsByIndex;
        }
    }
}
