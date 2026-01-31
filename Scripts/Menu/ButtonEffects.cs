using Godot;
using System;

public partial class ButtonEffects : Control {

	#region Rotation Settings
	private float rotationAmplitude = 0.05f; // Radians
	private float rotationSpeed = 2.0f;
	#endregion

	#region Scale Settings
	private Vector2 scaleAmplitude = new Vector2(0.025f, 0.025f);
	private float scaleSpeed = 3.0f;
	#endregion

	private Control button;
	private bool isHover;

	private float timer;

	public override void _Ready() {
		base._Ready();

		this.MouseEntered += this.OnMouseEntered;
		this.MouseExited += this.OnMouseExited;

		button = this.GetParent<Control>();

	}

	public override void _Process(double delta) {
		base._Process(delta);

		timer += (float) delta;

		if (isHover) {
			RunEffect();
		}

	}

	private void OnMouseEntered() {
		isHover = true;
	}

	private void OnMouseExited() {
		isHover = false;

		ResetEffect();

	}

	private void RunEffect() {
		button.PivotOffset = button.Size / 2;

		// See-Saw animation
		float rotation = (float) Math.Sin(timer * rotationSpeed) * rotationAmplitude;
		button.Rotation = rotation;

		// Pulsing animation
		Vector2 scale = Vector2.One + new Vector2(
			(float) Math.Sin(timer * scaleSpeed) * scaleAmplitude.X,
			(float) Math.Sin(timer * scaleSpeed) * scaleAmplitude.Y
		);
		button.Scale = scale;

	}

	private void ResetEffect() {
		button.Rotation = 0;
		button.Scale = Vector2.One;
	}
}