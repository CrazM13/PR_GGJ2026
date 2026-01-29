using Godot;
using System;

public partial class RandomizeNoise : AudioStreamPlayer2D {

	private float timer = 3f;

	private int state = 21;
	private int prime1 = 7;
	private int prime2 = 13;

	public override void _Process(double delta) {
		base._Process(delta);

		timer -= (float) delta;

		if (timer <= 0) {
			state += prime1;
			state *= prime2;

			timer += (state % 10) + 3;

			this.Position = new Vector2(Mathf.Sin(state * prime2 * Mathf.Pi) * 128, Mathf.Cos(state * prime1 * Mathf.Pi) * 128);
			this.Play();
		}

	}

}
