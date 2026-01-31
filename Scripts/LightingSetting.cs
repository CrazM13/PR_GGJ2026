using Godot;
using System;

public partial class LightingSetting : ColorRect {

	[Export] private Node levelContainer;

	public override void _Ready() {
		base._Ready();

		LevelData data = levelContainer.GetChild<LevelData>(0);
		this.Visible = data?.hasLighting ?? true;

	}

}
