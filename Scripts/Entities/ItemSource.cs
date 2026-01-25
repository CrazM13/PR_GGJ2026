using Godot;
using System;

public partial class ItemSource : Node {

	[Export] private Entity entity;
	[Export] private ItemData item;

	public override void _Process(double delta) {
		base._Process(delta);

		if (entity.GetHealth() <= 0) {
			if (item != null) {
				Node newNode = GD.Load<PackedScene>(item.prefabPath).Instantiate();
				this.GetTree().CurrentScene.AddChild(newNode);
			}
			entity.QueueFree();
		}
	}

}
