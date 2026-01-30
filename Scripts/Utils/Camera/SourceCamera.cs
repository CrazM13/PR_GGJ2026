using Godot;
using System;

public partial class SourceCamera : Camera2D {

	public override void _EnterTree() {
		base._EnterTree();

		VirtualCamera.mainCamera = this;

	}

	public override void _ExitTree() {
		base._ExitTree();

		VirtualCamera.mainCamera = null;

	}

}
