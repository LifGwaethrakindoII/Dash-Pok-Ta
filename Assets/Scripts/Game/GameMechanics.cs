using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMechanics : MonoBehaviour
{
	public AudioSource goalSound;

	public Image thunderImage;

	private int times;
	public GameObject ball;

	private int playerScore;
	private int aiScore;

	private int minutes;
	private int seconds;

	private Timer timer;

	public Text playerScoreText;
	public Text aiScoreText;
	public Text retroText;

	public GameObject gameOverCanvasButton;

	public Text countdownText;

	public GameObject gameOverCanvas;

	//private PlayerTeamManager //ptm;
	//private EnemyTeamManager //etm;

	//private SelectedPlayerManager //spm;

	//Store the reference of the prefabs of the team members...
	public GameObject teotihuacanReference;
	public GameObject alienReference;

	private bool gameOn;
	Scene scene;

	bool counting;
	bool pause;

	public Sprite teotihuacanRace;
	public Sprite alienRace;

	public Image playerStats;
	public Image aiStats;

	//Pistiando y desordenando//
	/*public GameObject ArosAlienG;
	public GameObject ArosTeotiG;

	public GameObject ArosAlienP;
	public GameObject ArosTeotiP;

	Vector3 */

	//Pistiando y desordenando//

	void Awake()
	{
		counting = false;
		pause = false;
		//DontDestroyOnLoad(this.gameObject);		
	}

	void OnEnable()
	{

	}

	// Use this for initialization
	void Start()
	{
		scene = SceneManager.GetActiveScene();
		if(scene.name == "gameplayScene 1")
		{
			initValues();
		}

		/*scene = SceneManager.GetActiveScene();
		if(scene.name == "gameplayScene 1")
		{
			timer = this.GetComponent<Timer>();
			timer.enabled = false;
			times = 1;
	 		gameOn = false;
			countdownText.text = "Ready?";
			//ptm = this.GetComponent<PlayerTeamManager>();
			//etm = this.GetComponent<EnemyTeamManager>();
			//spm = this.GetComponent<SelectedPlayerManager>();
			minutes = PlayerPrefs.GetInt("Minutes");
			//minutes = 0;
			//seconds = 5;
			if(minutes == 0)
			{
				minutes = 3;
			}
			seconds = 0;

			gameOverCanvas.SetActive(false);

			string raceSelection  = PlayerPrefs.GetString("Race Selection");

			//Checking the team the player selected....
			if(raceSelection == "Teotihuacan")
			{

			}
			else if(raceSelection == "Alien")
			{

			}

			playerScore = PlayerPrefs.GetInt("Player Goals");
			aiScore = PlayerPrefs.GetInt("Ai Goals");
	 
			useStartCoroutine();
			//   Debug.Log("I am referencing the timer...");

			//Stop the players...
			//ptm.setTeamState("Unasigned");
			//etm.setTeamState("Unasigned");


			aiScoreText.text = aiScore.ToString();
			playerScoreText.text = playerScore.ToString();
		}*/
	}

	void initValues()
	{
		timer = this.GetComponent<Timer>();
		timer.enabled = false;
		times = 1;
	 	gameOn = false;
		countdownText.text = "Ready?";
		//ptm = this.GetComponent<PlayerTeamManager>();
		//etm = this.GetComponent<EnemyTeamManager>();
		//spm = this.GetComponent<SelectedPlayerManager>();
		minutes = PlayerPrefs.GetInt("Minutes");
		//minutes = 0;
		//seconds = 5;
		if(minutes == 0)
		{
			minutes = 3;
		}
		seconds = 0;

		gameOverCanvas.SetActive(false);

		string raceSelection  = PlayerPrefs.GetString("Race Selection");

		//Checking the team the player selected....
		if(raceSelection == "Teotihuacan")
		{
			playerStats.sprite = teotihuacanRace;
			aiStats.sprite = alienRace;
		}
		else if(raceSelection == "Alien")
		{
			playerStats.sprite = alienRace;
			aiStats.sprite = teotihuacanRace;
		}

		playerScore = PlayerPrefs.GetInt("Player Goals");
		aiScore = PlayerPrefs.GetInt("Ai Goals");
	 
	 	/*if(!counting)
		useStartCoroutine();*/
		Invoke("notIEStartRegressiveCount", 2);
		//  Debug.Log("I am referencing the timer...");

		//Stop the players...
		//ptm.setTeamState("Unasigned");
		//etm.setTeamState("Unasigned");


		aiScoreText.text = aiScore.ToString();
		playerScoreText.text = playerScore.ToString();
	}

	void useStartCoroutine()
	{
		StartCoroutine(startRegressiveCount());
	}

	void Update()
	{
		if(gameOn)
		{
			//Debug.Log("Should be running the time...");
			if(timer != null)
			timer.tick();
			else
			//Debug.Log("Me have no time...");

			if(Input.GetKeyDown("p") && pause)
	       	{
	            SceneManager.UnloadScene("Pausa");
	            pause = false;
	            //Time.timeScale = 1;
	       	}
	       	else if(Input.GetKeyDown("p") && !pause)
	       	{
	            pause = true;
	            SceneManager.LoadScene("Pausa", LoadSceneMode.Additive);
	            //Time.timeScale = 0;
	       	}
		}
	}

	public void repositionPlayersAndBall()
	{
		//ptm.setTeamState("Unasigned");
		//etm.setTeamState("Unasigned");

		//Time.timeScale = 1.0f;

		//gameOn = false;
		timer.setInitialTimeValues();
		//ptm.setOnInitialPositions();
		//etm.setOnInitialPositions();
		//ball.GetComponent<PlayersRayCast>().setInitialPosition();

		gameOverCanvas.SetActive(false);

		StartCoroutine(startRegressiveCount());
	}

	public void addScore(string _player)
	{
		goalSound.Play();
		switch(_player)
		{
			case "Player":
			PlayerPrefs.SetInt("Ai Goals", PlayerPrefs.GetInt("Ai Goals")+1);
			//aiScore++;
			aiScoreText.text = PlayerPrefs.GetInt("Ai Goals").ToString();
			break;

			case "AI":	
			PlayerPrefs.SetInt("Player Goals", PlayerPrefs.GetInt("Player Goals")+1);		
			//playerScore++;
			playerScoreText.text = PlayerPrefs.GetInt("Player Goals").ToString();
			break;
		}
	}

	public void resetGame()
	{
		SceneManager.LoadScene("gameplayScene 1",LoadSceneMode.Additive);
	}

	public void timeUp()
	{
		gameOn = false;

		//gameOverCanvas.SetActive(true);
		//Time.timeScale = 0.0f;
		times--;

		//Debug.Log("Time: " +times);

		switch(times)
		{
			case 1:
			gameOverCanvasButton.transform.GetChild(0).GetComponent<Text>().text = "To second Half";

			if(playerScore > aiScore)
			{
				retroText.text = "You are Winning";
			}

			if(aiScore > playerScore)
			{
				retroText.text = "You are Losing";
			}

			if(playerScore == aiScore)
			{
				retroText.text = "You are Tied";
			}
			break;

			case 0:
			gameOverCanvasButton.transform.GetChild(0).GetComponent<Text>().text = "To Menu";

			//DontDestroyOnLoad(this.gameObject);

			if(playerScore > aiScore)
			{
				SceneManager.LoadScene("Post Juego");
				//retroText.text = "You Won!";
			}

			if(aiScore > playerScore)
			{
				SceneManager.LoadScene("Post Juego");
				//retroText.text = "You Lost!";
			}

			if(playerScore == aiScore)
			{
				SceneManager.LoadScene("Post Juego");
				//retroText.text = "Tie!";
			}
			break;
		}
	}

	public int getTime()
	{
		return this.times;
	}

	public int getSeconds()
	{
		return this.seconds;
	}

	public int getMinutes()
	{
		return this.minutes;
	}

	public void setMinutes(int minutes)
	{
		this.minutes = minutes;
	}

	public bool getGameOn()
	{
		return this.gameOn;
	}

	public GameObject getBall()
	{
		return ball;
	}

	public void fadeThunderImage()
	{
		StartCoroutine(fadeThunder());		
	}

	IEnumerator fadeThunder()
	{
		float times = 3f;
		float duration = 1.5f;
		float normalizedTime = 0.0f;

		Color thunderColor = thunderImage.GetComponent<Image>().color;
		Color newColor;

		float minAlpha = 0.0f;
		float maxAlpha = 255f;
		float currentAlpha = minAlpha;

		bool lerpMax = true;

		while(normalizedTime < 1.0f && times != 0)
		{
			if(lerpMax)
			{
				currentAlpha = Mathf.Lerp(currentAlpha, maxAlpha, normalizedTime);

				if(currentAlpha >= maxAlpha)
				{
					normalizedTime = 0.0f;
					lerpMax = false;
					//times--;
				}
			}

			else if(!lerpMax)
			{
				currentAlpha = Mathf.Lerp(currentAlpha, minAlpha, normalizedTime);

				if(currentAlpha >= maxAlpha)
				{
					normalizedTime = 0.0f;
					lerpMax = true;
					times--;
				}
			}
			
			thunderColor.a = currentAlpha;

			thunderImage.GetComponent<Image>().color = thunderColor;

			normalizedTime += Time.deltaTime / duration;

			yield return null;
		}
	}

	IEnumerator startRegressiveCount()
	{
		counting = true;
		//Debug.Log("startRegressiveCount");
		yield return new WaitForSeconds(2f);
		countdownText.text = "Go!";
		StartCoroutine(endRegressiveCount());
		counting = false;
		StopCoroutine(startRegressiveCount());
	}

	IEnumerator endRegressiveCount()
	{
		//Debug.Log("endRegressiveCount");
		yield return new WaitForSeconds(1f);
		countdownText.text = "";
		////ptm.setActivateMovement(true);
		/*//spm.switchPlayer();
		//etm.setActivateMovement(true);*/
		timer.enabled = true;
		gameOn = true;
		//ptm.setTeamState("Idle");
		//etm.setTeamState("Idle");

		StopCoroutine(endRegressiveCount());

		//timer.tick();
	}

	void notIEStartRegressiveCount()
	{
		countdownText.text = "Go!";
		Invoke("notIEEndRegressiveCount", 1);
	}

	void notIEEndRegressiveCount()
	{
		countdownText.text = "";
		////ptm.setActivateMovement(true);
		/*//spm.switchPlayer();
		//etm.setActivateMovement(true);*/
		timer.enabled = true;
		gameOn = true;
		//ptm.setTeamState("Idle");
		//etm.setTeamState("Idle");
	}
}
