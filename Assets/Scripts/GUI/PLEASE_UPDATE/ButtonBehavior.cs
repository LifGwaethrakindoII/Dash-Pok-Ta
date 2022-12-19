using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
	public GameObject game;
	private GameMechanics gm;

	// Use this for initialization
	void Start ()
	{
		gm = game.GetComponent<GameMechanics>();
	}
	
	public void resetGame()
	{
		switch(gm.getTime())
		{
			case 1:
			gm.repositionPlayersAndBall();
			break;

			case 0:
			Debug.Log("K...");
			SceneManager.LoadScene("Post Juego", LoadSceneMode.Single);
			break;
		}
	}
}
