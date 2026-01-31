using Godot;
using System;

public partial class HUD : CanvasLayer {

	private static int hidenLevel;
	private static bool isDirty;
	public static int HiddenLevel {
		set {
			hidenLevel = value;
			isDirty = true;
		}
		get {
			return hidenLevel;
		}
	}

	[Export] private HiddenDisplay hiddenDisplay;

	public override void _Process(double delta) {
		base._Process(delta);

		if (isDirty) {
			hiddenDisplay.UpdateTexture(hidenLevel > 0);
			isDirty = false;
		}
	}


}
