using Godot;
using System;
using System.Collections.Generic;

public partial class CameraController : Node2D {
	[Export] public Camera2D MainCamera { get; set; }

	private Vector2 _originalPosition;
	private bool _isAnimating = false;
	private float _animationDuration = 0f;
	private Vector2 _targetPosition;
	private float _pauseTime = 0f;
	private float _currentTime = 0f;
	private bool _movingToTarget = false;
	private bool _movingToOriginal = false;
	private bool _gameUnpaused = false;

	// Static instance for global access
	private static CameraController _instance;
	public static CameraController Instance => _instance;

	// Queue for multiple camera operations
	private Queue<CameraOperation> _operationQueue = new Queue<CameraOperation>();
	private bool _isProcessingQueue = false;

	private class CameraOperation {
		public Vector2 TargetPosition;
		public float PauseDuration;
		public Action OnComplete;

		public CameraOperation(Vector2 targetPosition, float pauseDuration) {
			TargetPosition = targetPosition;
			PauseDuration = pauseDuration;
		}
	}

	public override void _Ready() {
		if (MainCamera == null) {
			GD.PrintErr("CameraController: MainCamera must be assigned!");
			return;
		}

		_originalPosition = MainCamera.GlobalPosition;
		_instance = this;
	}

	public override void _Process(double delta) {
		if (_isAnimating) {
			_currentTime += (float) delta;

			if (_movingToTarget && _currentTime >= _pauseTime) {
				// Pause at target - unpausing the game so animations can play
				_movingToTarget = false;
				_movingToOriginal = true;
				_gameUnpaused = false; // Reset flag
				_currentTime = 0f;
				_animationDuration = 1.0f;
			} else if (_movingToOriginal && _currentTime >= _animationDuration) {
				// Animation complete
				_isAnimating = false;
				_currentTime = 0f;
				_movingToTarget = false;
				_movingToOriginal = false;
				_gameUnpaused = false;

				// Resume game
				GetTree().Paused = false;
				PlayerInput.allowInput = true;

				// Process next queued operation
				if (_operationQueue.Count > 0) {
					_isProcessingQueue = true;
					ProcessNextOperation();
				} else {
					_isProcessingQueue = false;
				}
				return;
			}

			// Handle game unpausing during pause at target
			if (_movingToTarget && !_gameUnpaused) {
				// Unpause the game during the pause time at target
				GetTree().Paused = false;
				_gameUnpaused = true;
			}

			// Only move when we're actually moving
			if (_movingToTarget || _movingToOriginal) {
				// Calculate interpolation for movement
				float t = _currentTime / _animationDuration;
				t = Mathf.Clamp(t, 0f, 1f);

				if (_movingToTarget) {
					// Move to target position
					Vector2 currentPosition = MainCamera.GlobalPosition;
					Vector2 newPosition = currentPosition.Lerp(_targetPosition, t);
					MainCamera.GlobalPosition = newPosition;
				} else if (_movingToOriginal) {
					// Return to original position
					Vector2 currentPosition = MainCamera.GlobalPosition;
					Vector2 newPosition = currentPosition.Lerp(_originalPosition, t);
					MainCamera.GlobalPosition = newPosition;
				}
			}
		} else if (_isProcessingQueue && _operationQueue.Count > 0) {
			// Process queued operations when not animating
			ProcessNextOperation();
		}
	}

	public void FocusCameraOn(Vector2 targetPosition, float pauseDuration) {
		if (_isAnimating) {
			// Queue the operation if currently animating
			_operationQueue.Enqueue(new CameraOperation(targetPosition, pauseDuration));
			return;
		}

		StartCameraAnimation(targetPosition, pauseDuration);
	}

	private void StartCameraAnimation(Vector2 targetPosition, float pauseDuration) {
		_originalPosition = MainCamera.GlobalPosition;
		PlayerInput.allowInput = false;

		_isAnimating = true;
		_movingToTarget = true;
		_movingToOriginal = false;
		_targetPosition = targetPosition;
		_pauseTime = pauseDuration;
		_animationDuration = 1.0f; // 1 second movement time
		_currentTime = 0f;
		_gameUnpaused = false;

		// Pause the game initially
		GetTree().Paused = true;
	}

	private void ProcessNextOperation() {
		if (_operationQueue.Count > 0) {
			var operation = _operationQueue.Dequeue();
			StartCameraAnimation(operation.TargetPosition, operation.PauseDuration);
		} else {
			_isProcessingQueue = false;
		}
	}

	// Static method to access the camera controller from anywhere
	public static void FocusCameraOnStatic(Vector2 targetPosition, float pauseDuration, Action onComplete = null) {
		if (Instance != null) {
			Instance.FocusCameraOn(targetPosition, pauseDuration);
		} else {
			GD.PrintErr("CameraController: Instance not available. Make sure CameraController is in the scene tree.");
		}
	}

	// Get current camera position
	public Vector2 GetCurrentPosition() {
		return MainCamera.GlobalPosition;
	}

	// Check if animation is in progress
	public bool IsAnimating() {
		return _isAnimating;
	}

	// Cleanup method
	public override void _ExitTree() {
		if (GetTree() != null) {
			GetTree().Paused = false;
		}
		_instance = null;
	}
}