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
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void GameOver()
	{
		GetNode<Timer>("RockTimer").Stop();
		GetNode<Timer>("FishTimer").Stop();
		GetNode<Timer>("BigFishTimer").Stop();
		GetNode<Timer>("LevelTimer").Stop();
		GetNode<hud>("HUD").ShowGameOver();
		GetNode<TextureRect>("gameoverscreen").Show();
		
		GetTree().CallGroup("Player", Node.MethodName.QueueFree);
		GetTree().CallGroup("Rock", Node.MethodName.QueueFree);
		GetTree().CallGroup("CheapFish", Node.MethodName.QueueFree);
		GetTree().CallGroup("BigFish", Node.MethodName.QueueFree);
	}
	
	public void NewGame()
	{
		_score = 0;
		_level = 1;
		
		_rockVelocity = 200;
		_cheapFishVelocity = 150;
		_bigFishVelocity = 300;
		
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		var hud = GetNode<hud>("HUD");
		hud.UpdateScore(_score);
		GetNode<Timer>("StartTimer").Start();
		
		GetNode<TextureRect>("startgamescreen").Hide();
		GetNode<TextureRect>("gameoverscreen").Hide();
	}
	
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("RockTimer").Start();
		GetNode<Timer>("LevelTimer").Start();
		GetNode<Timer>("FishTimer").Start();
		GetNode<Timer>("BigFishTimer").Start();
	}

	private void OnBigFishTimerTimeout()
	{
		// Create a new instance of the Mob scene.
		BigFish bigfish = BigFishScene.Instantiate<BigFish>();

		// Choose a random location on Path2D.
		var BigFishSpawn = GetNode<PathFollow2D>("BigFish/BigFishSpawn");
		BigFishSpawn.ProgressRatio = GD.Randf();

		// Set the mob's position to a random location.
		bigfish.Position = BigFishSpawn.Position;

		// Choose the velocity.
		var velocity = new Vector2(0, _bigFishVelocity);
		bigfish.LinearVelocity = velocity;

		// Spawn the mob by adding it to the Main scene.
		AddChild(bigfish);
	}

	private void OnFishTimerTimeout()
	{
		// Create a new instance of the Mob scene.
		CheapFish cheapfish = CheapFishScene.Instantiate<CheapFish>();

		// Choose a random location on Path2D.
		var CheapFishSpawn = GetNode<PathFollow2D>("CheapFish/CheapFishSpawn");
		CheapFishSpawn.ProgressRatio = GD.Randf();

		// Set the mob's position to a random location.
		cheapfish.Position = CheapFishSpawn.Position;

		// Choose the velocity.
		var velocity = new Vector2(0, _cheapFishVelocity);
		cheapfish.LinearVelocity = velocity;

		// Spawn the mob by adding it to the Main scene.
		AddChild(cheapfish);
	}

	private void OnRockTimerTimeout()
	{
		// Create a new instance of the Mob scene.
		Rock rock = RockScene.Instantiate<Rock>();

		// Choose a random location on Path2D.
		var rockSpawnLocation = GetNode<PathFollow2D>("RockPath/RockSpawnLocation");
		rockSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's position to a random location.
		rock.Position = rockSpawnLocation.Position;
		
		// Choose the velocity.
		var velocity = new Vector2(0, _rockVelocity);
		rock.LinearVelocity = velocity;

		// Spawn the mob by adding it to the Main scene.
		AddChild(rock);
	}
	
	public void OnLevelTimerTimeout()
	{
		_level++;
		GetNode<hud>("HUD").UpdateLevel(_level);
		_rockVelocity += 50;
		_cheapFishVelocity += 50;
		_bigFishVelocity += 50;
		
		if (GetNode<Timer>("RockTimer").WaitTime > 0.3){
			GetNode<Timer>("RockTimer").WaitTime -= 0.3;
		} else {
			GetNode<Timer>("FishTimer").WaitTime -= 0.1;
		}
		
		if (GetNode<Timer>("FishTimer").WaitTime > 0.3){
			GetNode<Timer>("FishTimer").WaitTime -= 0.3;
		} else {
			GetNode<Timer>("FishTimer").WaitTime -= 0.1;
		}
		
		if (GetNode<Timer>("BigFishTimer").WaitTime > 0.3){
			GetNode<Timer>("BigFishTimer").WaitTime -= 0.3;
		} else {
			GetNode<Timer>("BigFishTimer").WaitTime -= 0.1;
		}
		
		
		
		
		GD.Print(GetNode<Timer>("RockTimer").WaitTime);
		GD.Print(GetNode<Timer>("FishTimer").WaitTime);
			GD.Print(GetNode<Timer>("BigFishTimer").WaitTime);
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

}
