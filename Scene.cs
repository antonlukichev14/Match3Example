using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    abstract class Scene
    {
        private Viewport viewport;
        public Camera mainCamera { get; set; }
        private float mainCameraFOV;

        public double deltaTime;
        public double currentTime;

        public Scene(Viewport viewport, float mainCameraFOV) 
        {
            this.viewport = viewport;
            this.mainCameraFOV = mainCameraFOV;
        }

        public virtual void OnLoad()
        {
            mainCamera = new Camera(this, mainCameraFOV);
        }

        public abstract void OnRenderFrame(FrameEventArgs args);
        public virtual void OnUpdateFrame(FrameEventArgs args)
        {
            deltaTime = args.Time;
            currentTime += deltaTime;
        }
        public abstract void OnMouseMove(MouseMoveEventArgs e);
        public abstract void OnFramebufferResize(FramebufferResizeEventArgs e);
    }
}
