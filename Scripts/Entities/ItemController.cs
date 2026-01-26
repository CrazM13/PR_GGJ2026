using Godot;
using System;

public partial class ItemController : Node {

	[Export] private Entity entity;
	[Export] private ItemData item;
	[Export] private Sprite2D itemSprite;

	public override void _Ready() {
		base._Ready();

		itemSprite.Texture = item.carryingSprite;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (entity.GetHealth() <= 0) {
			ItemData lostItem = PlayerInput.inventory.StoreItem(this.item);
			if (lostItem != null) {
				item = lostItem;
				itemSprite.Texture = lostItem.carryingSprite;
				entity.SetHealth(1);
			} else {
				entity.QueueFree();
			}
		}
	}

}
