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

        public GameField cells;

        public Vector2i hoverCell = new Vector2i(-1, -1);
        public Vector2i selectedCell = new Vector2i(-1, -1);

        public GameState state = GameState.Interact;

        public double timer = 60;
        public int score = 0;

        public Game():base(20f) 
        {
            
        }

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

            cells = new GameField(new Vector2i(8, 8), new Vector2(-3.5f, -3.5f), elements, new Collider2DAABB(4, -4, 4, -4));

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
