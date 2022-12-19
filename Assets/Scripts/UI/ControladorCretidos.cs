using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControladorCretidos : MonoBehaviour
{
	public AudioSource outbuttonsound;
	public void Regresar()
	{
		outbuttonsound.Play();
		SceneManager.LoadScene("Main Menu",LoadSceneMode.Single);
	}
}
