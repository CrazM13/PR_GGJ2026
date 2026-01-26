using Godot;
using System;

public partial class VirtualCamera : Camera2D {

	public static Camera2D mainCamera;

	public override void _Ready() {
		base._Ready();

		if (mainCamera == null) return;

		this.GlobalPosition = mainCamera.GlobalPosition;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (mainCamera == null) return;

		this.GlobalPosition = mainCamera.GlobalPosition;
	}

}
