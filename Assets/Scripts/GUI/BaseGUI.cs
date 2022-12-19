using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseGUI<T> : MonoBehaviour
{
	private T _data; 						/// <summary>Data that the 3DGUI is concerned with.</summary>
	private RectTransform _rectTransform; 	/// <summary>RectTransform Component.</summary>
	private Image _image; 					/// <summary>GameObject's Main Image. Use in case the GUI contains an Image, and/or override its Get/Set</summary>
	private Text _text; 					/// <summary>GameObject's Main Text. Use in case the GUI contains a Text, and/or override its Get/Set</summary>

	/// <summary>Gets rectTransform property.</summary>
	public RectTransform rectTransform
	{
		get
		{
			if(_rectTransform == null)
			{
				if(GetComponent<RectTransform>() != null)
				_rectTransform = GetComponent<RectTransform>();
			}

			return _rectTransform;
		}
	}

	/// <summary>Gets and Sets image property. Can be Overriden.</summary>
	public virtual Image image
	{
		get
		{
			if(_image == null)
			{
				if(GetComponent<Image>() != null)
				_image = GetComponent<Image>();
			}

			return _image;
		}
		set { _image = value; }
	}

	/// <summary>Gets and Sets text property. Can be Overriden.</summary>
	public virtual Text text
	{
		get
		{
			if(_text == null)
			{
				if(GetComponent<Text>() != null)
				_text = GetComponent<Text>();
			}

			return _text;
		}
		set { _text = value; }
	}

	/// <summary>Gests and Sets Data property.</summary>
	public T Data
	{
		get { return _data; }
		set
		{
			_data = value;
			OnDataAssigned();
		}
	}

	/// <summary>Method invoked when GUI's Data is assigned.</summary>
	protected virtual void OnDataAssigned(){}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_updatedData">Data that the GUI will recieve.</param>
	public abstract void UpdateGUI(T _updatedData);

	/// <summary>Shows (Activates) GUI.</summary>
	/// <param name="_show">Will the GUI be shown?.</param>
	public virtual void ShowGUI(bool _show)
	{
		gameObject.SetActive(_show);
	}
}
