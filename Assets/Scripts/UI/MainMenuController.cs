using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour 
{
		public AudioSource inbuttonsound;

	public void Jugar()
	{
		inbuttonsound.Play();
		SceneManager.LoadScene("Pre Juego", LoadSceneMode.Single);
	}

	public void Configuraciones()
	{
		inbuttonsound.Play();
		SceneManager.LoadScene("Configuracion", LoadSceneMode.Additive);
		
	}

	public void Creditos()
	{
		inbuttonsound.Play();
		SceneManager.LoadScene("Creditos",LoadSceneMode.Single);
	}
}
