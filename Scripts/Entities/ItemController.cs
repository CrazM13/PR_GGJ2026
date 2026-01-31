using Godot;
using System;

public partial class ItemController : Node {

	[Signal] public delegate void OnItemChangeEventHandler(ItemData item);
	[Signal] public delegate void OnItemRemoveEventHandler(ItemData item);

	[Export] private Entity entity;
	[Export] private ItemData item;
	[Export] private Sprite2D itemSprite;
	[Export] private bool persistant;

	public override void _Ready() {
		base._Ready();

		if (item != null) itemSprite.Texture = item.carryingSprite;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		
		if (entity.GetHealth() <= 0) {

			EmitSignal(SignalName.OnItemRemove, item);

			ItemData lostItem = PlayerInput.inventory.StoreItem(this.item);
			if (lostItem != null) {
				item = lostItem;
				itemSprite.SelfModulate = Colors.White;
				itemSprite.Texture = lostItem.carryingSprite;
			} else {
				this.item = null;
				if (persistant) {
					itemSprite.SelfModulate = Colors.Black;
				} else {
					entity.QueueFree();
				}
			}

			entity.SetHealth(1);
			EmitSignal(SignalName.OnItemChange, item);
		}
	}

}
