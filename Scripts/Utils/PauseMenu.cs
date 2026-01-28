using Godot;
using System;

public partial class PauseMenu : CanvasLayer {

	private bool isOpen = false;

	public override void _Process(double delta) {
		base._Process(delta);

		if (Input.IsActionJustPressed("pause_toggle")) {
			if (isOpen) {
				UnpauseGame();
			} else {
				PauseGame();
			}
		}

	}

	public void PauseGame() {
		Show();
		isOpen = true;
		GetTree().Paused = true;
	}

	public void UnpauseGame() {
		Hide();
		isOpen = false;
		GetTree().Paused = false;
	}

	public void Quit() {
		SceneManager.Instance.LoadScene("res://Scenes/MainMenu.tscn");
	}

}
