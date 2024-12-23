﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK_Test_Lighting_01;
using System;
using System.Collections.Generic;
using System.Linq;
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

        RenderObject GameField;

        Cell[,] cells = new Cell[8, 8];

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

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    cells[i, j] = new Cell(new Vector3(i - 3.5f, 0, j - 3.5f), elements[random.Next(0, 5)]);
                }
            }

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

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    cells[i, j].Render(defaultShader);
                }
            }

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