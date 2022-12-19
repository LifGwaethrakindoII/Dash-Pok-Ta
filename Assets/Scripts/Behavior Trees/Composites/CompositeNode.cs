using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : TreeNode
{
	/// <summary>CompositeNode Constructor.</summary>
	public CompositeNode()
	{
		//...
	}

	/// <summary>CompositeNode Destructor.</summary>
	~CompositeNode()
	{
		//...
	}

	public override States Tick()
	{
		return state;
	}
}
