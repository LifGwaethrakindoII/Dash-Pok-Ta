using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
	/// <summary>Enters State.</summary>
	void Enter();

	/// <summary>Executes State.</summary>
	void Execute();

	/// <summary>Exits State.</summary>
	void Exit();
}

/*
public class FiniteStateMachine
{
	IState previousState;
	IState currentState;

	
}

public interface IPatrol : IState
{
	public IPatrol()
	{
		//...
	}

	public void Enter()
	{
	
	}

	public void Execute()
	{
	
	}

	public void Exit()
	{
	
	}
}
*/