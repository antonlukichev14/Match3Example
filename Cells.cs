using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    class Cells
    {
        public Cell[,] cells;

        public Element[] elements;

        private Vector2 transormOffset;

        public Cells(Vector2i cellsSize, Vector2 transormOffset, Element[] elements)
        {
            this.transormOffset = transormOffset;

            cells = new Cell[cellsSize.X, cellsSize.Y];
            this.elements = elements;

            for (int i = 0; i < cellsSize.X; i++)
            {
                for (int j = 0; j < cellsSize.Y; j++)
                {
                    cells[i, j] = new Cell(new Vector3(i + transormOffset.X, 0, j + transormOffset.Y));
                }
            }

            int[,] cellsByIndex = GenerateStartCells();
            ApplyByIndex(cellsByIndex);
            ConsolePrintIndex();
        }

        int tr = 100;
        private int[,] GenerateStartCells()
        {
            int[,] cellsByIndex = Match3.Randomize(cells, 0, Element.elementsCount);

            int i = 0;
            while (!Match3.IsWithoutDelete(cellsByIndex))
            {
                i++;
                bool[,] mask = Match3.CheckDelete(cellsByIndex);

                Match3.MaskRandomize(cellsByIndex, mask, 0, Element.elementsCount);

                if (i > tr) break;
            }

            if (i > tr)
                cellsByIndex = GenerateStartCells();

            return cellsByIndex;
        }

        public void ConsolePrintIndex()
        {
            for(int i = cells.GetLength(1) - 1; i > -1; i--)
            {
                for (int j = cells.GetLength(0) - 1; j > -1; j--)
                {
                    Console.Write(cells[j, i].element != null ? cells[j, i].element.ID : "#");
                }
                Console.Write("\n");
            }
        }

        public void ApplyByIndex(int[,] cellsByIndex)
        {
            for(int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j].element = elements[cellsByIndex[i, j]];
                }
            }
        }

        public Vector3 GetNewPositionByIndex(Vector2i index)
        {
            return new Vector3(index.X + transormOffset.X, 0, index.Y + transormOffset.Y);
        }
    }
}
