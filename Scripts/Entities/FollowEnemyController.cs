using Godot;
using System;
using System.Collections.Generic;

public partial class FollowEnemyController : Node {

	[Export] private Entity entity;
	[Export] private Area2D view;
	[Export, ExportGroup("Audio")] private AudioStreamPlayer2D gunSFX;

	private enum EnemyState {
		TRACK,
		ATTACK,
		IDLE,
		DISABLED
	}
	private EnemyState currentState;

	private Vector2 alertPoint;

	public override void _Ready() {
		base._Ready();

		currentState = EnemyState.IDLE;

		view.BodyEntered += (body) => { AlertOfPlayer(body.GlobalPosition); };
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (entity.GetHealth() <= 0) {
			currentState = EnemyState.TRACK;
			entity.SetHealth(1);
		}

		switch (currentState) {
			case EnemyState.TRACK:
				OnTrack();
				break;
			case EnemyState.ATTACK:
				OnAttack();
				break;
			case EnemyState.IDLE:
				OnIdle();
				break;
		}
	}

	public void AlertOfPlayer(Vector2 alertPoint) {
		if (currentState != EnemyState.ATTACK && currentState != EnemyState.DISABLED) {
			currentState = EnemyState.ATTACK;
		}

		this.alertPoint = alertPoint;
	}

	private void OnTrack() {

		if (!entity.IsMoving()) {
			Vector2 playerPosition = PlayerInput.playerPosition;
			Vector2 entityPosition = entity.GlobalPosition;

			// Get tile positions
			Vector2 entityTile = TileUtils.GetTilePosition(entityPosition);
			Vector2 playerTile = TileUtils.GetTilePosition(playerPosition);

			Vector2 direction = Vector2.Zero;

			if (entityTile.X < playerTile.X) {
				direction += Vector2.Right;
			} else if (entityTile.X > playerTile.X) {
				direction -= Vector2.Right;
			}

			if (entityTile.Y < playerTile.Y) {
				direction -= Vector2.Up;
			} else if (entityTile.Y > playerTile.Y) {
				direction += Vector2.Up;
			}

			if (direction != Vector2.Zero) {
				entity.MoveTo(entityTile + direction);
			}
		}

		if (PlayerInput.inventory.GetStoredItem() == null) {
			currentState = EnemyState.IDLE;
		}
	}

	private void OnAttack() {
		entity.FaceDirection(alertPoint - entity.GlobalPosition);

		PlayerInput.player.Damage();
		PlayerInput.player.Knockback(entity.GlobalPosition - alertPoint, 0.01f);
		gunSFX.Play();
	}

	private void OnIdle() {
		if (PlayerInput.inventory.GetStoredItem() != null) {
			currentState = EnemyState.TRACK;
		}
	}

}
