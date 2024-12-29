using OpenTK.Windowing.Common;

namespace Match3Example
{
    abstract class Scene
    {
        public Camera mainCamera { get; set; }
        private float mainCameraFOV;

        public double deltaTime; //Время между кадрами. Time between frames
        public double currentTime; //Общее время. Total time

        public Scene(float mainCameraFOV) 
        {
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

        public virtual void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            mainCamera.SetProjection();
        }
    }
}
