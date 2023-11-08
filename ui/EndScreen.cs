using Godot;
using System;

public partial class EndScreen : CanvasLayer {

	[Export] private AudioStream VictorySound;
	[Export] private AudioStream DefeatSound;
	private Button _restartButton;
	private Button _quitButton;
	private Label _endText;
	private Label _endDesc;

	private PanelContainer _panelContainer;
	public override void _Ready() {
		_panelContainer = GetNode<PanelContainer>("MarginContainer/PanelContainer");
		_restartButton = GetNode<Button>("%RestartButton");
		_quitButton = GetNode<Button>("%QuitButton");
		_endText = GetNode<Label>("%EndText");
		_endDesc = GetNode<Label>("%EndDesc");
		
		_restartButton.Pressed += OnRestartButtonPressed;
		_quitButton.Pressed += OnQuitButtonPressed;
		_panelContainer.PivotOffset = _panelContainer.Size / 2;
		TweenAnimation();
		GetTree().Paused = true;
	}

	private void TweenAnimation() {

		var tween = CreateTween();
		tween.TweenProperty(_panelContainer, "scale", Vector2.Zero, 0);
		tween.TweenProperty(_panelContainer, "scale", Vector2.One, 0.3)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Back);
	}

	public void SetDefeatScreen() {
		_endText.Text = "Defeat";
		_endDesc.Text = "You lose!";
		PlayJingle();
	}
	
	public void SetVictoryScreen() {
		_endText.Text = "Victory";
		_endDesc.Text = "You win!";
		PlayJingle(true);
	}

	public void PlayJingle(bool victory = false) {
		if (victory) {
			GetNode<AudioStreamPlayer>("VictoryAudioStreamPlayer").Play();
		}
		else {
			GetNode<AudioStreamPlayer>("DefeatAudioStreamPlayer2").Play();
		}
	}

	private async void OnRestartButtonPressed() {
		GetTree().Paused = false;
		var screenTransitionLayer = GetNode<ScreenTransitionLayer>("/root/ScreenTransitionLayer");
		screenTransitionLayer.ReloadTransition();
	}
	
	private void OnQuitButtonPressed() {
		GetTree().Quit();
	}

}
