using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : Singleton<LoadingSceneManager>
{
	private Behavior asyncSceneLoader; 		/// <summary>Scene Loader Async Operation.</summary>
	private Behavior asyncScenesLoader; 	/// <summary>Scenes Loader Async Operation.</summary>
	private float _progress; 				/// <summary>Current progress of the Async Operation.</summary>
	private float timeLoadingScene; 		/// <summary>Total time that the Scene[s] from 0.0 to 0.9 of the progress</summary>

	/// <summary>Gets and Sets progress property.</summary>
	public float progress
	{
		get { return _progress; }
		set { _progress = value; }
	}

	public void LoadScene(string _scene)
	{
		asyncSceneLoader = new Behavior(this, WaitTillSceneLoads(_scene));
	}
	
	public void LoadScenes(params string[] _scenes)
	{
		asyncScenesLoader = new Behavior(this, WaitTillScenesLoads(_scenes));
	}
	
	/// <summary>Waits until the scene loads.</summary>
	/// <param name="_sceneLoading">Asychronous Scene to load.</param>
	IEnumerator WaitTillSceneLoads(string _sceneLoading)
	{
		timeLoadingScene = 0.0f;
		AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(_sceneLoading, LoadSceneMode.Single);
		sceneLoading.allowSceneActivation = false;

		yield return new WaitForSeconds(1f);

		while(sceneLoading.progress < 0.9f)
		{
			_progress = sceneLoading.progress;
			timeLoadingScene += Time.deltaTime;
			yield return null;
		}

		Debug.Log("[" + _sceneLoading + "]" + " took " + timeLoadingScene + " seconds to load.");
		
		yield return new WaitForSeconds(timeLoadingScene);
		sceneLoading.allowSceneActivation = true; //Once the process isDone, we activate the scene...
		//UnloadScene();
		
		asyncSceneLoader.EndBehavior();
	}

	/// <summary>Waits until scenes loads.</summary>
	/// <param name="_scenesLoading">Asychronous Scenes to load.</param>
	IEnumerator WaitTillScenesLoads(params string[] _scenesLoading)
	{
		timeLoadingScene = 0.0f;
		AsyncOperation[] scenesLoading = new AsyncOperation[_scenesLoading.Length];

		yield return new WaitForSeconds(1f);

		for(int i = 0; i < _scenesLoading.Length; i++)
		{
			scenesLoading[i] = SceneManager.LoadSceneAsync(_scenesLoading[i], LoadSceneMode.Additive);
			scenesLoading[i].allowSceneActivation = false;
		}

		while(!AllScenesLoaded(scenesLoading))
		{
			timeLoadingScene += Time.deltaTime;
			yield return null;
		}

		Debug.Log("[" + _scenesLoading + "]" + " took " + timeLoadingScene + " seconds to load.");
		
		yield return new WaitForSeconds(timeLoadingScene);
		for(int i = 0; i < _scenesLoading.Length; i++)
		{
			scenesLoading[i].allowSceneActivation = true;
		}
		//UnloadScene();

		asyncScenesLoader.EndBehavior();
	}

	/// <summary>Are all scene progresses at 90%?.</summary>
	/// <param name="_scenesLoading">Asychronous Scenes to load.</param>
	/// <returns>If All scenes are at least on 90%.</returns>
	private bool AllScenesLoaded(params AsyncOperation[] _scenesLoading)
	{
		float totalProgress = 0.0f;

		for(int i = 0; i < _scenesLoading.Length; i++)
		{
			totalProgress += _scenesLoading[i].progress;
			if(_scenesLoading[i].progress < 0.9f && !_scenesLoading[i].isDone) return false;
		}

		_progress = (totalProgress / _scenesLoading.Length);

		return true;
	}

	void UnloadScene()
	{
		SceneManager.UnloadScene("loadingScene");
		SceneManager.UnloadSceneAsync("loadingScene");
	}
}