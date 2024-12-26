using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

using Match3Example.Inputs;

namespace Match3Example
{
    //Класс, отвечающий за работу с окном приложения
    class Viewport:GameWindow
    {
        private Scene _currentScene;

        public static Viewport Instance { get; set; }
        public Scene currentScene { get { return _currentScene; } }

        private bool _loaded = false;

        public Viewport(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() 
        { Size = (width, height), Title = title, NumberOfSamples = 4, Vsync = VSyncMode.On }) 
        { 

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Instance = this;

            if (currentScene != null)
                currentScene.OnLoad();

            _loaded = true;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            if(currentScene != null)
                currentScene.OnRenderFrame(args);
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (currentScene != null)
                currentScene.OnUpdateFrame(args);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (currentScene != null)
                currentScene.OnMouseMove(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            if (currentScene != null)
                currentScene.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        public void SetCurrentScene(Scene scene)
        {
            _currentScene = scene;

            if(_loaded)
                _currentScene.OnLoad();
        }
    }
}
