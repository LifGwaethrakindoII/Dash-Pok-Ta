using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneAdministrator : Singleton<SceneAdministrator>
{
	private const string LoadingScene = "LoadingScene"; 		/// <summary>Loading Scene Name.</summary>
	private List<string> _activeScenes = new List<string>(); 	/// <summary>List of Active Scenes.</summary>
	private string _sceneToLoad; 								/// <summary>Stores the Scene to load.</summary>
	private string _currentSceneName; 							/// <summary>Current Scene's name.</summary>
	private int _currentSceneIndex; 							/// <summary>Current Scene's name.</summary>
	private int _sceneCount; 									/// <summary>Scene Count on Build Settings.</summary>
	private bool _hasNextScee; 									/// <summary>Has the Game a Next Scene.</summary>
	private LoadTypes _loadType; 								/// <summary>Stores the current Load Type.</summary>
	private Behavior loadScene; 								/// <summary>LoadScene Coroutine Controller.</summary>

	/// <summary>Load Scene Types.</summmary>
	public enum LoadTypes
	{
		Unassigned, 											/// <summary>Unassigned Load Scene Type.</summmary>
		Single, 												/// <summary>Single Load Scene Type.</summmary>
		Multiple 												/// <summary>Unassigned Load Scene Type.</summmary>
	}

#region Getters/Setters:	
	/// <summary>Gets and Sets sceneToLoad property.</summary>
	public string sceneToLoad
	{
		get { return _sceneToLoad; }
		set { SaveSystem.SaveData("SceneToLoad", _sceneToLoad = value); }
	}

	/// <summary>Gets currentSceneName property.</summary>
	public string currentSceneName
	{
		get { return _currentSceneName = SceneManager.GetActiveScene().name; }
	}

	/// <summary>Gets currentSceneIndex property.</summary>
	public int currentSceneIndex
	{
		get { return _currentSceneIndex = SceneManager.GetActiveScene().buildIndex; }
	}

	/// <summary>Gets sceneCount property.</summary>
	public int sceneCount
	{
		get { return _sceneCount = SceneManager.sceneCountInBuildSettings; }
	}

	/// <summary>Gets hasNextScene property.</summary>
	public bool hasNextScene
	{
		get { return _hasNextScee = (currentSceneIndex < (sceneCount - 1)); }
	}
#endregion

#region UnityMethods:
	/*void OnEnable()
	{
		SceneManager.sceneLoaded += SceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= SceneLoaded;
	}

	void SceneLoaded(Scene _scene, LoadSceneMode _mode)
	{
		switch(_scene.name)
		{
			case "LoadingScene":
			break;

			default:
			break;
		}
	}*/

	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
		else DontDestroyOnLoad(gameObject);
	}
#endregion


#region Methods:
	/// <summary>Gets Scene Name by Build Index.</summary>
	/// <param name="_index">Index of the gotten Scene.</summmary>
	/// <returns>Scene's name by index.</returns>
	public string GetSceneByBuildIndex(int _index)
	{
		return SceneManager.GetSceneByBuildIndex(_index).name;
	}

	/// <summary>Goes to Next Scene [If there is].</summary>
	public void GoToNextScene()
	{
		if(hasNextScene) LoadScene(GetSceneByBuildIndex(currentSceneIndex + 1));
	}

	/// <summary>Loads Scene.</summary>
	/// <param name="_scene">Scene to load.</summmary>
	public void LoadScene(string _scene)
	{
		SaveSystem.SaveData("SceneToLoad", _scene);
		loadScene = new Behavior(this, WaitTillLoadSceneIsReady(LoadTypes.Single));
	}

	/// <summary>Loads Scenes.</summary>
	/// <param name="_scenes">Scene to load.</summmary>
	public void LoadScenes(params string[] _scenes)
	{
		SaveSystem.SaveData("ScenesToLoad", _scenes);
		loadScene = new Behavior(this, WaitTillLoadSceneIsReady(LoadTypes.Multiple));
	}

	/// <summary>Reloads Scene.</summary>
	public void ReloadScene()
	{
		SaveSystem.SaveData("SceneToLoad", _currentSceneName);
		loadScene = new Behavior(this, WaitTillLoadSceneIsReady(LoadTypes.Single));
	}

	/// <summary>Waits until LoadingScene loads.</summary>
	/// <param name="_loadType">Load Type.</param>
	IEnumerator WaitTillLoadSceneIsReady(LoadTypes _loadType)
	{
		float timeLoadingScene = 0.0f;
		AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(LoadingScene, LoadSceneMode.Single);
		sceneLoading.allowSceneActivation = false;

		if(BaseSceneTransition.Instance != null)
		{//If there is a SceneTransition Object, enable its SceneExitTransition.
			BaseSceneTransition.Instance.StartSceneExitTransition();
			yield return new WaitForSeconds(BaseSceneTransition.Instance.sceneExitTransitionDuration);
		}

		while(sceneLoading.progress < 0.9f)
		{
			timeLoadingScene += Time.deltaTime;
			yield return null;
		}

		Debug.Log("[" + LoadingScene + "]" + " took " + timeLoadingScene + " seconds to load.");
		
		yield return new WaitForSeconds(timeLoadingScene);
		sceneLoading.allowSceneActivation = true;

		while(LoadingSceneManager.Instance == null)
		{
			yield return null;
		}

		switch(_loadType)
		{
			case LoadTypes.Single:
			LoadingSceneManager.Instance.LoadScene(SaveSystem.LoadString("SceneToLoad"));
			break;

			case LoadTypes.Multiple:
			LoadingSceneManager.Instance.LoadScenes(SaveSystem.LoadStringArray("ScenesToLoad"));
			break;

			default:
			break;
		}

		/*SceneManager.UnloadScene(currentSceneName);
		SceneManager.UnloadSceneAsync(currentSceneName);*/

		loadScene.EndBehavior();
	}
#endregion
}
