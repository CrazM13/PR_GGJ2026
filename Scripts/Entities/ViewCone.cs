using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class ViewCone : Area2D {

	[Export] private EnemyController enemy;
	[Export] private Polygon2D display;
	[Export] private CollisionPolygon2D collisionPolygon;

	[Export] private float viewDistance = 512f;
	[Export] private float viewAngle = Mathf.DegToRad(60); // Total angle
	[Export] private bool updating = false;

	[Export] private int maxQuality = 100; // Number of points for the cone edge
	[Export] private int minQuality = 5; // Number of points for the cone edge

	private bool playerInSight;

	private int realQuality;
	private bool shouldUpdate;

	public override void _Ready() {
		realQuality = maxQuality;
		enemy.AssignViewCone(this);

		shouldUpdate = true;

		// Connect signals
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (playerInSight) {
			enemy.AlertOfPlayer(PlayerInput.playerPosition);
		}

		if (shouldUpdate) {
			UpdateVisionCone();
			shouldUpdate = updating;
		}

	}

	private void UpdateVisionCone() {
		List<Vector2> points = [ Vector2.Zero ];

		PhysicsDirectSpaceState2D spaceState = GetWorld2D().DirectSpaceState;

		for (int i = 0; i <= realQuality; i++) {
			
			float angle = -viewAngle / 2 + (viewAngle / realQuality) * i;
			Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			direction = direction.Rotated(GlobalRotation);

			Vector2 endPoint = direction * viewDistance;

			PhysicsRayQueryParameters2D rayParams = new() {
				From = GlobalPosition,
				To = GlobalPosition + endPoint,
				CollideWithBodies = true,
				CollisionMask = 512,
				HitFromInside = false
			};

			// Check for collision along the ray
			Dictionary rayResult = spaceState.IntersectRay(rayParams);

			// If there's a collision, use the collision point instead of the full distance
			if (rayResult != null && rayResult.ContainsKey("position")) {
				Vector2 collisionPoint = (Vector2) rayResult["position"];
				endPoint = collisionPoint - GlobalPosition;
			}

			points.Add(endPoint);
		}

		collisionPolygon.Polygon = [.. points];
		UpdateViewConeDisplay();
	}

	private void OnBodyExited(Node2D body) {
		playerInSight = false;
		HUD.HiddenLevel--;
	}

	private void OnBodyEntered(Node2D body) {
		playerInSight = true;
		HUD.HiddenLevel++;
	}

	private void UpdateViewConeDisplay() {
		if (collisionPolygon.Polygon.Length > 0) {
			Vector2[] newShape = new Vector2[collisionPolygon.Polygon.Length];
			for (int i = 0; i < collisionPolygon.Polygon.Length; i++) {
				newShape[i] = collisionPolygon.Polygon[i];
			}

			display.Polygon = newShape;
			collisionPolygon.Rotation = display.Rotation = -this.GlobalRotation;
		}
	}

	public void SetLOD(float lod) {
		realQuality = Mathf.FloorToInt(((maxQuality - minQuality) * lod) + minQuality);
	}

	public bool CanSeePlayer() {
		return this.playerInSight;
	}
}
