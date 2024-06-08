using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 300;

	public Vector2 ScreenSize;
	
	private int _health;
	
	private const int MAX_HEALTH = 4;

	[Signal]
	public delegate void HitCheapFishEventHandler();
	
	[Signal]
	public delegate void HitBigFishEventHandler();
	
	[Signal]
	public delegate void GameOverEventHandler();

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
		_health = MAX_HEALTH;
		GetNode<hud>("../HUD").UpdateHealth(_health);
	}

	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;
		
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		var tempPosition = Position + (velocity * (float)delta);

		if (tempPosition.X < 400 && tempPosition.X > 90)
		{
			Position = new Vector2(
				x: Mathf.Clamp(tempPosition.X, 0, ScreenSize.X),
				y: Mathf.Clamp(tempPosition.Y, 0, ScreenSize.Y)
			);
		}

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Rock")) 
		{
			body.QueueFree();
			if (_health > 1){
				body.QueueFree();
				_health--;
				GetNode<hud>("../HUD").UpdateHealth(_health);
			} else {
				EmitSignal(SignalName.GameOver);
			}
			
		} else if (body.IsInGroup("BigFish")) 
		{
			body.QueueFree();
			EmitSignal(SignalName.HitBigFish);
			
		} else if (body.IsInGroup("CheapFish")) 
		{
			body.QueueFree();
			EmitSignal(SignalName.HitCheapFish);
		}
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	
	public void ResetHealth(){
		_health = MAX_HEALTH;
		GetNode<hud>("../HUD").UpdateHealth(_health);
	}

}
