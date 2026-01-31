using Godot;
using System;

public partial class HiddenDisplay : TextureRect {

	[Export] private Texture2D showTexture;
	[Export] private Texture2D hiddenTexture;


	public void UpdateTexture(bool isVisable) {
		this.Texture = isVisable ? showTexture : hiddenTexture;
	}

}
