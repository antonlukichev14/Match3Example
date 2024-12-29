using Match3Example.Scenes;
using StbImageSharp;
using System.IO;

namespace Match3Example
{
    internal class Program
    {
        //Точка входа в программе
        //Entry point in the program
        static void Main(string[] args)
        {
            //Создание окна приложения
            //Creating the application window
            using (Viewport viewport = new Viewport(1920, 1000, "Match3Example"))
            {
                viewport.Icon = LoadWindowIcon(Path.GetAssetPath("Textures/element4.png"));
                viewport.SetCurrentScene(new MainMenu()); //Установка начальной сцены. Set the initial scene.
                viewport.Run();
            }
        }

        //Функция которая загружает изображение для использования в качестве window.icon
        //Function that loads an image for use as window.icon
        static OpenTK.Windowing.Common.Input.WindowIcon LoadWindowIcon(string path)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            OpenTK.Windowing.Common.Input.Image icon = new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, image.Data);
            return new OpenTK.Windowing.Common.Input.WindowIcon(icon);
        }
    }
}
