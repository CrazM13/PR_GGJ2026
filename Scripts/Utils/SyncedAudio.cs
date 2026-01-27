using Godot;
using System;

public partial class SyncedAudio : AudioStreamPlayer {


	private static float time;

	public override void _EnterTree() {
		base._EnterTree();

		this.Play(time);

	}

	public override void _Process(double delta) {
		base._Process(delta);

		time = this.GetPlaybackPosition();

	}

}
