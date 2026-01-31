using Godot;
using System;

[GlobalClass]
public partial class SceneManager : Node {

	[Export(PropertyHint.File, "*.tscn")] private string transitionPath = "res://Prefabs/Transition.tscn";

	#region Singleton
	public static SceneManager Instance { get; private set; }
	public override void _EnterTree() {
		base._EnterTree();

		Instance = this;
	}
	#endregion

	public override void _Ready() {
		base._Ready();
	}

	public void LoadScene(string scenePath) {
		TransitionEffect transition = ResourceLoader.Load<PackedScene>(transitionPath).Instantiate<TransitionEffect>();

		transition.TransitionInComplete += () => FullLoadScene(scenePath, transition);
		transition.TransitionInComplete += transition.PlayTransitionOut;
		transition.TransitionOutComplete += () => { transition.QueueFree(); };

		GetTree().Root.AddChild(transition);

		transition.PlayTransitionIn();
	}

	public void LoadLevelScene(string scenePath) {
		LoadLevel.levelToLoad = scenePath;
		TransitionEffect transition = ResourceLoader.Load<PackedScene>(transitionPath).Instantiate<TransitionEffect>();

		transition.TransitionInComplete += () => FullLoadScene("res://Scenes/SceneContainer.tscn", transition);
		transition.TransitionInComplete += transition.PlayTransitionOut;
		transition.TransitionOutComplete += () => { transition.QueueFree(); };

		GetTree().Root.AddChild(transition);

		transition.PlayTransitionIn();
	}

	public void ReloadScene() {
		TransitionEffect transition = ResourceLoader.Load<PackedScene>(transitionPath).Instantiate<TransitionEffect>();

		transition.TransitionInComplete += FullReloadScene;
		transition.TransitionInComplete += transition.PlayTransitionOut;
		transition.TransitionOutComplete += () => { transition.QueueFree(); };

		GetTree().Root.AddChild(transition);

		transition.PlayTransitionIn();
	}

	private void FullLoadScene(string scenePath, TransitionEffect transition) {
		GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>(scenePath));
	}

	private void FullReloadScene() {
		if (GetTree().CurrentScene != null) GetTree().ReloadCurrentScene();
	}

	public void Quit() {

		TransitionEffect transition = ResourceLoader.Load<PackedScene>(transitionPath).Instantiate<TransitionEffect>();

		transition.TransitionInComplete += () => GetTree().Quit();

		GetTree().Root.AddChild(transition);

		transition.PlayTransitionIn();
	}
}
