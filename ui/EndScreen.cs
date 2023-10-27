using Godot;
using System;

public partial class EndScreen : CanvasLayer {

	private Button _restartButton;
	private Button _quitButton;
	private Label _endText;
	private Label _endDesc;
	public override void _Ready() {
		_restartButton = GetNode<Button>("%RestartButton");
		_quitButton = GetNode<Button>("%QuitButton");
		_endText = GetNode<Label>("%EndText");
		_endDesc = GetNode<Label>("%EndDesc");
		
		_restartButton.Pressed += OnRestartButtonPressed;
		_quitButton.Pressed += OnQuitButtonPressed;
		GetTree().Paused = true;
	}

	public void SetDefeatScreen() {
		_endText.Text = "Defeat";
		_endDesc.Text = "You lose!";
	}
	
	public void SetVictoryScreen() {
		_endText.Text = "Victory";
		_endDesc.Text = "You win!";
	}

	private void OnRestartButtonPressed() {
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}
	
	private void OnQuitButtonPressed() {
		GetTree().Quit();
	}

}
