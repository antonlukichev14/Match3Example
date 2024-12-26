using Match3Example.Scenes;

namespace Match3Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Viewport viewport = new Viewport(1920, 1000, "Match3Example"))
            {
                viewport.currentScene = new MainMenu(viewport);
                viewport.Run();
            }
        }
    }
}
