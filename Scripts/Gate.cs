using Godot;
using System;

public partial class Gate : Node2D {

	[Export] private string keyID;
	[Export] private StaticBody2D collision;
	[Export] private AnimatedSprite2D sprite;
	[Export] private bool perminantUnlock = false;

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
			if (!perminantUnlock) Close();
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
