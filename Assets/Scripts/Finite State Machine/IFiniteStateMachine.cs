using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFiniteStateMachine<T>
{
	/// <summary>Enters T State.</summary>
	/// <param name="_state">T State that will be entered.</param>
	void EnterState(T _state);

	/// <summary>Exited T State.</summary>
	/// <param name="_state">T State that will be left.</param>
	void ExitState(T _state);
}