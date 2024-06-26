using Godot;
using System;

public partial class hud : CanvasLayer
	{

	[Signal]
	public delegate void StartGameEventHandler();

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();

		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver()
	{
		ShowMessage("Perdeste");
		GetNode<ProgressBar>("HealthBar").Hide();

		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		var message = GetNode<Label>("Message");
		message.Text = "Seu pato!";
		message.Show();

		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
		GetNode<Button>("QuitButton").Show();
		GetNode<Button>("PauseButton").Hide();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreValue").Text = score.ToString();
	}
	
	public void UpdateLevel(int level)
	{
		GetNode<Label>("LevelValue").Text = level.ToString();
	}
	
	public void UpdateHealth(int health)
	{
		GetNode<ProgressBar>("HealthBar").Value = health;
	}
	
	public void UpdateHighScore()
	{
		GetNode<Label>("HighScoreValue").Text = SaveData.Load().ToString();
	}
	
	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("Message").Hide();
		GetNode<Button>("QuitButton").Hide();
		GetNode<Button>("PauseButton").Show();
		
		GetNode<Label>("Score").Show();
		GetNode<Label>("ScoreValue").Show();
		GetNode<Label>("HighScore").Hide();
		GetNode<Label>("HighScoreValue").Hide();
		GetNode<Label>("Level").Show();
		GetNode<Label>("LevelValue").Show();
		GetNode<ProgressBar>("HealthBar").Show();

		EmitSignal(SignalName.StartGame);
		GetTree().Paused = false;
	}

	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
	
	public void OnPauseButtonPressed()
	{
		GetNode<Button>("ResumeButton").Show();
		GetNode<Button>("RestartButton").Show();
		GetNode<Button>("QuitButton").Show();
		GetNode<Button>("PauseButton").Hide();
		GetTree().Paused = true;
	}
	
	public void OnResumeButtonPressed()
	{
		GetNode<Button>("ResumeButton").Hide();
		GetNode<Button>("PauseButton").Show();
		GetNode<Button>("RestartButton").Hide();
		GetNode<Button>("QuitButton").Hide();
		GetTree().Paused = false;
	}
	
	private void OnRestartButtonPressed()
	{
		GetTree().ReloadCurrentScene();
	}
	
}
