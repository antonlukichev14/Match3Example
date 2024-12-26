using Match3Example.Inputs;
using Match3Example.Scenes.GameBehavior.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace Match3Example.Scenes.GameBehavior
{
    class GameLogic
    {
        private Game game;

        private GameStateLogic[] gameStates;

        public GameLogic(Game game) 
        {
            this.game = game;

            float gravitySpeed = 0.25f;
            GenerateNewElements gne = new GenerateNewElements(game, gravitySpeed);
            SwitchElements se = new SwitchElements(game, 4f);
            ElementsFall ef = new ElementsFall(game, gravitySpeed);
            Match3Check m3c = new Match3Check(game, ef, gne);

            gameStates = new GameStateLogic[]
            {
                new InteractWithField(game, se),
                se,
                m3c,
                se,
                ef,
                m3c,
                gne
            };
        }

        public void OnUpdate()
        {
            gameStates[(int)game.state].Update();
        }

        public void OnMouseMove()
        {
            Vector3 mouseWorldPos = MouseInput.ScreenToCameraOrtWorldPosition(game.mainCamera, 1);
            mouseWorldPos += new Vector3(4f, 0, 4f);

            if (mouseWorldPos.X > 0 && mouseWorldPos.X < 8 && mouseWorldPos.Z > 0 && mouseWorldPos.Z < 8)
            {
                game.hoverCell.X = (int)Math.Ceiling(mouseWorldPos.X) - 1;
                game.hoverCell.Y = (int)Math.Ceiling(mouseWorldPos.Z) - 1;
            }
            else
            {
                game.hoverCell.X = -1;
                game.hoverCell.Y = -1;
            }
        }
    }
}
