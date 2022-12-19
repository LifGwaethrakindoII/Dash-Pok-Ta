using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SequenceNode : CompositeNode
{
	/// <summary>SequenceNode Constructor.</summary>
	public SequenceNode()
	{
		//...
	}

	/// <summary>Overload SequenceNode Constructor.</summary>
	/// <paranm name="_treeNodes">Collection of TreNodes that will be added as childs of the SequenceNode.</summary>
	public SequenceNode(params TreeNode[] _treeNodes)
	{
		//childNodes = _treeNodes.ToList<TreeNode>();
		AddNodes(_treeNodes);
	}

	/// <summary>SequenceNode Destructor.</summary>
	~SequenceNode()
	{
		//...
	}

	/// <summary>Iterates through the treeChilds, until one returns Failure, or all childNodes return Success.</summary>
	/// <returns>State of the childNodes iteration.</summary>
	public override States Tick()
	{
		foreach(TreeNode childNode in childNodes)
		{
			States childNodeState = childNode.Tick(); //To avoid multyple executions on multyple if checkings.

			if(childNodeState == States.Failure || childNodeState == States.Error) return /*parentNode.state = */state = childNodeState;
		}

		return /*parentNode.state = */state = States.Success;
	}
}
