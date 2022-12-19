using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController
{
	private Dictionary<InputCommands, KeyCode> _inputMappings; 	/// <summary>Mappings of the Butons indexes.</summary>

	/// <summary>Input Commands.</summary>
	public enum InputCommands
	{
		Unassigned,
		HorizontalAxis,
		VerticalAxis,
		Dash,
		Jump,
		Run
	}

	/// <summary>Gets and Sets InputMappings property.</summary>
	public Dictionary<InputCommands, KeyCode> InputMappings
	{
		get { return _inputMappings; }
		set { _inputMappings = value; }
	}

	/// <summary>Sets InputMapping.</summary>
	/// <returns>Newly setted Input Mappings.</returns>
	public abstract Dictionary<InputCommands, KeyCode> SetInputMappings();

	/// <summary>Gets orientation axis.</summary>
	/// <param name="_axis">Evaluated axis.</param>
	/// <returns>Normalized axis orientatin.</returns>
	public abstract float GetAxis(InputCommands _axis);

	/// <summary>Gets input down.</summary>
	/// <param name="_axis">Evaluated input.</param>
	/// <returns>Input being down (boolean).</returns>
	public abstract bool GetInputDown(InputCommands _input);

	/// <summary>Gets input up.</summary>
	/// <param name="_axis">Evaluated input.</param>
	/// <returns>Input being up (boolean).</returns>
	public abstract bool GetInputUp(InputCommands _input);
}
