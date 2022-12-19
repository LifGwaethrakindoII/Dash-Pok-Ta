using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : Singleton<ImageManager>
{
	[SerializeField] private List<SpritesDictionary> spritesDictionary = new List<SpritesDictionary>(); 	/// <summary>Inspector's Collection od SpritesDictionary class.</summary>
	private Dictionary<string, Sprite> _sprites; 															/// <summary>Audios Dictionary.</summary>

	[System.Serializable]
	public class SpritesDictionary 																			/// <summary>Inspector Sprite's Dictionary.</summary>
	{
		public string spriteKey; 																			/// <summary>SpritesDictionary's Sprite Key ID.</summary>
		public Sprite sprite; 																				/// <summary>SpritesDictionary's Sprite.</summary>
	}

#region Getters/Setters:
	/// <summary>Gets and Sets sprites property.</summary>
	public Dictionary<string, Sprite> sprites
	{
		get
		{
			if(_sprites == null)
			{
				_sprites = new Dictionary<string, Sprite>();
			}

			return _sprites;
		}

		set { _sprites = value; }
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

	/// <summary>Creates Dictionary based on the Inspector's SpritesDictionary's Collection.</summary>
	private void CreateDictionary()
	{
		_sprites = new Dictionary<string, Sprite>();
		
		for(int i = 0; i < spritesDictionary.Count; i++)
		{
			_sprites.Add(spritesDictionary[i].spriteKey, spritesDictionary[i].sprite);
		}
	}

	/// <summary>Gets Sprite by the given Key ID.</summary>
	/// <param name="_spriteKey">The Key ID of the Sprite.</param>
	/// <returns>The Sprite of the Key ID given.</returns>
	public Sprite GetSprite(string _spriteKey)
	{
		return _sprites[_spriteKey];
	}
	
}