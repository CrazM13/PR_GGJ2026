using Godot;
using System;

public partial class ItemSource : Node {

	[Export] private Entity entity;
	[Export] private ItemData item;

	public override void _Process(double delta) {
		base._Process(delta);

		if (entity.GetHealth() <= 0) {
			if (item != null) {
				Node2D newNode = GD.Load<PackedScene>(item.prefabPath).Instantiate<Node2D>();
				newNode.GlobalPosition = entity.GlobalPosition;
				entity.GetParent().AddChild(newNode);
			}

			this.QueueFree();
		}

	}

}
