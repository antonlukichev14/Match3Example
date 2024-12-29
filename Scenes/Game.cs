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

using Match3Example.Scenes.GameBehavior;

namespace Match3Example.Scenes
{
    class Game : Scene
    {
        GameLogic logic;
        GameRender render;

        public RenderObject GameFieldObject;

        public GameField GameField;

        public Vector2i hoverCell = new Vector2i(-1, -1);
        public Vector2i selectedCell = new Vector2i(-1, -1);

        public GameState state = GameState.Interact;

        public double timer = 60;
        public int score = 0;

        public Game() : base(20f) { }

        public override void OnLoad()
        {
            base.OnLoad();
            LoadGameObjects();

            logic = new GameLogic(this);
            render = new GameRender(this);
        }

        void LoadGameObjects()
        {
            Texture gamefieldTexture = new Texture(Path.GetAssetPath("Textures/cell.png"), Vector2.One);
            Mesh gamefieldMesh = new Mesh(AssimpLoader.GetMeshFromFile(Path.GetAssetPath("Models/gamefield.obj")));
            GameFieldObject = new RenderObject(gamefieldMesh, gamefieldTexture, new Transforms(new Vector3(0.0f, -1.0f, 0.0f)));

            Mesh quadMesh = new Mesh(AssimpLoader.GetMeshFromFile(Path.GetAssetPath("Models/quad.obj")));

            Element.ResetElementsCount();
            int elementCount = 5;
            Element[] elements = new Element[elementCount];
            for(int i = 0; i < elementCount; i++)
            {
                Texture elementTexture = new Texture(Path.GetAssetPath($"Textures/element{i + 1}.png"), Vector2.One, (int)TextureWrapMode.ClampToEdge);
                elements[i] = new Element(quadMesh, elementTexture);
            }

            Random random = new Random();

            GameField = new GameField(new Vector2i(8, 8), new Vector2(-3.5f, -3.5f), elements, new Collider2DAABB(4, -4, 4, -4));

            mainCamera.position.X = 3;
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            render.RenderFrame(args);
        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            logic.OnUpdate();
        }

        public override void OnMouseMove(MouseMoveEventArgs e)
        {
            logic.OnMouseMove();
        }
    }
}
