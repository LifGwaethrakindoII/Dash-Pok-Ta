using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CoroutineHolder : MonoBehaviour
{
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	IEnumerator changeSceneAfterTime(string _sceneName, float _time)
	{
		yield return new WaitForSeconds(_time);
		SceneManager.LoadScene(_sceneName);
	}
}
