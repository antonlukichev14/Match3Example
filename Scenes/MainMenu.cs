﻿using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK_Test_Lighting_01;
using Match3Example.Inputs;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Match3Example.Scenes
{
    class MainMenu : Scene
    {
        Shader defaultShader;
        Shader textShader;

        RenderObject ButtonObject;
        Texture buttonTexture;
        Texture buttonTextureHover;

        Vector2 buttonColliderVertical;
        Vector2 buttonColliderHorizontal;

        bool buttonHover = false;

        public MainMenu() : base(50)
        {
            GL.ClearColor(194f / 255f, 153f / 255f, 121f / 255f, 1.0f);

            TextRender textRender = new TextRender(Path.GetAssetPath("Fonts/Impact.ttf"));
            textShader = new Shader(Path.GetAssetPath("Shaders/Text.vert"), Path.GetAssetPath("Shaders/Text.frag"));

            defaultShader = new Shader(Path.GetAssetPath("Shaders/Default.vert"), Path.GetAssetPath("Shaders/Default.frag"));

            Mesh buttonMesh = new Mesh(AssimpLoader.GetMeshFromFile(Path.GetAssetPath("Models/button.obj")));
            buttonTexture = new Texture(Path.GetAssetPath("Textures/button.png"), new Vector2(1, 1));
            buttonTextureHover = new Texture(Path.GetAssetPath("Textures/button_hover.png"), Vector2.One);
            ButtonObject = new RenderObject(buttonMesh, buttonTexture, new Transforms(new Vector3(0, -1, 0), new Vector3(0, 0, 0), new Vector3(4, 4, 4)));
            
            Vector2 buttonColliderPosition = new Vector2(0, 0.25f);
            Vector2 buttonColliderScale = new Vector2(4, 1.5f);

            buttonColliderVertical = new Vector2(buttonColliderPosition.Y + buttonColliderScale.Y, buttonColliderPosition.Y - buttonColliderScale.Y);
            buttonColliderHorizontal = new Vector2(buttonColliderPosition.X + buttonColliderScale.X, buttonColliderPosition.X - buttonColliderScale.X);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        public override void OnMouseMove(MouseMoveEventArgs e)
        {
            Vector3 mousePos = MouseInput.ScreenToCameraOrtWorldPosition(mainCamera, 0);

            if (mousePos.X < buttonColliderHorizontal.X && mousePos.X > buttonColliderHorizontal.Y && mousePos.Z < buttonColliderVertical.X && mousePos.Z > buttonColliderVertical.Y)
            {
                ButtonObject.texture = buttonTextureHover;
                buttonHover = true;
            }
            else
            {
                ButtonObject.texture = buttonTexture;
                buttonHover = false;
            }
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            defaultShader.Use();
            mainCamera.Use(defaultShader);

            ButtonObject.Render(defaultShader);
            TextRender.Instance.RenderAlignCenter(textShader, mainCamera, "PLAY", new Vector2(0, -1f), 2f, Vector3.One);
        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (buttonHover && MouseInput.state.IsButtonPressed(MouseButton.Left))
            {
                Viewport.Instance.SetCurrentScene(new Game());
            }
        }
    }
}
