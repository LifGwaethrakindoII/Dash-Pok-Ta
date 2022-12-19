using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseButton : MonoBehaviour
{
	[SerializeField] private ButtonActions _buttonAction; /// <summary>Button individual Action.</summary>
	public enum ButtonActions /// <summary>Button Actions.</summary>
	{
		Unasigned,
		GoToJosQuest, //Temporal functionallity for scene change testing.
		AddPlayer //Add players to the corresponding Team Manager.
	};

	/// <summary>Gets and Sets buttonAction property.</summary>
	public ButtonActions buttonAction
	{
		get { return _buttonAction; }
		set { _buttonAction = value; }
	}

	/// <summary>Excecutes the ButtonAction of the button.</summary>
	public abstract void ExcecuteAction();
}
