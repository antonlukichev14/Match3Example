using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.Scenes.GameBehavior.GameStates
{
    internal class ElementsFall : GameStateLogic
    {
        private Game game;

        public float graviteSpeed;

        int[,] elementsFall;
        List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)> cells;

        public ElementsFall(Game game, float graviteSpeed)
        {
            this.game = game;
            this.graviteSpeed = graviteSpeed;
        }

        public override void Update()
        {
            List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)> deleteObjects = new List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)>();
            int efa_cells_length = cells.Count;

            if (efa_cells_length == 0)
            {
                game.state = GameState.Match3Check;
                return;
            }

            for (int i = 0; i < efa_cells_length; i++)
            {
                (Cell cCell, Vector2i cIndex, float floorPos, float gValue) sCell = cells[i];
                sCell.gValue += (float)(graviteSpeed * game.deltaTime);
                if (sCell.cCell.transforms.position.Z - sCell.gValue <= sCell.floorPos)
                {
                    sCell.cCell.transforms.position.Z = sCell.floorPos;
                    Vector2i newIndex = new Vector2i(sCell.cIndex.X, sCell.cIndex.Y - elementsFall[sCell.cIndex.X, sCell.cIndex.Y]);

                    Cell _cell = game.GameField.cells[newIndex.X, newIndex.Y];
                    _cell.transforms.position = game.GameField.GetNewPositionByIndex(sCell.cIndex);
                    game.GameField.cells[newIndex.X, newIndex.Y] = sCell.cCell;
                    game.GameField.cells[sCell.cIndex.X, sCell.cIndex.Y] = _cell;

                    cells[i] = sCell;
                    deleteObjects.Add(sCell);
                }
                else
                {
                    sCell.cCell.transforms.position.Z -= sCell.gValue;
                    cells[i] = sCell;
                }
            }

            for (int i = 0; i < deleteObjects.Count; i++)
            {
                if (!cells.Remove(deleteObjects[i]))
                {
                    throw new Exception();
                }
            }
        }

        public void Start()
        {
            elementsFall = Match3.ElementsFall(game.GameField);
            InstantiateList();
        }

        void InstantiateList()
        {
            cells = new List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)>();

            for (int i = 0; i < elementsFall.GetLength(0); i++)
            {
                for (int j = 0; j < elementsFall.GetLength(1); j++)
                {
                    if (elementsFall[i, j] != 0)
                    {
                        Cell cCell = game.GameField.cells[i, j];
                        Vector2i cIndex = new Vector2i(i, j);
                        float floorPos = game.GameField.GetNewPositionByIndex(new Vector2i(cIndex.X, cIndex.Y - elementsFall[cIndex.X, cIndex.Y])).Z;
                        cells.Add((cCell, cIndex, floorPos, 0));
                    }
                }
            }
        }
    }
}
