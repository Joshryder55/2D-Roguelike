using Godot;

public partial class HUD : Control
{
	private Label _scoreLabel;
	private Label _timerLabel;

	private GameManager _gameManager;

	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("VBoxContainer/ScoreLabel");
		_timerLabel = GetNode<Label>("VBoxContainer/TimerLabel");

		_gameManager = GetNode<GameManager>("/root/GameManager");
	}

	public override void _Process(double delta)
	{
		_scoreLabel.Text = "Score: " + _gameManager.score;

		int minutes = (int)(_gameManager.gameTime / 60);
		int seconds = (int)(_gameManager.gameTime % 60);
		_timerLabel.Text = "Time: " + $"{minutes:00}:{seconds:00}";
	}
}
