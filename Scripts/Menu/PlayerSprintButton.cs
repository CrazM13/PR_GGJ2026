using BetterUISuite;
using Godot;
using System;

public partial class PlayerSprintButton : BetterButton {

	public override void _Ready() {
		base._Ready();

		this.Pressed += this.OnPressed;

	}

	private void OnPressed() {
		PlayerInput.sprintToggle = this.ButtonPressed;
	}
}
