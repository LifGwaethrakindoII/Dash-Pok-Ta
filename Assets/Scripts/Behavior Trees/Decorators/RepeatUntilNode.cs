using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RepeatUntilNode<T> : DecoratorNode<T>
{
	private States repeatCondition; /// <summary>Repetition Condition.</summary>

	/// <summary>RepeatUntilNode Constructor.</summary>
	public RepeatUntilNode()
	{
		//...
	}

	/// <summary>Overload RepeatUntilNode Constructor.</summary>
	/// <paranm name="_treeNodes">Collection of TreNodes that will be added as childs of the RepeatUntilNode.</summary>
	/// <param name="_repeatCondition">RepeatUntilNode's Repeat Condition.</summary>
	public RepeatUntilNode(LeafTask<T> _leafChild, States _repeatCondition)
	{
		repeatCondition = _repeatCondition;
		leafChild = _leafChild;
		AddNode(_leafChild);
	}

	/// <summary>RepeatUntilNode Destructor.</summary>
	~RepeatUntilNode()
	{
		//...
	}

	/// <summary>Repeats childNode's Tick() until repeatCondition is false.</summary>
	/// <returns>State of the repeating proceedures.</summary>
	public override States Tick()
	{
		States leafChildState;

		do
		{
			leafChildState = leafChild.Tick();
		}
		while(leafChildState != repeatCondition);

		return /*parentNode.state = */state = leafChildState;
	}
}