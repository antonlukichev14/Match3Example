using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common;
using Assimp;

namespace Match3Example.Scenes.GameBehavior
{
    class GameRender
    {
        private Game game;
        public Vector4 clearColor = new Vector4(194f / 255f, 153f / 255f, 121f / 255f, 1.0f);

        Shader defaultShader;
        Shader hoverShader;
        Shader selectedShader;
        Shader textShader;

        public GameRender(Game game)
        {
            this.game = game;
            SetRenderingParameters();
            LoadShaders();
        }

        public void SetRenderingParameters()
        {
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
        }

        public void LoadShaders()
        {
            defaultShader = new Shader(Path.GetAssetPath("Shaders/Default.vert"), Path.GetAssetPath("Shaders/Default.frag"));
            hoverShader = new Shader(Path.GetAssetPath("Shaders/Hover.vert"), Path.GetAssetPath("Shaders/Hover.frag"));
            selectedShader = new Shader(Path.GetAssetPath("Shaders/Selected.vert"), Path.GetAssetPath("Shaders/Selected.frag"));
            textShader = new Shader(Path.GetAssetPath("Shaders/Text.vert"), Path.GetAssetPath("Shaders/Text.frag"));
        }

        public void RenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            defaultShader.Use();
            game.mainCamera.Use(defaultShader);

            game.GameFieldObject.Render(defaultShader);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.state == GameState.Interact && game.selectedCell.X == i && game.selectedCell.Y == j)
                    {
                        selectedShader.Use();
                        game.mainCamera.Use(selectedShader);
                        game.cells.cells[i, j].Render(selectedShader);
                        defaultShader.Use();
                    }
                    else if (game.state == GameState.Interact && game.hoverCell.X == i && game.hoverCell.Y == j)
                    {
                        hoverShader.Use();
                        game.mainCamera.Use(hoverShader);
                        hoverShader.SetUniformFloat("time", (float)game.currentTime);
                        game.cells.cells[i, j].Render(hoverShader);
                        defaultShader.Use();
                    }
                    else
                    {
                        game.cells.cells[i, j].Render(defaultShader);
                    }
                }
            }

            TextRender.Instance.Render(textShader, game.mainCamera, $"TIME: {(int)Math.Floor(game.timer)}", new Vector2(-8, 2), 0.7f, game.state == GameState.Interact ? Vector3.One : new Vector3(1, 0, 0));
            TextRender.Instance.Render(textShader, game.mainCamera, $"SCORE: {game.score}", new Vector2(-8, -1), 0.7f, Vector3.One);
        }
    }
}
