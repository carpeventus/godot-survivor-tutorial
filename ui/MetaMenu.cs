using Godot;
using System;
using Godot.Collections;

public partial class MetaMenu : CanvasLayer {

	[Export] public Array<MetaUpgrade> MetaUpgrades;
	public PackedScene MetaCardScene { get; private set; }

	private GridContainer _gridContainer;
	public override void _Ready() {
		GetNode<Button>("%BackButton").Pressed += OnBackButtonPressed;
		_gridContainer = GetNode<GridContainer>("%GridContainer");
		MetaCardScene = ResourceLoader.Load<PackedScene>("res://ui/meta_upgrade_card.tscn");
		DisplayMetaCards();
	}

	private void OnBackButtonPressed() {
		var screenTransitionLayer = GetNode<ScreenTransitionLayer>("/root/ScreenTransitionLayer");
		screenTransitionLayer.Transition("res://scenes/main_menu.tscn");
	}


	private void DisplayMetaCards() {
		for (int i = 0; i < MetaUpgrades.Count; i++) {
			var card = MetaCardScene.Instantiate<MetaUpgradeCard>();
			_gridContainer.AddChild(card);
            card.SetMetaCard(MetaUpgrades[i]);
		}
	}
	
}
