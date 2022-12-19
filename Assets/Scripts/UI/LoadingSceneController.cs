using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingSceneController : CoroutineHolder
{
	bool counting;

	void Awake()
	{
		counting = false;
		//DontDestroyOnLoad(this.gameObject);
	}
	//Scene scene;
	// Use this for initialization
	void Start()
	{
		Time.timeScale = 1.0f;
		//scene = SceneManager.GetActiveScene();
		DontDestroyOnLoad(gameObject);
		/*if(!counting)
		{
			counting = true;
			StartCoroutine(loadScene());
		}*/

		Invoke("loadLevel", 6);
	}

	void loadLevel()
	{
		SceneManager.LoadScene("gameplayScene 1");
	}

	void useStartCoroutine()
	{
		StartCoroutine(loadScene());
	}
	
	IEnumerator loadScene()
	{
		//counting = true;
		yield return new WaitForSeconds(6);
		//SceneManager.LoadSceneAsync("gameplayScene 1");
		counting = false;
		
		//StopCoroutine(loadScene());
		SceneManager.LoadScene("gameplayScene 1");
	}
}
