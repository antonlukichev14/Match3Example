using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK_Test_Lighting_01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Match3Example
{
    class Game : GameWindow
    {
        public static Game instance;

        Camera camera;

        Shader defaultShader;
        Shader hoverShader;
        Shader selectedShader;

        RenderObject GameField;

        Cells cells;

        public double deltaTime;
        public double currentTime;

        public Vector2i hoverCell = new Vector2i(-1, -1);
        public Vector2i selectedCell = new Vector2i(-1, -1);

        public GameState gameState = GameState.Interact;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title, NumberOfSamples = 4, Vsync = VSyncMode.On }) { }

        protected override void OnLoad()
        {
            base.OnLoad();

            //BASE SETTINGS
            instance = this;
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor((194f / 255f), (153f / 255f), (121f / 255f), 1.0f);

            Texture gamefieldTexture = new Texture(Path.GetAssetPath("Textures/cell.png"), Vector2.One);
            Mesh gamefieldMesh = new Mesh(AssimpLoader.GetMeshFromFile(Path.GetAssetPath("Models/gamefield.obj")));
            GameField = new RenderObject(gamefieldMesh, gamefieldTexture, new Transforms(new Vector3(0.0f, -1.0f, 0.0f)));

            Mesh quadMesh = new Mesh(AssimpLoader.GetMeshFromFile(Path.GetAssetPath("Models/quad.obj")));

            Texture element1Texture = new Texture(Path.GetAssetPath("Textures/element1.png"), Vector2.One);
            element1Texture.SetTextureWrapping((int)TextureWrapMode.ClampToEdge);
            Texture element2Texture = new Texture(Path.GetAssetPath("Textures/element2.png"), Vector2.One);
            element2Texture.SetTextureWrapping((int)TextureWrapMode.ClampToEdge);
            Texture element3Texture = new Texture(Path.GetAssetPath("Textures/element3.png"), Vector2.One);
            element3Texture.SetTextureWrapping((int)TextureWrapMode.ClampToEdge);
            Texture element4Texture = new Texture(Path.GetAssetPath("Textures/element4.png"), Vector2.One);
            element4Texture.SetTextureWrapping((int)TextureWrapMode.ClampToEdge);
            Texture element5Texture = new Texture(Path.GetAssetPath("Textures/element5.png"), Vector2.One);
            element5Texture.SetTextureWrapping((int)TextureWrapMode.ClampToEdge);

            Element[] elements = new Element[5];
            elements[0] = new Element(quadMesh, element1Texture);
            elements[1] = new Element(quadMesh, element2Texture);
            elements[2] = new Element(quadMesh, element3Texture);
            elements[3] = new Element(quadMesh, element4Texture);
            elements[4] = new Element(quadMesh, element5Texture);

            Random random = new Random();

            cells = new Cells(new Vector2i(8, 8), new Vector2(-3.5f, -3.5f), elements);

            defaultShader = new Shader(Path.GetAssetPath("Shaders/Default.vert"), Path.GetAssetPath("Shaders/Default.frag"));
            hoverShader = new Shader(Path.GetAssetPath("Shaders/Hover.vert"), Path.GetAssetPath("Shaders/Hover.frag"));
            selectedShader = new Shader(Path.GetAssetPath("Shaders/Selected.vert"), Path.GetAssetPath("Shaders/Selected.frag"));

            camera = new Camera(20f);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            defaultShader.Use();
            camera.Use(defaultShader);

            GameField.Render(defaultShader);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(gameState == GameState.Interact && selectedCell.X == i && selectedCell.Y == j)
                    {
                        selectedShader.Use();
                        camera.Use(selectedShader);
                        cells.cells[i, j].Render(selectedShader);
                        defaultShader.Use();
                    }
                    else if(gameState == GameState.Interact && hoverCell.X == i && hoverCell.Y == j)
                    {
                        hoverShader.Use();
                        camera.Use(hoverShader);
                        hoverShader.SetUniformFloat("time", (float)currentTime);
                        cells.cells[i, j].Render(hoverShader);
                        defaultShader.Use();
                    }
                    else
                    {
                        cells.cells[i, j].Render(defaultShader);
                    }
                }
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            deltaTime = args.Time;
            currentTime += deltaTime;

            switch (gameState)
            {
                case GameState.Interact:
                    InteractWithField();
                    break;
                case GameState.SwitchElements:
                    SwitchElements(3, false);
                    break;
                case GameState.Match3CheckSE:
                    Match3CheckSE();
                    break;
                case GameState.SwitchElementsREVERSE:
                    SwitchElements(3, true);
                    break;
                case GameState.ElementsFall:
                    ElementsFallAnimation(0.1f);
                    break;
            }
        }

        void InteractWithField()
        {
            if (MouseState.IsButtonPressed(MouseButton.Left))
            {
                if(selectedCell.X == -1 && selectedCell.Y == -1)
                {
                    selectedCell.X = hoverCell.X; selectedCell.Y = hoverCell.Y;
                }
                else
                {
                    if(Math.Abs(hoverCell.X - selectedCell.X) < 2 && Math.Abs(hoverCell.Y - selectedCell.Y) < 2 && ((hoverCell.X != selectedCell.X && hoverCell.Y == selectedCell.Y) || (hoverCell.X == selectedCell.X && hoverCell.Y != selectedCell.Y)))
                    {
                        se_time = 0;
                        se_cell1 = new Vector2i(selectedCell.X, selectedCell.Y);
                        se_cell2 = new Vector2i(hoverCell.X, hoverCell.Y);
                        se_cell1pos = cells.cells[hoverCell.X, hoverCell.Y].transforms.position;
                        se_cell2pos = cells.cells[selectedCell.X, selectedCell.Y].transforms.position;

                        gameState = GameState.SwitchElements;
                    }

                    selectedCell.X = -1;
                    selectedCell.Y = -1;
                }
            }
        }

        double se_time;
        private Vector2i se_cell1;
        private Vector2i se_cell2;
        private Vector3 se_cell1pos;
        private Vector3 se_cell2pos;
        void SwitchElements(float animSpeed, bool toInteract)
        {
            if(se_time + deltaTime * animSpeed >= 1)
            {
                se_time = 0;

                Cell _cell = cells.cells[se_cell1.X, se_cell1.Y];
                cells.cells[se_cell1.X, se_cell1.Y] = cells.cells[se_cell2.X, se_cell2.Y];
                cells.cells[se_cell2.X, se_cell2.Y] = _cell;

                cells.cells[se_cell1.X, se_cell1.Y].transforms.position = se_cell2pos;
                cells.cells[se_cell2.X, se_cell2.Y].transforms.position = se_cell1pos;

                if(toInteract)
                    gameState = GameState.Interact;
                else
                    gameState = GameState.Match3CheckSE;

                return;
            }

            se_time += deltaTime * animSpeed;
            cells.cells[se_cell1.X, se_cell1.Y].transforms.position = Vector3.Lerp(se_cell2pos, se_cell1pos, (float)se_time);
            cells.cells[se_cell2.X, se_cell2.Y].transforms.position = Vector3.Lerp(se_cell1pos, se_cell2pos, (float)se_time);
        }

        void Match3CheckSE()
        {
            if (Match3.IsWithoutDelete(cells.cells))
            {
                gameState = GameState.SwitchElementsREVERSE;
                return;
            }

            bool[,] deleteCells = Match3.CheckDelete(cells.cells);

            for (int i = 0; i < deleteCells.GetLength(0); i++)
            {
                for (int j = 0; j < deleteCells.GetLength(1); j++)
                {
                    if (deleteCells[i, j])
                    {
                        cells.cells[i, j].element = null;
                    }
                }
            }

            efa_elementsFall = Match3.ElementsFall(cells.cells);
            efa_InstantiateList();
            gameState = GameState.ElementsFall;
        }

        void efa_InstantiateList()
        {
            efa_cells = new List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)>();

            for (int i = 0; i < efa_elementsFall.GetLength(0); i++)
            {
                for(int j = 0; j <  efa_elementsFall.GetLength(1); j++)
                {
                    if (efa_elementsFall[i, j] != 0)
                    {
                        Cell cCell = cells.cells[i, j];
                        Vector2i cIndex = new Vector2i(i, j);
                        float floorPos = cells.GetNewPositionByIndex(new Vector2i(cIndex.X, cIndex.Y - efa_elementsFall[cIndex.X, cIndex.Y])).Z;
                        efa_cells.Add((cCell, cIndex, floorPos, 0));
                    }
                }
            }
        }

        int[,] efa_elementsFall;
        List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)> efa_cells;
        void ElementsFallAnimation(double graviteSpeed)
        {
            List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)> deleteObjects = new List<(Cell cCell, Vector2i cIndex, float floorPos, float gValue)>();
            int efa_cells_length = efa_cells.Count;

            if (efa_cells_length == 0)
            {
                throw new Exception("New State");
                return;
            }

            for (int i = 0; i < efa_cells_length; i++)
            {
                (Cell cCell, Vector2i cIndex, float floorPos, float gValue) sCell = efa_cells[i];
                sCell.gValue += (float)(graviteSpeed * deltaTime);
                if (sCell.cCell.transforms.position.Z - sCell.gValue <= sCell.floorPos)
                {
                    sCell.cCell.transforms.position.Z = sCell.floorPos;
                    Vector2i newIndex = new Vector2i(sCell.cIndex.X, sCell.cIndex.Y - efa_elementsFall[sCell.cIndex.X, sCell.cIndex.Y]);

                    Cell _cell = cells.cells[newIndex.X, newIndex.Y];
                    cells.cells[newIndex.X, newIndex.Y] = sCell.cCell;
                    cells.cells[sCell.cIndex.X, sCell.cIndex.Y] = _cell;

                    efa_cells[i] = sCell;
                    deleteObjects.Add(sCell);
                }
                else
                {
                    sCell.cCell.transforms.position.Z -= sCell.gValue;
                    efa_cells[i] = sCell;
                }
            }

            for (int i = 0; i < deleteObjects.Count; i++)
            {
                if (!efa_cells.Remove(deleteObjects[i]))
                {
                    throw new Exception();
                }
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            Vector3 mouseWorldPos = ScreenToWorldCoord();
            mouseWorldPos += new Vector3(4f, 0, 4f);
            
            if(mouseWorldPos.X > 0 && mouseWorldPos.X < 8 && mouseWorldPos.Z > 0 && mouseWorldPos.Z < 8)
            {
                hoverCell.X = (int)Math.Ceiling(mouseWorldPos.X) - 1;
                hoverCell.Y = (int)Math.Ceiling(mouseWorldPos.Z) - 1;
            }
            else
            {
                hoverCell.X = -1;
                hoverCell.Y = -1;
            }
        }

        Vector3 ScreenToWorldCoord()
        {
            float mouseNDC_X = (MousePosition.X * 2) / ClientSize.X - 1;
            float mouseNDC_Y = 1 - (MousePosition.Y * 2) / ClientSize.Y;
            Vector3 mouseNDC = new Vector3(mouseNDC_X, mouseNDC_Y, 1);

            Matrix4 invProjectionView = Matrix4.Invert(camera.projection * camera.view);

            Vector4 clipCoords = new Vector4(mouseNDC.X, mouseNDC.Y, -1.0f, 1.0f);
            Vector4 worldCoords = invProjectionView * clipCoords;
            worldCoords /= worldCoords.W;

            Vector3 worldPosition = new Vector3(worldCoords.X, worldCoords.Y, worldCoords.Z);
            return worldPosition;
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            camera.SetProjection();

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }

    public enum GameState
    {
        Interact,
        SwitchElements,
        Match3CheckSE,
        SwitchElementsREVERSE,
        ElementsFall
    }
}
