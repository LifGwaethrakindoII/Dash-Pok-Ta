using UnityEngine;
using System.Collections;

public class VariableHolder : MonoBehaviour
{
	public bool gameOn;
	public string countdownText;
	public int times;

	void Awake()
	{
		this.gameObject.name = "persistentObject";
		DontDestroyOnLoad(this.gameObject);

		//GameMechanics Variables //
		gameOn = false;
		countdownText = "Ready?";
		times = 1;
	}

	void Start ()
	{
		
	}
}
