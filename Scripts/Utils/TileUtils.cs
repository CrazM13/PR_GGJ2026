using Godot;
using System;

public static class TileUtils {

	public const int TileSize = 64;

	public static Vector2 GetTilePosition(Vector2 pixelPosition) {
		return new Vector2(
			Mathf.Floor(pixelPosition.X / TileSize),
			Mathf.Floor(pixelPosition.Y / TileSize)
		);
	}

	public static Vector2 GetCenterPosition(Vector2 tilePosition) {
		return new Vector2(
			tilePosition.X * TileSize + (TileSize / 2),
			tilePosition.Y * TileSize + (TileSize / 2)
		);
	}

}
