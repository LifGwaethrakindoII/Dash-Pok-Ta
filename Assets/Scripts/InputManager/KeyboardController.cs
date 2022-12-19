using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : BaseController
{
	/// <summary>KeyboardController Constructor.</summary>
	public KeyboardController()
	{
		InputMappings = SetInputMappings();
	}

	/// <summary>(Override) Sets InputMapping.</summary>
	/// <returns>Newly setted Input Mappings.</returns>
	public override Dictionary<InputCommands, KeyCode> SetInputMappings()
	{
		Dictionary<InputCommands, KeyCode> newMappings = new Dictionary<InputCommands, KeyCode>();

#if UNITY_STANDALONE_WIN
		newMappings.Add(InputCommands.Dash, KeyCode.X);
		newMappings.Add(InputCommands.Run, KeyCode.Z);
		newMappings.Add(InputCommands.Jump, KeyCode.Space);
#endif

		return newMappings;
	}

	/// <summary>(Override) Gets orientation axis.</summary>
	/// <param name="_axis">Evaluated axis.</param>
	/// <returns>Normalized axis orientatin.</returns>
	public override float GetAxis(InputCommands _axis)
	{
		switch(_axis)
		{
			case InputCommands.HorizontalAxis:
			return Input.GetAxis("Horizontal");
			break;

			case InputCommands.VerticalAxis:
			return Input.GetAxis("Vertical");
			break;

			default:
			Debug.LogError(_axis.ToString() + " does not belong to axis range.");
			break;
		}

		return 0f;
	}

	/// <summary>(Override) Gets input down.</summary>
	/// <param name="_axis">Evaluated input.</param>
	/// <returns>Input being down (boolean).</returns>
	public override bool GetInputDown(InputCommands _input)
	{

		return false;
	}

	/// <summary>(Override) Gets input up.</summary>
	/// <param name="_axis">Evaluated input.</param>
	/// <returns>Input being up (boolean).</returns>
	public override bool GetInputUp(InputCommands _input)
	{

		return false;
	}
}
