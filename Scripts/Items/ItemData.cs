using Godot;
using System;

[GlobalClass]
public partial class ItemData : Resource {

	[Export(PropertyHint.FilePath)] public string prefabPath;
	[Export] public Texture2D carryingSprite;
	[Export] public string playerSpriteOverride;

}
