using Godot;
using System;

public partial class WinArea : Area2D {

	[Export] private string nextLevel;
	[Export] private AudioStreamPlayer winSound;

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

			winSound.Play();

			GetTree().CreateTimer(1.5f).Timeout += () => {

				if (string.IsNullOrEmpty(nextLevel)) {
					LoadLevel.levelToLoad = "res://Scenes/Level1.tscn";
					SceneManager.Instance.LoadScene("res://Scenes/MainMenu.tscn");
				} else {
					LoadLevel.levelToLoad = nextLevel;
					SceneManager.Instance.ReloadScene();
				}

				Engine.TimeScale = 1f;
				
			};
		}
	}
}
