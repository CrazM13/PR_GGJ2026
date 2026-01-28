using Godot;
using System;

public partial class StepTrigger : Area2D {

	[Signal] public delegate void OnStateChangeEventHandler(bool state);

	[Export] private bool toggleable;
	[Export] private Sprite2D sprite;

	private int activeLayers;
	private bool currentState = false;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEntered;
		this.BodyExited += this.OnBodyExited;

	}

	private void OnBodyExited(Node2D body) {
		activeLayers--;

		if (activeLayers == 0 && !toggleable) {
			currentState = false;
			EmitSignal(SignalName.OnStateChange, currentState);
			sprite.Frame = 0;
		}
	}

	private void OnBodyEntered(Node2D body) {
		int oldActiveLayers = activeLayers;
		activeLayers++;

		if (oldActiveLayers == 0) {
			if (toggleable) {
				currentState = !currentState;
				EmitSignal(SignalName.OnStateChange, currentState);
				sprite.Frame = currentState ? 1 : 0;
			}  else {
				currentState = true;
				EmitSignal(SignalName.OnStateChange, currentState);
				sprite.Frame = 1;
			} 
		}
	}
}
