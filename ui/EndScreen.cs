using Godot;
using System;

public partial class EndScreen : CanvasLayer {

	private Button _restartButton;
	private Button _quitButton;
	private Label _endText;
	private Label _endDesc;
	public override void _Ready() {
        GD.Print("End Screen Ready");
		_restartButton = GetNode<Button>("%RestartButton");
		_quitButton = GetNode<Button>("%QuitButton");
		_endText = GetNode<Label>("%EndText");
		_endDesc = GetNode<Label>("%EndDesc");
		GD.Print(_endText);
		GD.Print(_endDesc);
		
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
		GetTree().ChangeSceneToFile("res://scenes/main_scene.tscn");
	}
	
	private void OnQuitButtonPressed() {
		GetTree().Quit();
	}

}
