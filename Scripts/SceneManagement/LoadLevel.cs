using Godot;
using System;

public partial class LoadLevel : Node {

	public static string levelToLoad = "res://Scenes/Level1.tscn";

	public override void _Ready() {
		base._Ready();

		PackedScene levelFile = ResourceLoader.Load<PackedScene>(levelToLoad);

		Node root = levelFile.Instantiate();
		AddChild(root);

	}

}
