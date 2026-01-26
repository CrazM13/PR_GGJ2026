using Godot;
using System;

public partial class TransitionEffect : CanvasLayer {

	[Signal] public delegate void TransitionInCompleteEventHandler();
	[Signal] public delegate void TransitionOutCompleteEventHandler();
	[Signal] public delegate void TransitionCompleteEventHandler();

	[Export] private AnimationPlayer animation;

	[Export] private float transitionDuration = 0.8f;

	private float timer = 0;
	private bool isPlaying = false;
	private bool isInOut = false;

	public override void _Ready() {
		base._Ready();

		GetTree().Paused = true;
	}

	public void CloseMenu() {
		GetTree().Paused = false;

		QueueFree();
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (isPlaying) {
			timer += (float) delta;

			if (timer >= transitionDuration) {
				isPlaying = false;

				EmitSignal(SignalName.TransitionComplete);

				if (isInOut) {
					EmitSignal(SignalName.TransitionInComplete);
				} else {
					EmitSignal(SignalName.TransitionOutComplete);
				}

				GetTree().Paused = false;
			}
		}

	}

	public void PlayTransitionIn() {
		animation.Play("transition_in");
		isInOut = true;
		isPlaying = true;
		timer = 0;

		GetTree().Paused = true;
	}

	public void PlayTransitionOut() {
		animation.Play("transition_out");
		isInOut = false;
		isPlaying = true;
		timer = 0;

		GetTree().Paused = true;
	}

}
