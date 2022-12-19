using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSceneTransition : Singleton<BaseSceneTransition>
{
	[SerializeField] private float _sceneEnterTransitionDuration; 	/// <summary>Time the Scene Transition takes when the Scene starts.</summary>
	[SerializeField] private float _sceneExitTransitionDuration; 	/// <summary>Time the Scene Transition takes when the Scene ends.</summary>
	private Behavior _sceneEnterTransition; 						/// <summary>SceneEnterTransition Coroutine Controller.</summary>
	private Behavior _sceneExitTransition; 							/// <summary>SceneExitTransition Coroutine Controller.</summary>

	/// <summary>Gets sceneEnterTransitionDuration property.</summary>
	public float sceneEnterTransitionDuration
	{
		get { return _sceneEnterTransitionDuration; }
		set { _sceneEnterTransitionDuration = value; }
	}

	/// <summary>Gets sceneExitTransitionDuration property.</summary>
	public float sceneExitTransitionDuration
	{
		get { return _sceneExitTransitionDuration; }
		set { _sceneExitTransitionDuration = value; }
	}

	/// <summary>Gets sceneEnterTransition property.</summary>
	public Behavior sceneEnterTransition
	{
		get { return _sceneEnterTransition; }
		set { _sceneEnterTransition = value; }
	}

	/// <summary>Gets sceneExitTransition property.</summary>
	public Behavior sceneExitTransition
	{
		get { return _sceneExitTransition; }
		set { _sceneExitTransition = value; }
	}

	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
	}

	/// <summary>Calls sceneEnterTransitionBehavior [from outside script].</summary>
	public void StartSceneEnterTransition()
	{
		sceneEnterTransition = new Behavior(this, SceneEnterTransition());
	}

	/// <summary>Calls sceneExitTransitionBehavior [from outside script].</summary>
	public void StartSceneExitTransition()
	{
		sceneExitTransition = new Behavior(this, SceneExitTransition());
	}

	/// <summary>Scene Enter Transition Coroutine.</summary>
	public abstract IEnumerator SceneEnterTransition();

	/// <summary>Scene Exit Transition Coroutine.</summary>
	public abstract IEnumerator SceneExitTransition();
}
