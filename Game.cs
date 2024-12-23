using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK_Test_Lighting_01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    class Game : GameWindow
    {
        public static Game instance;

        Camera camera;

        Shader defaultShader;

        RenderObject GameField;

        public float deltaTime;

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
            GameField = new RenderObject(gamefieldMesh, gamefieldTexture, Transforms.Default());

            defaultShader = new Shader(Path.GetAssetPath("Shaders/Default.vert"), Path.GetAssetPath("Shaders/Default.frag"));
            camera = new Camera(20f);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            defaultShader.Use();
            camera.Use(defaultShader);

            GameField.Render(defaultShader);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            camera.SetProjection();

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
