using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitTaskNode<T> : DecoratorNode<T>
{
	private int timesLimit;			 ///<summary>Number of times the leafChild can be executed.</summary>
	private int currentTimeIterator; /// <summary>Current iterator of times the leafChild Tick has been repeated.</summary>

	/// <summary>LimitTaskNode Constructor.</summary>
	public LimitTaskNode()
	{
		//...
	}

	/// <summary>Overload LimitTaskNode Constructor.</summary>
	/// <paranm name="_leafChild">LimitTaskNode's leafChild.</summary>
	/// <paranm name="_timesLimit">LimitTaskNode's timesLimit.</summary>
	public LimitTaskNode(LeafTask<T> _leafChild, int _timesLimit)
	{
		currentTimeIterator = 0;
		leafChild = _leafChild;
		timesLimit = _timesLimit;
		AddNode(leafChild);
	}

	/// <summary>LimitTaskNode Destructor.</summary>
	~LimitTaskNode()
	{
		//...
	}

	/// <summary>Ticks leafChild while it hasn't be ticked the limitTime amount.</summary>
	/// <returns>State of the leafChild if allowed, Failure if not allowed to Tick anymore.</summary>
	public override States Tick()
	{
		if(currentTimeIterator >= timesLimit)
		{
			return state = leafChild.Tick();
			currentTimeIterator++;
		}
		else return state = States.Failure;
	}
}
