using Godot;
using System;

public partial class OptionsMenu : CanvasLayer
{
	[Signal]
	public delegate void BackButtonPressedEventHandler();

	private Button _backButton;
	private Button _windowModeButton;
	private HSlider _sfxHSlider;
	private HSlider _musicHSlider;
	public override void _Ready()
	{
		_backButton = GetNode<Button>("%BackButton");
		_windowModeButton = GetNode<Button>("%WindowModeButton");
		_musicHSlider = GetNode<HSlider>("%MusicHSlider");
		_sfxHSlider = GetNode<HSlider>("%SfxHSlider");
		_windowModeButton.Pressed += OnWindowModeButtonPressed;
		_musicHSlider.ValueChanged += (p) => { OnHSliderChanged(p, "music");};
		_sfxHSlider.ValueChanged += (p) => { OnHSliderChanged(p, "sfx");};
		_backButton.Pressed += OnBackButtonPressed;
		UpdateDisplay();
	}

	private void OnBackButtonPressed() {
		EmitSignal(SignalName.BackButtonPressed);
	}

	private float GetBusVolume(string busName)
	{
		var busIndex = AudioServer.GetBusIndex(busName);
		var volumeDb = AudioServer.GetBusVolumeDb(busIndex);
		return Mathf.DbToLinear(volumeDb);
	}

	private void OnHSliderChanged(double percent, string busName)
	{
		SetBusVolume((float)percent, busName);
	}
	
	private void SetBusVolume(float percent, string busName)
	{
		var busIndex = AudioServer.GetBusIndex(busName);
		float db = Mathf.LinearToDb(percent);
		AudioServer.SetBusVolumeDb(busIndex, db);
	}

	private void OnWindowModeButtonPressed()
	{
		var mode = DisplayServer.WindowGetMode();
		if (mode != DisplayServer.WindowMode.Fullscreen)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}

		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		_windowModeButton.Text = "Windowed";
		if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
		{
			_windowModeButton.Text = "FullScreen";
		}

		_sfxHSlider.Value = GetBusVolume("sfx");
		_musicHSlider.Value = GetBusVolume("music");
	}
}
