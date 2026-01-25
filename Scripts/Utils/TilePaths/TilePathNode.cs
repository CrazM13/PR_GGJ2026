using Godot;
using System;

public partial class TilePathNode : Node2D {

	[Export] private TilePathNode nextNode;

	public TilePathNode GetNextNode() {
		if (nextNode != null) return nextNode;
		return this;
	}

	public override void _Draw() {
		base._Draw();

		Vector2 correctedPosition = TileUtils.GetCenterPosition(TileUtils.GetTilePosition(this.GlobalPosition)) - this.GlobalPosition;

		if (OS.IsDebugBuild()) {
			this.DrawCircle(correctedPosition, 16, Colors.Blue);
			if (nextNode != null) {
				Vector2 nextCorrectedPosition = TileUtils.GetCenterPosition(TileUtils.GetTilePosition(nextNode.GlobalPosition)) - this.GlobalPosition;
				this.DrawDashedLine(correctedPosition, nextCorrectedPosition, Colors.Blue, 4);
			}
		}

	}

}
