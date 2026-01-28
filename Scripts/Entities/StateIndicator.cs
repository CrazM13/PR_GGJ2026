using Godot;
using System;

public partial class StateIndicator : Node {

	[Export] private AnimatedSprite2D[] displays;
	[Export] private AudioStreamPlayer2D audioSource;
	[Export] private AudioStream[] alertSFX;
	[Export] private AudioStream[] lostSFX;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public void PlayAlert() {
		// Take an open display and play "alert" animation
		// If none found, override the oldest playing animation
		PlayAnimation("alert");

		if (!audioSource.Playing) {
			audioSource.Stream = alertSFX[rng.RandiRange(0, alertSFX.Length - 1)];
			audioSource.Play();
		}
		
	}

	public void PlayLost() {
		// Take an open display and play "lost" animation
		// If none found, override the oldest playing animation
		PlayAnimation("lost");

		if (!audioSource.Playing) {
			audioSource.Stream = lostSFX[rng.RandiRange(0, lostSFX.Length - 1)];
			audioSource.Play();
		}
	}

	private void PlayAnimation(string animationName) {
		// Find the first available display
		foreach (AnimatedSprite2D display in displays) {
			if (display == null) continue;

			// Check if display is not playing any animation or is in a stopped state
			if (!display.IsPlaying()) {
				display.Play(animationName);
				return;
			}
		}

		// No available display found, override the oldest playing animation
		AnimatedSprite2D oldestDisplay = null;
		float oldestTimestamp = 2;

		foreach (AnimatedSprite2D display in displays) {
			if (display == null) continue;

			// Get the current playback position to determine how far along the animation is
			// We'll use the time as a proxy for "how long it's been playing"
			if (display.IsPlaying()) {
				// Get the time the animation has been playing
				float currentTime = display.FrameProgress;
				if (currentTime < oldestTimestamp) {
					oldestTimestamp = currentTime;
					oldestDisplay = display;
				}
			}
		}

		// If we found an oldest display, override it
		oldestDisplay?.Play(animationName);
	}
}