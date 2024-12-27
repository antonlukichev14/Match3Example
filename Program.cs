using Match3Example.Scenes;
using StbImageSharp;
using System.IO;

namespace Match3Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(Path.GetAssetPath("Textures/element4.png")), ColorComponents.RedGreenBlueAlpha);

            using (Viewport viewport = new Viewport(1920, 1000, "Match3Example"))
            {
                OpenTK.Windowing.Common.Input.Image icon = new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, image.Data);
                viewport.Icon = new OpenTK.Windowing.Common.Input.WindowIcon(icon);
                viewport.SetCurrentScene(new MainMenu());
                viewport.Run();
            }
        }
    }
}
