using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class MetaUpgradeCard : PanelContainer
{
    private Label _nameLabel;
    private Label _descLabel;
    private Label _progressLabel;
    private Label _countLabel;
    private ProgressBar _progressBar;
    private Button _purchaseButton;
    private AnimationPlayer _animationPlayer;

    [Signal]
    public delegate void UpgradeCardSelectedEventHandler();

    private MetaUpgrade _metaUpgrade;
    
    public override void _Ready() {
        _nameLabel = GetNode<Label>("%AbilityCardName");
        _descLabel = GetNode<Label>("%AbilityCardDesc");
        _countLabel = GetNode<Label>("%CountLabel");
        _progressBar = GetNode<ProgressBar>("%ProgressBar");
        _purchaseButton = GetNode<Button>("%PurchaseButton");
        _progressLabel = GetNode<Label>("%ProgressLabel");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        GuiInput += OnGuiInput;
        _purchaseButton.Pressed += OnPurchaseButtonPressed;
    }

    private void OnPurchaseButtonPressed() {
        if (_metaUpgrade is null) {
            return;
        }
        GetNode<MetaProgression>("/root/MetaProgression").SavedData.MetaUpgradeCurrency -= _metaUpgrade.ExperienceCost;
        GetNode<MetaProgression>("/root/MetaProgression").AddMetaUpgrade(_metaUpgrade);
        GetTree().CallGroup("MetaUpgradeCard", MethodName.UpdateProgress);
        Selected();
    }


    public void SetMetaCard(MetaUpgrade metaUpgrade) {
        _metaUpgrade = metaUpgrade;
        _nameLabel.Text = metaUpgrade.Name;
        _descLabel.Text = metaUpgrade.Description;
        UpdateProgress();
    }

    private void UpdateProgress() {
        var savedData = GetNode<MetaProgression>("/root/MetaProgression").SavedData;
        var currentQuantity = 0;
        var currentCurrency = savedData.MetaUpgradeCurrency;
        if (savedData.SavedDict.TryGetValue(_metaUpgrade.Id, out var dictValue)) {
            GD.Print(dictValue.Quantity);
            currentQuantity = dictValue.Quantity;
        }
        var isMaxQuantity = currentQuantity >= _metaUpgrade.MaxQuantity;
        var percent = (float) currentCurrency / _metaUpgrade.ExperienceCost;
        _progressBar.Value = MathF.Min(percent, 1);
        _purchaseButton.Disabled = percent < 1 || isMaxQuantity;
        if (isMaxQuantity) {
            _purchaseButton.Text = "Max";
        }
        _progressLabel.Text = currentCurrency + "/" + _metaUpgrade.ExperienceCost;
        _countLabel.Text = $"x{currentQuantity}";
    }

    private void OnGuiInput(InputEvent @event) {

        if (@event.IsActionPressed("left_click") ) {
            Selected();
        }
    }

    private void Selected() {
        _animationPlayer.Play("selected");
    }
    
}
