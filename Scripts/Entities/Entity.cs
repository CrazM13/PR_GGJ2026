using Godot;
using System;

public partial class Entity : CharacterBody2D {
	[Export] private float BaseSpeed { get; set; } = 100.0f;
	[Export] private float BounceDistance { get; set; } = 16.0f;
	[Export] private int MaxHealth { get; set; } = 1;
	[Export] private bool SlowTurn { get; set; } = false;
	[Export] private float TurnSpeed { get; set; } = 5.0f;

	private Vector2 facingDirection = Vector2.Right;
	private Vector2 targetPosition;
	private Vector2 startingPosition;
	private bool isMoving = false;
	private float moveTimer = 0.0f;

	private bool shouldBounce = false;
	private bool isKnockedBack = false;
	private bool isTurning = false;
	private Vector2 targetFacingDirection = Vector2.Right;

	private Vector2 knockbackDirection = Vector2.Zero;
	private float knockbackStrength = 0.0f;
	private float knockbackDuration = 0.0f;
	private float knockbackTimer = 0.0f;

	private int health;

	private const float BOUNCE_DURATION = 0.2f;
	private const float KNOCKBACK_EASE_POWER = 3;
	private const float MODULATE_FADE_SPEED = 10f;

	public override void _Ready() {
		SnapToGrid();
		health = MaxHealth;
	}

	public override void _Process(double delta) {
		UpdateVisualEffects(delta);

		if (shouldBounce) {
			HandleBounceAnimation(delta);
		} else if (isKnockedBack) {
			HandleKnockback(delta);
		} else if (isTurning) {
			HandleTurn(delta);
		} else if (isMoving) {
			HandleMovement(delta);
		}
	}

	private void UpdateVisualEffects(double delta) {
		if (Modulate != Colors.White) {
			Modulate = Modulate.Lerp(Colors.White, (float) delta * MODULATE_FADE_SPEED);
		}
	}

	private void HandleBounceAnimation(double delta) {
		moveTimer += (float) delta;
		float bounceProgress = moveTimer / BOUNCE_DURATION;

		if (bounceProgress >= 1.0f) {
			CompleteBounce();
		} else {
			float bounceOffset = Mathf.Sin(bounceProgress * Mathf.Pi) * BounceDistance;
			GlobalPosition = startingPosition + (facingDirection * bounceOffset);
		}
	}

	private void CompleteBounce() {
		shouldBounce = false;
		moveTimer = 0.0f;
		GlobalPosition = startingPosition;
	}

	private void HandleKnockback(double delta) {
		knockbackTimer += (float) delta;

		if (knockbackTimer >= knockbackDuration) {
			CompleteKnockback();
		} else {
			ApplyKnockbackMovement();
		}
	}

	private void CompleteKnockback() {
		isKnockedBack = false;
		knockbackTimer = 0.0f;
		GlobalPosition = startingPosition;
	}

	private void ApplyKnockbackMovement() {
		float progress = knockbackTimer / knockbackDuration;
		float easeProgress = 1 - Mathf.Pow(1 - progress, KNOCKBACK_EASE_POWER);

		Vector2 knockbackPosition = startingPosition - (knockbackDirection * knockbackStrength * easeProgress);
		GlobalPosition = knockbackPosition;
	}

	private void HandleTurn(double delta) {
		// Calculate the angle difference between current and target direction
		float currentAngle = Mathf.Atan2(facingDirection.Y, facingDirection.X);
		float targetAngle = Mathf.Atan2(targetFacingDirection.Y, targetFacingDirection.X);
		
		// Calculate the shortest angle difference
		float angleDiff = targetAngle - currentAngle;
		
		// Normalize the angle difference to [-π, π]
		while (angleDiff > Mathf.Pi) angleDiff -= 2 * Mathf.Pi;
		while (angleDiff < -Mathf.Pi) angleDiff += 2 * Mathf.Pi;
		
		// Calculate turn speed based on TurnSpeed export
		float turnAmount = angleDiff * (float)delta * TurnSpeed;
		
		// Check if we've completed the turn
		if (Mathf.Abs(angleDiff) < 0.01f || Mathf.Abs(turnAmount) >= Mathf.Abs(angleDiff)) {
			// Complete the turn
			facingDirection = targetFacingDirection;
			Rotation = Mathf.Atan2(targetFacingDirection.Y, targetFacingDirection.X);
			isTurning = false;
			
			// Start movement after turning is complete
			if (isMoving) {
				SetupMovement(targetPosition);
			}
		} else {
			// Continue turning
			Rotation += turnAmount;
			facingDirection = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
		}
	}

	private void HandleMovement(double delta) {
		moveTimer += (float) delta;
		float progress = moveTimer * (BaseSpeed / TileUtils.TileSize);

		if (progress >= 1.0f) {
			FinishMovement();
		} else {
			GlobalPosition = startingPosition.Lerp(targetPosition, progress);
		}
	}

	/// <summary>
	/// Move the entity towards a target position in tile coordinates
	/// </summary>
	/// <param name="targetTile">Target tile position as Vector2</param>
	public bool MoveTo(Vector2 targetTile) {
		Vector2 targetPixel = TileUtils.GetCenterPosition(targetTile);

		if (targetPixel == targetPosition)
			return true;

		Vector2 direction = (targetPixel - GlobalPosition).Normalized();
		
		// Check if we need to turn
		bool needsTurn = SlowTurn && direction != facingDirection;
		
		if (needsTurn) {
			// Start turning first
			targetFacingDirection = direction;
			isTurning = true;
			isMoving = false;
			// Store the target for later use
			targetPosition = targetPixel;
			startingPosition = GlobalPosition;
			return true;
		} else {
			FaceDirection(direction);
			
			if (IsPositionValid(targetPixel)) {
				SetupMovement(targetPixel);
				return true;
			} else {
				PerformBounceAnimation(targetPixel);
				return false;
			}
		}
	}

	private void SetupMovement(Vector2 targetPixel) {
		targetPosition = targetPixel;
		startingPosition = GlobalPosition;
		isMoving = true;
		moveTimer = 0.0f;
	}

	private void PerformBounceAnimation(Vector2 targetPosition) {
		startingPosition = GlobalPosition;
		shouldBounce = true;
		moveTimer = 0.0f;
		isMoving = false;
	}

	/// <summary>
	/// Instantly warp the entity to a new position without movement animation
	/// </summary>
	/// <param name="targetTile">Target tile position as Vector2</param>
	public void WarpTo(Vector2 targetTile) {
		Vector2 targetPixel = TileUtils.GetCenterPosition(targetTile);

		if (IsPositionValid(targetPixel)) {
			GlobalPosition = targetPixel;
			SnapToGrid();
		} else {
			ReturnToCurrentTile();
		}
	}

	/// <summary>
	/// Face a specific direction
	/// </summary>
	/// <param name="direction">Direction vector to face</param>
	public void FaceDirection(Vector2 direction) {
		facingDirection = direction;

		if (direction != Vector2.Zero) {
			Rotation = Mathf.Atan2(direction.Y, direction.X);
		}
	}

	public void Knockback(Vector2 direction, float strength, float duration = 0.3f) {
		if (isKnockedBack)
			return;

		knockbackDirection = direction;
		knockbackStrength = strength;
		knockbackDuration = duration;
		knockbackTimer = 0.0f;
		startingPosition = GlobalPosition;
		isKnockedBack = true;
		isMoving = false;
		shouldBounce = false;
	}

	private bool IsPositionValid(Vector2 position) {
		Vector2 originalPosition = GlobalPosition;
		GlobalPosition = position;
		bool hasCollision = MoveAndCollide(Vector2.Zero, true) != null;
		GlobalPosition = originalPosition;
		return !hasCollision;
	}

	private void ReturnToCurrentTile() {
		Vector2 currentTile = GetTilePosition();
		Vector2 targetPixel = TileUtils.GetCenterPosition(currentTile);
		GlobalPosition = targetPixel;
		isMoving = false;
		moveTimer = 0.0f;
	}

	private void FinishMovement() {
		GlobalPosition = targetPosition;
		SnapToGrid();
		isMoving = false;
		moveTimer = 0.0f;
	}

	private void SnapToGrid() {
		Vector2 tilePosition = GetTilePosition();
		GlobalPosition = TileUtils.GetCenterPosition(tilePosition);
	}

	public Vector2 GetTilePosition() {
		return TileUtils.GetTilePosition(GlobalPosition);
	}

	public Vector2 GetFacingDirection() {
		return facingDirection;
	}

	public bool IsMoving() {
		return isMoving || shouldBounce || isTurning;
	}

	public void Damage() {
		health -= 1;
		Modulate = Colors.Red;
	}

	public int GetHealth() {
		return health;
	}

	internal void SetHealth(int v) {
		health = v;
	}
}