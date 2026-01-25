using Godot;
using System;

public partial class EnemyController : Node {

	[Export] private Entity entity;
	[Export] private TilePathNode pathNode;
	[Export] private Label debug;

	private ViewCone viewCone;

	private enum EnemyState {
		PATROL,
		TRACK,
		ATTACK,
		IDLE,
		DISABLED
	}
	private EnemyState currentState;
	private float stateTimeRemaining = 0f;

	private Vector2 alertPoint;

	public override void _Ready() {
		base._Ready();

		if (pathNode != null) {
			currentState = EnemyState.PATROL;
			entity.MoveTo(TileUtils.GetTilePosition(pathNode.GlobalPosition));
		} else {
			currentState = EnemyState.IDLE;
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (entity.GetHealth() <= 0) {
			currentState = EnemyState.DISABLED;
		}

		switch (currentState) {
			case EnemyState.PATROL:
				OnPatrol();
				break;
			case EnemyState.TRACK:
				OnTrack((float) delta);
				break;
			case EnemyState.ATTACK:
				OnAttack((float) delta);
				break;
			case EnemyState.IDLE:
				OnIdle((float) delta);
				break;
		}

		float newLOD = 1 - Mathf.Clamp(entity.GlobalPosition.DistanceSquaredTo(PlayerInput.playerPosition) / 3_686_400f, 0, 1);
		viewCone.SetLOD(newLOD);
		debug.Text = $"STATE: {currentState}\nSTATE_TIME: {stateTimeRemaining}\nLOD: {Mathf.FloorToInt(newLOD * 100f)}";
	}

	public void AlertOfPlayer(Vector2 alertPoint) {
		if (currentState != EnemyState.TRACK && currentState != EnemyState.ATTACK && currentState != EnemyState.DISABLED) {
			currentState = EnemyState.TRACK;
			stateTimeRemaining = 4f;
		}
		this.alertPoint = alertPoint;
	}

	public void AssignViewCone(ViewCone vc) {
		this.viewCone = vc;
	}

	private void OnPatrol() {
		if (!entity.IsMoving() && pathNode != null) {

			TilePathNode nextNode = pathNode.GetNextNode();
			if (entity.MoveTo(TileUtils.GetTilePosition(nextNode.GlobalPosition))) {
				pathNode = nextNode;
			}
		}
	}

	private void OnTrack(float delta) {
		stateTimeRemaining -= delta;

		entity.FaceDirection(alertPoint - entity.GlobalPosition);

		if (stateTimeRemaining < 0) {
			if (viewCone.CanSeePlayer()) {
				currentState = EnemyState.ATTACK;
			} else {
				currentState = EnemyState.IDLE;
				stateTimeRemaining = 4f;
			}
		}
	}

	private void OnAttack(float delta) {
		stateTimeRemaining -= delta;

		entity.FaceDirection(alertPoint - entity.GlobalPosition);

		if (viewCone.CanSeePlayer()) {
			PlayerInput.player.Damage();

			currentState = EnemyState.TRACK;
			stateTimeRemaining = 2f;
		}
	}

	private void OnIdle(float delta) {
		stateTimeRemaining -= delta;

		if (stateTimeRemaining < 0) {
			currentState = EnemyState.PATROL;
		}
	}

}
