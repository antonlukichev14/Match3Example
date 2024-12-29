using Match3Example.Inputs;
using Match3Example.Scenes.GameSceneBehavior;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.GameObjects
{
    class GameField
    {
        public Cell[,] cells;

        public Element[] elements;

        private Vector2 transormOffset;

        public UserInterfaceCollider collider;

        public GameField(Vector2i cellsSize, Vector2 transormOffset, Element[] elements, UserInterfaceCollider collider)
        {
            this.collider = collider;
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
            ApplyCellsByIndex(cellsByIndex);
            ConsolePrintIndex();
        }

        int tr = 100;
        private int[,] GenerateStartCells()
        {
            int[,] cellsByIndex = Match3.Randomize(new Vector2i(cells.GetLength(0), cells.GetLength(1)), 0, Element.elementsCount);

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
            for (int i = cells.GetLength(1) - 1; i > -1; i--)
            {
                for (int j = cells.GetLength(0) - 1; j > -1; j--)
                {
                    Console.Write(cells[j, i].element != null ? cells[j, i].element.ID : "#");
                }
                Console.Write("\n");
            }
        }

        public void ApplyCellsByIndex(int[,] cellsByIndex)
        {
            for (int i = 0; i < cells.GetLength(0); i++)
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

        public int[,] GetCellsByIndex()
        {
            int[,] indexCells = new int[cells.GetLength(0), cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    indexCells[i, j] = cells[i, j].element != null ? cells[i, j].element.ID : -1;
                }
            }

            return indexCells;
        }
    }
}
