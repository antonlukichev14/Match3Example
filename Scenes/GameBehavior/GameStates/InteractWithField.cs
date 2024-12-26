using Match3Example.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.Scenes.GameBehavior.GameStates
{
    class InteractWithField : GameStateLogic
    {
        private Game game;
        private SwitchElements switchElements;

        public InteractWithField(Game game, SwitchElements switchElements)
        {
            this.game = game;
            this.switchElements = switchElements;
        }

        public override void Update()
        {
            if (MouseInput.state.IsButtonPressed(MouseButton.Left))
            {
                Vector2i selectedCell = game.selectedCell;
                Vector2i hoverCell = game.hoverCell;

                if (selectedCell.X == -1 && selectedCell.Y == -1)
                {
                    selectedCell.X = hoverCell.X; selectedCell.Y = hoverCell.Y;
                }
                else
                {
                    if (Math.Abs(hoverCell.X - selectedCell.X) < 2 && Math.Abs(hoverCell.Y - selectedCell.Y) < 2 && (hoverCell.X != selectedCell.X && hoverCell.Y == selectedCell.Y || hoverCell.X == selectedCell.X && hoverCell.Y != selectedCell.Y))
                    {
                        game.state = GameState.SwitchElements;
                        switchElements.Start(selectedCell, hoverCell);
                    }

                    selectedCell.X = -1;
                    selectedCell.Y = -1;
                }

                game.selectedCell = selectedCell;
            }
        }
    }
}
