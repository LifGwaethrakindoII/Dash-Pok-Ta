using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{

	// Use this for initialization
	void Start () 
	{
		PlayerPrefs.SetInt("Minutes", 3);
		PlayerPrefs.SetInt("Ai Goals", 0);
		PlayerPrefs.SetInt("Player Goals", 0);
		StartCoroutine("ChangeScene");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	IEnumerator ChangeScene()
	{
		
		yield return new WaitForSeconds(3.5f);
		SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
	}
}
