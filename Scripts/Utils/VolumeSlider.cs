using Godot;
using System;

public partial class VolumeSlider : HSlider {

	[Export] private string busName = "Master";

	public override void _EnterTree() {
		base._EnterTree();

		this.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex(busName));

		this.ValueChanged += this.OnVolumeChanged;

	}

	private void OnVolumeChanged(double value) {
		AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex(busName), (float) value);
	}
}
