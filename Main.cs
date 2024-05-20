using Godot;
using System;

public partial class Main : Node
{
	
	[Export]
	public PackedScene RockScene { get; set; }
	
	public PackedScene CheapFishScene { get; set; }
	
	public PackedScene BigFishScene { get; set; }

	private int _score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
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
		GetNode<Timer>("ScoreTimer").Stop();
	}
	
	public void NewGame()
	{
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}
	
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("RockTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Timer>("FishTimer").Start();
		GetNode<Timer>("BigFishTimer").Start();
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
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
		var velocity = new Vector2(0, 150);
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
		var velocity = new Vector2(0, 150);
		cheapfish.LinearVelocity = velocity;

		// Spawn the mob by adding it to the Main scene.
		AddChild(cheapfish);
	}

	private void OnRockTimerTimeout()
	{
		// Note: Normally it is best to use explicit types rather than the `var`
		// keyword. However, var is acceptable to use here because the types are
		// obviously Mob and PathFollow2D, since they appear later on the line.

		// Create a new instance of the Mob scene.
		Rock rock = RockScene.Instantiate<Rock>();

		// Choose a random location on Path2D.
		//var rockSpawnLocation = GetNode<PathFollow2D>("Rockpath2D/PathFollow2D");
		var rockSpawnLocation = GetNode<PathFollow2D>("RockPath/RockSpawnLocation");
		rockSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's position to a random location.
		rock.Position = rockSpawnLocation.Position;


		// Choose the velocity.
		var velocity = new Vector2(0, 150);
		rock.LinearVelocity = velocity;

		// Spawn the mob by adding it to the Main scene.
		AddChild(rock);
	}

}

