namespace Test_Game
{
	class Program
	{
		static void Main (string[] args)
		{
			var game = new TestGame();
			game.Initialize();
			game.Run();
			game.Shutdown();
		}
	}
}
