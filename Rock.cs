using Godot;
using System;

public partial class Rock : RigidBody2D
{
	
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
	}

	public override void _Process(double delta)
	{
	}
	
	private void OnVisibleOnScreenNotifier2dScreenExited()
	{
		QueueFree();
	}

}
