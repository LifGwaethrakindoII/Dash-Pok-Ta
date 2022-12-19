using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteStateMachine : MetaMonoBehaviour
{
	public enum States 								/// <summary>States enumerator.</summary>
	{
		Unassigned,
		Roaming,
		FollowBall,
		ReturnBall
	}; 

	private States _state;							/// <summary>Current State.</summary>
	public States initialState;						/// <summary>Initial State. Setted on Editor.</summary>

	//Sensors and Tasks:
	private Behavior _unassignedSensor; 			/// <summary>Sensor for Unassigned State.</summary>
	private Behavior _unassignedTask; 				/// <summary>Task for Unassigned State.</summary>
	private Behavior _roamingSensor; 				/// <summary>Sensor for Roaming State.</summary>
	private Behavior _roamingTask; 					/// <summary>Task for Roaming State.</summary>
	private Behavior _followBallSensor;				/// <summary>Sensor for FollowBall State.</summary>
	private Behavior _followBallTask; 				/// <summary>Task for FollowBall State.</summary>
	private Behavior _returnBallSensor;				/// <summary>Sensor for ReturnBall State.</summary>
	private Behavior _returnBallTask; 				/// <summary>Task for ReturnBall State.</summary>


#region Getters/Setters:
	/// <summary>Gets and Sets state property.</summary>
	public States state
	{
		get { return _state; }
		set
		{
			ExitState(_state); //First it exits the current State.
			EnterState(_state = value); //Then enters the newly assigned State.
		}
	}

	/// <summary>Gets and Sets unassignedSensor property.</summary>
	public Behavior unassignedSensor
	{
		get { return _unassignedSensor; }
		set { _unassignedSensor = value; }
	}

	/// <summary>Gets and Sets unassignedTask property.</summary>
	public Behavior unassignedTask
	{
		get { return _unassignedTask; }
		set { _unassignedTask = value; }
	}

	/// <summary>Gets and Sets roamingSensor property.</summary>
	public Behavior roamingSensor
	{
		get { return _roamingSensor; }
		set { _roamingSensor = value; }
	}

	/// <summary>Gets and Sets roamingTask property.</summary>
	public Behavior roamingTask
	{
		get { return _roamingTask; }
		set { _roamingTask = value; }
	}

	/// <summary>Gets and Sets followBallSensor property.</summary>
	public Behavior followBallSensor
	{
		get { return _followBallSensor; }
		set { _followBallSensor = value; }
	}

	/// <summary>Gets and Sets followBallTask property.</summary>
	public Behavior followBallTask
	{
		get { return _followBallTask; }
		set { _followBallTask = value; }
	}

	/// <summary>Gets and Sets returnBallSensor property.</summary>
	public Behavior returnBallSensor
	{
		get { return _returnBallSensor; }
		set { _returnBallSensor = value; }
	}

	/// <summary>Gets and Sets returnBallTask property.</summary>
	public Behavior returnBallTask
	{
		get { return _returnBallTask; }
		set { _returnBallTask = value; }
	}
#endregion

	/// <summary>Enters new State.</summary>
	/// <param name="_state">The new State.</param>
	public abstract void EnterState(States _state);

	/// <summary>Exits State. Ends Behaviors of respective State.</summary>
	/// <param name="_state">The State that will be left.</param>
	public void ExitState(States _state)
	{
		switch(state)
		{
			case States.Unassigned:
			if(_unassignedSensor != null) _unassignedSensor.EndBehavior();
			if(_unassignedTask != null) _unassignedTask.EndBehavior();
			break;

			case States.Roaming:
			if(_roamingSensor != null) _roamingSensor.EndBehavior();
			if(_roamingTask != null) _roamingTask.EndBehavior();
			break;

			case States.FollowBall:
			if(_followBallSensor != null) _followBallSensor.EndBehavior();
			if(_followBallTask != null) _followBallTask.EndBehavior();
			break;

			case States.ReturnBall:
			if(_returnBallSensor != null) _returnBallSensor.EndBehavior();
			if(_returnBallTask != null) _returnBallTask.EndBehavior();
			break;

			default:
			Debug.LogError("State " + state.ToString() + " not yet on switch checking. Update FiniteStateMachine class.");
			break;
		}

		Debug.Log("Left State: " + _state.ToString());
	}
}