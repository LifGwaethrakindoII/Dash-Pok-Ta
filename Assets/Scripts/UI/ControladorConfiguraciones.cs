using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorConfiguraciones : MonoBehaviour 
{
	public GameObject tiempo;
	int tiempo2;
	public AudioSource ChangeStatSound, outbuttonsound;

	void Start()
	{
		tiempo2 = PlayerPrefs.GetInt("Minutes");
		tiempo.GetComponent<Text>().text = tiempo2 + " Mins";
	}

	public void Regresar()
	{
		outbuttonsound.Play();
		SceneManager.UnloadScene("Configuracion");
	}

	public void MasTiempo()
	{
		ChangeStatSound.Play();
		PlayerPrefs.SetInt("Minutes" ,PlayerPrefs.GetInt("Minutes")+1);
		tiempo.GetComponent<Text>().text = PlayerPrefs.GetInt("Minutes").ToString() +" Mins";
		if(PlayerPrefs.GetInt("Minutes") >= 5)
		{
			PlayerPrefs.SetInt("Minutes" , 5);
			tiempo.GetComponent<Text>().text = PlayerPrefs.GetInt("Minutes").ToString() +" Mins";
		}
	}

	public void MenosTiempo()
	{
		ChangeStatSound.Play();
		PlayerPrefs.SetInt("Minutes" ,PlayerPrefs.GetInt("Minutes")-1);
		tiempo.GetComponent<Text>().text = PlayerPrefs.GetInt("Minutes").ToString() +" Mins";
		if(PlayerPrefs.GetInt("Minutes") <= 1)
		{
			PlayerPrefs.SetInt("Minutes" , 1);
			tiempo.GetComponent<Text>().text = PlayerPrefs.GetInt("Minutes").ToString() +" Mins";
		}
	}
}
