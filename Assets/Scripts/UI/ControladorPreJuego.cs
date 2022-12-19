using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorPreJuego : MonoBehaviour 
{
	public Button Atras;
	public Button Adelante;
	public GameObject AroAlien;
	public GameObject AroTeotihuacano;
	private string playerTeamSelection; //Stores the selection of the race the player has chosen, that wiil be then stored into the playerprefs...
	public GameObject TxtAlien;
	public GameObject TxtTeotihuacano;
	public AudioSource prestartsound, outbutsound, changestatsound;

	void Start()
	{
		Atras.interactable = false;
		AroAlien.SetActive(false);
		AroTeotihuacano.SetActive(true);
		playerTeamSelection = "Teotihuacan"; //As the interface begins, the first option appearing is the teotihuacan race, so thats the default value...
		TxtAlien.SetActive(false);
		TxtTeotihuacano.SetActive(true);
	}


	public void EquipoAnterior()
	{
		changestatsound.Play();
		Adelante.interactable = true;
		//Debug.Log("Equipo Teotihuacano activado");
		Atras.interactable = false;
		AroAlien.SetActive(false);
		TxtAlien.SetActive(false);
		TxtTeotihuacano.SetActive(true);
		AroTeotihuacano.SetActive(true);
		playerTeamSelection = "Teotihuacan";
	}

	public void EquipoSiguiente()
	{
		changestatsound.Play();
		Atras.interactable = true;
		//Debug.Log("Equipo Alien activado");
		Adelante.interactable = false;
		AroAlien.SetActive(true);
		TxtAlien.SetActive(true);
		TxtTeotihuacano.SetActive(false);
		AroTeotihuacano.SetActive(false);
		playerTeamSelection = "Alien";
	}

	public void ComenzarJuego()
	{
		prestartsound.Play();
		PlayerPrefs.SetInt("Ai Goals", 0);
		PlayerPrefs.SetInt("Player Goals", 0);
		PlayerPrefs.SetString("Race Selection", playerTeamSelection);

		//Debug.Log("Race selected: " + PlayerPrefs.GetString("Race Selection"));

		SceneManager.LoadScene("Loading y Controles", LoadSceneMode.Single);
		//SceneManager.LoadSceneAsync("Loading y Controles", LoadSceneMode.Single);
	}

	public void Regresar()
	{
		outbutsound.Play();
		SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
	}

}
