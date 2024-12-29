using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

namespace Match3Example
{
    //Класс, отвечающий за работу с окном приложения
    //Class responsible for managing the application window
    class Viewport : GameWindow
    {
        //Глобальная ссылка на единственный экземпляр класса
        //Global reference to the single instance of the class
        public static Viewport Instance { get; set; }

        //Текущая активная сцена окна
        //Current active scene of the window
        public Scene CurrentScene { get { return _currentScene; } }
        private Scene _currentScene;

        //Позволяет узнать, была ли уже выполнен метод OnLoad()
        //Allows checking if the OnLoad() function has already been executed
        public bool IsLoaded { get { return _loaded; } }
        private bool _loaded = false;

        //В параметры конструктора класса передаются данные о стартовой ширине и высоте окна, а также название окна, которое используется в левом верхнем углу
        //The constructor parameters of the class receive data about the initial width and height of the window, as well as the window title used in the upper left corner
        public Viewport(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title, NumberOfSamples = 4, Vsync = VSyncMode.On }) 
        { 

        }

        //Метод запускается после this.run()
        //The function is executed after this.run()
        protected override void OnLoad()
        {
            base.OnLoad();

            Instance = this;

            if (CurrentScene != null)
                CurrentScene.OnLoad();

            _loaded = true;
        }

        //Метод отрисовки кадра в графическом цикле приложения
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            if(CurrentScene != null)
                CurrentScene.OnRenderFrame(args);

            SwapBuffers();
        }

        //Метод выполнения логики программы в графическом цикле приложения
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (CurrentScene != null)
                CurrentScene.OnUpdateFrame(args);
        }

        //Метод срабатывает при движение пользователем мышью
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (CurrentScene != null)
                CurrentScene.OnMouseMove(e);
        }

        //Метод срабатывает при изменение размеров приложения
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            if (CurrentScene != null)
                CurrentScene.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        //Метод для смены текущей сцены
        //Method for changing the current scene
        public void SetCurrentScene(Scene scene)
        {
            _currentScene = scene;

            if(IsLoaded)
                _currentScene.OnLoad();
        }
    }
}
