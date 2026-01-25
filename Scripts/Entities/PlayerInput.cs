using Godot;
using System;

public partial class PlayerInput : Node {

	public static Vector2 playerPosition;
	public static Entity player;
	public static Inventory inventory;

	[Export] private Entity entity;
	[Export] private Sprite2D heldSprite;

	public override void _Ready() {
		base._Ready();

		player = entity;
		inventory = new Inventory();
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (!entity.IsMoving()) {
			if (Input.IsActionPressed("move_right")) {
				entity.MoveTo(entity.GetTilePosition() + Vector2.Right);
			} else if (Input.IsActionPressed("move_up")) {
				entity.MoveTo(entity.GetTilePosition() + Vector2.Up);
			} else if (Input.IsActionPressed("move_left")) {
				entity.MoveTo(entity.GetTilePosition() + Vector2.Left);
			} else if (Input.IsActionPressed("move_down")) {
				entity.MoveTo(entity.GetTilePosition() + Vector2.Down);
			}
		}

		playerPosition = entity.GlobalPosition;

		heldSprite.Texture = inventory.GetStoredItem()?.carryingSprite;
	}

}
