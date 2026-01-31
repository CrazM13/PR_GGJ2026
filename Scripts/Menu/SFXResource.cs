using Godot;
using System;

namespace BetterUISuite {
	[GlobalClass]
	public partial class SFXResource : Resource {

		[Export] public AudioStream SFX { get; set; }
		[Export] public StringName AudioBus { get; set; } = "Master";

	}
}
