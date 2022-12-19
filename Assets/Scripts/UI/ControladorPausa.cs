using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControladorPausa : MonoBehaviour 
{
	public void RegresarMenu()
	{
		SceneManager.LoadScene("Main Menu",LoadSceneMode.Single);
	}

	void OnEnable()
	{
		Time.timeScale = 0.0f;
	}

	void OnDisable()
	{
		Time.timeScale = 1.0f;
	}
}
