using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene RockScene { get; set; }
	
	[Export]
	public PackedScene CheapFishScene { get; set; }
	
	[Export]
	public PackedScene BigFishScene { get; set; }

	private int _rockVelocity;
	private int _cheapFishVelocity;
	private int _bigFishVelocity;
	
	private int _score;
	
	private int _level;
	
	public override void _Ready()
	{
		GetNode<hud>("HUD").UpdateHighScore();
	}

	public override void _Process(double delta)
	{
	}
	
	private void GameOver()
	{
		UpdateHighScore();
		
		GetNode<Timer>("RockTimer").Stop();
		GetNode<Timer>("FishTimer").Stop();
		GetNode<Timer>("BigFishTimer").Stop();
		GetNode<Timer>("LevelTimer").Stop();
		GetNode<hud>("HUD").ShowGameOver();
		GetNode<TextureRect>("gameoverscreen").Show();
		
		GetNode<Player>("Player").Hide();
		GetTree().CallGroup("Rock", Node.MethodName.QueueFree);
		GetTree().CallGroup("CheapFish", Node.MethodName.QueueFree);
		GetTree().CallGroup("BigFish", Node.MethodName.QueueFree);
	}
	
	public void NewGame()
	{
		_score = 0;
		_level = 1;
		
		_rockVelocity = 150;
		_cheapFishVelocity = 150;
		_bigFishVelocity = 300;
		
		GetNode<Timer>("RockTimer").WaitTime = 3;
		GetNode<Timer>("FishTimer").WaitTime = 3;
		GetNode<Timer>("BigFishTimer").WaitTime = 10;
		
		var player = GetNode<Player>("Player");
		player.ResetHealth();
		player.Start(GetNode<Marker2D>("StartPosition").Position);
		var hud = GetNode<hud>("HUD");
		hud.UpdateScore(_score);
		hud.UpdateLevel(_level);
		
		GetNode<Timer>("RockTimer").Start();
		GetNode<Timer>("LevelTimer").Start();
		GetNode<Timer>("FishTimer").Start();
		GetNode<Timer>("BigFishTimer").Start();
		
		GetNode<TextureRect>("startgamescreen").Hide();
		GetNode<TextureRect>("gameoverscreen").Hide();
	}

	private void OnBigFishTimerTimeout()
	{
		BigFish bigfish = BigFishScene.Instantiate<BigFish>();

		var BigFishSpawn = GetNode<PathFollow2D>("BigFish/BigFishSpawn");
		BigFishSpawn.ProgressRatio = GD.Randf();
		bigfish.Position = BigFishSpawn.Position;
		bigfish.LinearVelocity = new Vector2(0, _bigFishVelocity);
		
		AddChild(bigfish);
	}

	private void OnFishTimerTimeout()
	{
		CheapFish cheapfish = CheapFishScene.Instantiate<CheapFish>();

		var CheapFishSpawn = GetNode<PathFollow2D>("CheapFish/CheapFishSpawn");
		CheapFishSpawn.ProgressRatio = GD.Randf();
		cheapfish.Position = CheapFishSpawn.Position;
		cheapfish.LinearVelocity = new Vector2(0, _cheapFishVelocity);

		AddChild(cheapfish);
	}

	private void OnRockTimerTimeout()
	{
		Rock rock = RockScene.Instantiate<Rock>();

		var rockSpawnLocation = GetNode<PathFollow2D>("RockPath/RockSpawnLocation");
		rockSpawnLocation.ProgressRatio = GD.Randf();
		rock.Position = rockSpawnLocation.Position;
		rock.LinearVelocity = new Vector2(0, _rockVelocity);

		AddChild(rock);
	}
	
	public void OnLevelTimerTimeout()
	{
		_level++;
		GetNode<hud>("HUD").UpdateLevel(_level);
		_rockVelocity += 50;
		_cheapFishVelocity += 50;
		_bigFishVelocity += 50;
		
		if (GetNode<Timer>("RockTimer").WaitTime > 0.4){
			GetNode<Timer>("RockTimer").WaitTime -= 0.3;
		} else {
			GetNode<Timer>("RockTimer").WaitTime = 0.1;
		}
		
		if (GetNode<Timer>("FishTimer").WaitTime > 0.4){
			GetNode<Timer>("FishTimer").WaitTime -= 0.3;
		} else {
			GetNode<Timer>("FishTimer").WaitTime = 0.1;
		}
		
		if (GetNode<Timer>("BigFishTimer").WaitTime > 0.4){
			GetNode<Timer>("BigFishTimer").WaitTime -= 0.3;
		} else {
			GetNode<Timer>("BigFishTimer").WaitTime = 0.1;
		}
	}
	
	private void OnCheapFishHit()
	{
		_score+=5;
		GetNode<hud>("HUD").UpdateScore(_score);
	}
	
	private void OnBigFishHit()
	{
		_score += 100;
		GetNode<hud>("HUD").UpdateScore(_score);
	}
	
	private void UpdateHighScore()
	{
		var highScore = SaveData.Load();
		if (_score > highScore)
		{
			SaveData.Save(_score);
		}
	}

}
