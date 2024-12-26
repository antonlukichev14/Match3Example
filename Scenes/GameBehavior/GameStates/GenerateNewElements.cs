using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.Scenes.GameBehavior.GameStates
{
    class GenerateNewElements : GameStateLogic
    {
        private Game game;

        public float graviteSpeed;

        List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)> gne_cells;

        public GenerateNewElements(Game game, float graviteSpeed)
        {
            this.game = game;
            this.graviteSpeed = graviteSpeed;
        }

        public override void Update()
        {
            List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)> deleteObjects = new List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)>();
            int gne_cells_length = gne_cells.Count;

            if (gne_cells_length == 0)
            {
                game.state = GameState.Match3Check;
                return;
            }

            for (int i = 0; i < gne_cells_length; i++)
            {
                (Cell cCell, Vector2i cIndex, float floorPos, float gValue) sCell = gne_cells[i];
                sCell.gValue += (float)(graviteSpeed * game.deltaTime);

                if (sCell.cCell.transforms.position.Z - sCell.gValue <= sCell.floorPos)
                {
                    sCell.cCell.transforms.position.Z = sCell.floorPos;
                    gne_cells[i] = sCell;
                    deleteObjects.Add(sCell);
                }
                else
                {
                    sCell.cCell.transforms.position.Z -= sCell.gValue;
                    gne_cells[i] = sCell;
                }
            }

            for (int i = 0; i < deleteObjects.Count; i++)
            {
                if (!gne_cells.Remove(deleteObjects[i]))
                {
                    throw new Exception();
                }
            }
        }

        public void Start(bool[,] gne_empty) 
        {
            int minY = 8;
            gne_cells = new List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)>();

            for (int j = gne_empty.GetLength(1) - 1; j > -1; j--)
            {
                for (int i = 0; i < gne_empty.GetLength(0); i++)
                {
                    if (gne_empty[i, j]) minY = j;
                }
            }

            for (int j = 0; j < gne_empty.GetLength(1); j++)
            {
                for (int i = 0; i < gne_empty.GetLength(0); i++)
                {
                    if (gne_empty[i, j])
                    {
                        game.cells.cells[i, j].transforms.position.Z = game.cells.GetNewPositionByIndex(new Vector2i(i, j + gne_empty.GetLength(1) + 2 - minY)).Z;
                        gne_cells.Add((game.cells.cells[i, j], new Vector2i(i, j), game.cells.GetNewPositionByIndex(new Vector2i(i, j)).Z, 0));
                    }
                }
            }

            Console.WriteLine(minY);
            game.state = GameState.Interact;

            int[,] newcells = new int[game.cells.cells.GetLength(0), game.cells.cells.GetLength(1)];

            for (int i = 0; i < newcells.GetLength(0); i++)
            {
                for (int j = 0; j < newcells.GetLength(1); j++)
                {
                    newcells[i, j] = -1;
                }
            }

            newcells = Match3.MaskRandomize(newcells, gne_empty, 0, game.cells.elements.Length);

            for (int i = 0; i < newcells.GetLength(0); i++)
            {
                for (int j = 0; j < newcells.GetLength(1); j++)
                {
                    if (newcells[i, j] != -1)
                        game.cells.cells[i, j].element = game.cells.elements[newcells[i, j]];
                }
            }
        }
    }
}
