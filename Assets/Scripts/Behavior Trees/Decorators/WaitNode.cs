using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode<T> : DecoratorNode<T>
{
	private float _waitTime; /// <summary>Time the decorator waits before ticking the childs.</summary>
	private bool _timeCompleted; /// <summary>Flag activated when the Wait Time has been completed.</summary>

	/// <summary>Gets and Sets waitTime property.</summary>
	public float waitTime 
	{
		get { return _waitTime; }
		set { _waitTime = value; }
	}

	/// <summary>Gets and Sets timeCompleted property.</summary>
	public bool timeCompleted 
	{
		get { return _timeCompleted; }
		set { _timeCompleted = value; }
	}

	/// <summary>WaitNode Constructor.</summary>
	public WaitNode()
	{
		//...
	}

	/// <summary>Overload WaitNode Constructor.</summary>
	/// <param name="_leafChild">WaitNode's LeafTask child node.</param>
	/// <param name="_waitTime">WaitNode's waitTime value.</param>
	public WaitNode(LeafTask<T> _leafChild, float _waitTime)
	{
		this._waitTime = _waitTime;
		leafChild = _leafChild;
		AddNode(_leafChild);
	}

	/// <summary>WaitNode Destructor.</summary>
	~WaitNode()
	{
		//...
	}

	/// <summary>Waits for waitTime seconds, then returns a timeCompleted flag equal to true.</summary>
	IEnumerator Wait()
	{
		timeCompleted = false;
		yield return new WaitForSeconds(_waitTime);
		timeCompleted = true;
	}

	/// <summary>Waits waitTime seconds then Ticks the child.</summary>
	/// <returns>State of the single leafChild.</summary>
	public override States Tick()
	{
		Wait();

		while(!timeCompleted) { /*...*/ } return /*parentNode.state = */state = leafChild.Tick();
	}
}
