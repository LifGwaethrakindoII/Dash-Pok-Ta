using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private List<AudiosDictionary> audiosDictionary = new List<AudiosDictionary>(); 	/// <summary>Inspector's Collection od AudiosDictionary class.</summary>
	private Dictionary<string, AudioClip> _audios; 														/// <summary>Audios Dictionary.</summary>
	private AudioSource _audioSource; 																	/// <summary>AudioManager's AudioSource Component.</summary>

	[System.Serializable]
	public class AudiosDictionary 																		/// <summary>Inspector Audio's Dictionary.</summary>
	{
		public string audioKey; 																		/// <summary>AudiosDictionary's Audio Key ID.</summary>
		public AudioClip audioClip; 																	/// <summary>AudiosDictionary's AudioClip.</summary>
	}

	public enum PlayModes 																				/// <summary>AudioManager's Play Modes.</summary>
	{
		Unassigned,
		PlayOneShot,
		Play,
		Loop
	}

#region Getters/Setters:
	/// <summary>Gets and Sets audios property.</summary>
	public Dictionary<string, AudioClip> audios
	{
		get
		{
			if(_audios == null)
			{
				_audios = new Dictionary<string, AudioClip>();
			}

			return _audios;
		}

		set { _audios = value; }
	}

	/// <summary>Gets audioSource property.</summary>
	public AudioSource audioSource
	{
		get
		{
			if(_audioSource == null)
			{
				if(GetComponent<AudioSource>() != null)
				_audioSource = GetComponent<AudioSource>();
			}

			return _audioSource;
		}
	}
#endregion

	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
		else
		{
			CreateDictionary();
			DontDestroyOnLoad(gameObject);
		}
	}

	/// <summary>Creates Dictionary based on the Inspector's AudiosDictionary's Collection.</summary>
	private void CreateDictionary()
	{
		_audios = new Dictionary<string, AudioClip>();

		for(int i = 0; i < audiosDictionary.Count; i++)
		{
			_audios.Add(audiosDictionary[i].audioKey, audiosDictionary[i].audioClip);
		}
	}

	/// <summary>Plays Audioclip.</summary>
	/// <param name="_audioKey">The Key ID of the Audio.</param>
	/// <param name="_playMode">The Audio Play Mode.</param>
	public void PlayAudioClip(string _audioClip, PlayModes _playMode)
	{
		AudioClip audio = _audios[_audioClip];
		
		if(audio != null)
		{
			switch(_playMode)
			{
				case PlayModes.PlayOneShot:
				audioSource.PlayOneShot(audio);
				break;

				case PlayModes.Play:
				//audioSource.Play(audio);
				break;
			}
		}
		else Debug.LogError("Didn't found audio of Key " + _audioClip + " on Dictionary...");	
	}

	/// <summary>Gets Audio by the given Key ID.</summary>
	/// <param name="_audioKey">The Key ID of the Audio.</param>
	/// <returns>The Audio of the Key ID given.</returns>
	public AudioClip GetAudio(string _audioKey)
	{
		return _audios[_audioKey];
	}
}
