using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.Scenes
{
    class MainMenu : Scene
    {
        Shader textShader;

        public MainMenu(Viewport viewport) : base(viewport, 50)
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);

            TextRender textRender = new TextRender(Path.GetAssetPath("Fonts/Impact.ttf"), "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдезийклмнопрстуфхцчшщъыьэюяАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789!@#$%^&*()-_=+[]{};:',.<>?/|`~");
            textShader = new Shader(Path.GetAssetPath("Shaders/Text.vert"), Path.GetAssetPath("Shaders/Text.frag"));

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        public override void OnMouseMove(MouseMoveEventArgs e)
        {
            
        }

        public override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            TextRender.Instance.Render(textShader, mainCamera, "Z wa shna ЛЕГО", Vector2.Zero, 0.01f, Vector3.Zero);
        }
        public override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            
        }
    }
}
