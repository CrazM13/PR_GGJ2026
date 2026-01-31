using Godot;
using System;
using System.Collections.Generic;

public partial class PunchArea : Area2D {

	public override void _Process(double delta) {
		base._Process(delta);

		if (Input.IsActionJustPressed("attack")) {
			foreach (Node2D node in this.GetOverlappingBodies()) {
				if (node is Entity e) {
					e.Damage();
					e.Knockback(GlobalPosition - e.GlobalPosition, 0.05f);
				}
			}
		}
	}

}
