using Godot;
using System;

public partial class Gate : Node2D {

	[Export] private string keyID;
	[Export] private StaticBody2D collision;
	[Export] private AnimatedSprite2D sprite;
	[Export] private AudioStreamPlayer2D audio;
	[Export] private bool perminantUnlock = false;

	private uint collisionData;
	private bool isOpen = false;

	public override void _Ready() {
		base._Ready();

		collisionData = collision.CollisionLayer;
		sprite.Play("close");
	}

	public void AttemptUnlock(ItemData key) {
		if ((key != null && key.id == keyID) || (string.IsNullOrEmpty(keyID) && key == null)) {
			Open();
		} else {
			if (!perminantUnlock) Close();
		}
	}

	public void ChangeState(bool isOpen) {
		if (isOpen) {
			Open();
		} else {
			if (!perminantUnlock) Close();
		}
	}

	public void Open() {
		if (isOpen) return;
		sprite.Play("open");

		audio.Play();
		CameraController.Instance.FocusCameraOn(this.GlobalPosition, 3f);
		isOpen = true;

		collision.CollisionLayer = 0;
	}

	public void Close() {
		if (!isOpen) return;
		sprite.Play("close");

		audio.Play();
		CameraController.Instance.FocusCameraOn(this.GlobalPosition, 3f);
		isOpen = false;

		collision.CollisionLayer = collisionData;
	}

}
