using Godot;
using System;

public partial class Gate : Node2D {

	[Export] private string keyID;
	[Export] private StaticBody2D collision;
	[Export] private AnimatedSprite2D sprite;

	private uint collisionData;

	public override void _Ready() {
		base._Ready();

		collisionData = collision.CollisionLayer;
		Close();
	}

	public void AttemptUnlock(ItemData key) {
		if (key.id == keyID) {
			Open();
		} else {
			Close();
		}
	}

	public void Open() {
		sprite.Play("open");
		collision.CollisionLayer = 0;
	}

	public void Close() {
		sprite.Play("close");
		collision.CollisionLayer = collisionData;
	}

}
