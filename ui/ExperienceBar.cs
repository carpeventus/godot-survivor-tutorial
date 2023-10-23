using Godot;

public partial class ExperienceBar : CanvasLayer {
    [Export] public ExperienceManager ExperienceManager;

    private ProgressBar _experienceProgressBar;
    public override void _Ready() {
        _experienceProgressBar = GetNode<ProgressBar>("MarginContainer/ProgressBar");
        _experienceProgressBar.Value = 0f;
        ExperienceManager.ExperienceUpdated += OnExperienceUpdated;
    }

    private void OnExperienceUpdated(float current, float target) {
        _experienceProgressBar.Value = current / target;
    }
}
