using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorPostJuego : MonoBehaviour 
{
	public Text AiScore;
	public Text PlayerScore;
	public GameObject AroAlien;
	public GameObject AroTeotihuacano;
	public GameObject AlienVictory;
	public GameObject GordoVictory;
	public GameObject Tie;
	public AudioSource teotihuacan_win, alien_win, empatesound;

	string teamSelection;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		AiScore.text = PlayerPrefs.GetInt("Ai Goals").ToString();
		PlayerScore.text = PlayerPrefs.GetInt("Player Goals").ToString();

		teamSelection = PlayerPrefs.GetString("Race Selection");

		if(PlayerPrefs.GetInt("Ai Goals") > PlayerPrefs.GetInt("Player Goals"))
		{
			
			if(teamSelection == "Alien")
			{
				AroAlien.SetActive(false);
				AroTeotihuacano.SetActive(true);
				AlienVictory.SetActive(false);
				GordoVictory.SetActive(true);
				Tie.SetActive(false);
				teotihuacan_win.Play();
			}
			else if(teamSelection == "Teotihuacan")
			{
				AroAlien.SetActive(true);
				AroTeotihuacano.SetActive(false);
				AlienVictory.SetActive(true);
				GordoVictory.SetActive(false);
				Tie.SetActive(false);
				alien_win.Play();
			}
			
		}
		else if(PlayerPrefs.GetInt("Ai Goals") < PlayerPrefs.GetInt("Player Goals"))
		{
			if(teamSelection == "Teotihuacan")
			{
				AroAlien.SetActive(false);
				AroTeotihuacano.SetActive(true);
				AlienVictory.SetActive(false);
				GordoVictory.SetActive(true);
				Tie.SetActive(false);
				teotihuacan_win.Play();
			}
			else if(teamSelection == "Alien")
			{
				AroAlien.SetActive(true);
				AroTeotihuacano.SetActive(false);
				AlienVictory.SetActive(true);
				GordoVictory.SetActive(false);
				Tie.SetActive(false);
				alien_win.Play();
			}
		}
		else if(PlayerPrefs.GetInt("Ai Goals") == PlayerPrefs.GetInt("Player Goals"))
		{
			AroAlien.SetActive(true);
			AroTeotihuacano.SetActive(true);
			AlienVictory.SetActive(false);
			GordoVictory.SetActive(false);
			Tie.SetActive(true);
			empatesound.Play();
		}
	}

	public void VolverAJugar()
	{
		PlayerPrefs.SetInt("Ai Goals", 0);
		PlayerPrefs.SetInt("Player Goals", 0);
		SceneManager.LoadScene("Loading y Controles");
		//SceneManager.LoadSceneAsync("loadingScene");
	}
	public void SalirAlMenu()
	{
		SceneManager.LoadScene("Main Menu",LoadSceneMode.Single);
		PlayerPrefs.SetInt("Ai Goals", 0);
		PlayerPrefs.SetInt("Player Goals", 0);
	}
}
