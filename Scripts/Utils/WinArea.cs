using Godot;
using System;

public partial class WinArea : Area2D {


	public override void _Ready() {
		base._Ready();


		this.BodyEntered += this.OnBodyEntered;

	}

	private void OnBodyEntered(Node2D body) {
		if (body is Entity) {
			PlayerInput.allowInput = false;
			Entity player = PlayerInput.player;
			player.MoveTo(player.GetTilePosition() + (Vector2.Up * 5));
			Engine.TimeScale = 0.5f;

			GetTree().CreateTimer(1.5f).Timeout += SceneManager.Instance.ReloadScene;
		}
	}
}
