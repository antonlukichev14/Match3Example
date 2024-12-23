namespace Match3Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1920, 1000, "Match3Example"))
            {
                game.Run();
            }
        }
    }
}
