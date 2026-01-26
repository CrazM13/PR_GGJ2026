using Godot;
using System;

public partial class SharedViewport : SubViewport {

	[Export] private SubViewport viewport;

	public override void _Ready() {
		base._Ready();

		viewport.World2D = this.World2D;

	}

}
