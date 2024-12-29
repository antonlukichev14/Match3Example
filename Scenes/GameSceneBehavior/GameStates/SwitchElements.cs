using Match3Example.GameObjects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.Scenes.GameBehavior.GameStates
{
    class SwitchElements : GameStateLogic
    {
        GameScene game;

        private static double time;
        private static Vector2i cell1;
        private static Vector2i cell2;
        private static Vector3 cell1pos;
        private static Vector3 cell2pos;

        public float animationSpeed;

        private static Cell[,] cells;

        public SwitchElements(GameScene game, float animationSpeed)
        {
            cells = game.GameField.cells;
            this.game = game;
            this.animationSpeed = animationSpeed;
        }

        public override void Update()
        {
            if (time + game.deltaTime * animationSpeed >= 1)
            {
                time = 0;

                Cell _cell = cells[cell1.X, cell1.Y];
                cells[cell1.X, cell1.Y] = cells[cell2.X, cell2.Y];
                cells[cell2.X, cell2.Y] = _cell;

                cells[cell1.X, cell1.Y].transforms.position = cell2pos;
                cells[cell2.X, cell2.Y].transforms.position = cell1pos;

                if (game.state == GameState.SwitchElementsREVERSE)
                    game.state = GameState.Interact;
                else
                    game.state = GameState.Match3CheckSE;

                return;
            }

            time += game.deltaTime * animationSpeed;
            cells[cell1.X, cell1.Y].transforms.position = Vector3.Lerp(cell2pos, cell1pos, (float)time);
            cells[cell2.X, cell2.Y].transforms.position = Vector3.Lerp(cell1pos, cell2pos, (float)time);
        }

        public void Start(Vector2i selectedCell, Vector2i hoverCell)
        {
            time = 0;
            cell1 = new Vector2i(selectedCell.X, selectedCell.Y);
            cell2 = new Vector2i(hoverCell.X, hoverCell.Y);
            cell1pos = cells[hoverCell.X, hoverCell.Y].transforms.position;
            cell2pos = cells[selectedCell.X, selectedCell.Y].transforms.position;
        }
    }
}
