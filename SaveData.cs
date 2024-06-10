using Godot;
using System;

public partial class SaveData : Node
{
	
	private const string SAVE_PATH = "user://save_game.dat";
	
	public static void Save(int score)
	{
		using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
		file.Store32((uint) score);
	}

	public static int Load()
	{
		int highScore = 0;
		try{
			using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
			highScore = (int) file.Get32();
			
		} catch (Exception e)
		{
			Console.WriteLine($"\tMessage: {e.Message}");
		}
		return highScore;
	}
	
}
