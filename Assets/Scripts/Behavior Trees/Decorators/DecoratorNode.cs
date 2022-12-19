using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode<T> : TreeNode
{
	private LeafTask<T> _leafChild; /// <summary>Decorator's LeafTask's child node.</summary>

	/// <summary>Gets and sets leafChild property.</summary>
	public LeafTask<T> leafChild
	{
		get { return _leafChild; }
		set { _leafChild = value; }
	}

	/// <summary>DecoratorNode Constructor.</summary>
	public DecoratorNode()
	{
		//...
	}

	/// <summary>Overload DecoratorNode Constructor.</summary>
	/// <param name="_leafChild">DecoratorNode's LeafTask child node.</param>
	public DecoratorNode(LeafTask<T> _leafChild)
	{
		this._leafChild = _leafChild;
		AddNode(_leafChild);
	}

	/// <summary>DecoratorNode Destructor.</summary>
	~DecoratorNode()
	{
		//...
	}

	public override States Tick()
	{
		return state;
	}
}
