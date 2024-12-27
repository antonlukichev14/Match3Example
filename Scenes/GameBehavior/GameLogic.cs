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

            if (game.state == GameState.Interact)
                game.timer -= (float)game.deltaTime;

            if (game.timer <= 0)
                Viewport.Instance.SetCurrentScene(new EndScene());
        }

        public void OnMouseMove()
        {
            if(game.cells.collider.ScreenPointCollison(game.mainCamera, MouseInput.MousePosition))
            {
                Vector2 AABBpoint1 = game.cells.collider.GetScreenAABBpoint(game.mainCamera, 0);
                Vector2 AABBpoint2 = game.cells.collider.GetScreenAABBpoint(game.mainCamera, 1);

                float a = AABBpoint2.X - AABBpoint1.X;
                float a8 = a / 8;

                int aa = (int)Math.Floor((MouseInput.MousePosition.X - AABBpoint1.X) / a8);

                float b = AABBpoint1.Y - AABBpoint2.Y;
                float b8 = b / 8;

                int bb = (int)Math.Floor((MouseInput.MousePosition.Y - AABBpoint2.Y) / b8);

                game.hoverCell.X = 7 - aa;
                game.hoverCell.Y = 7 - bb;
            }

            
        }
    }
}
