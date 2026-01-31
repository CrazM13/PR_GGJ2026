using Godot;
using System;

public partial class PlayerInput : Node {

	public static Vector2 playerPosition;
	public static Entity player;
	public static Inventory inventory;
	public static bool allowInput = true;

	[Export] private Entity entity;
	[Export] private Sprite2D heldSprite;
	[Export] private AudioStreamPlayer deathSound;

	private bool isAlive = true;

	private const float SPRINT_MOD = 2f;

	public override void _Ready() {
		base._Ready();

		player = entity;
		inventory = new Inventory();
		allowInput = true;
		isAlive = true;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (allowInput) {

			entity.SetSpeedModifier(Input.IsActionPressed("move_sprint") ? SPRINT_MOD : 1);
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

			if (Input.IsActionJustPressed("drop_item")) {
				ItemData item = inventory.GetStoredItem();

				if (item != null) {
					Node2D newNode = GD.Load<PackedScene>(item.prefabPath).Instantiate<Node2D>();
					newNode.GlobalPosition = entity.GlobalPosition + (entity.GetFacingDirection() * TileUtils.TileSize);
					entity.GetParent().AddChild(newNode);

					inventory.ClearInventory();
				}
			}

		}

		playerPosition = entity.GlobalPosition;

		ItemData held = inventory.GetStoredItem();
		heldSprite.Texture = held?.carryingSprite;
		entity.CollisionLayer = (uint) (held?.id == "disguise" ? 4097 : 32769);

		if (isAlive && entity.GetHealth() <= 0) {
			allowInput = false;
			isAlive = false;
			deathSound.Play();

			GetTree().CreateTimer(3).Timeout += SceneManager.Instance.ReloadScene;
		}
	}

}
