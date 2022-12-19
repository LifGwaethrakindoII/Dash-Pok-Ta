using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectorNode : CompositeNode
{
	/// <summary>SelectorNode Contructor.</summary>
	public SelectorNode()
	{
		//...
	}

	/// <summary>Overload SelectorNode Constructor.</summary>
	/// <paranm name="_treeNodes">Collection of TreNodes that will be added as childs of the SelectorNode.</summary>
	public SelectorNode(params TreeNode[] _treeNodes)
	{
		//childNodes = _treeNodes.ToList<TreeNode>();
		AddNodes(_treeNodes);
	}

	/// <summary>SelectorNode Destructor.</summary>
	~SelectorNode()
	{
		//...
	}

	/// <summary>Iterates through the treeChilds, until one returns Success, or all childNodes return Failure.</summary>
	/// <returns>State of the childNodes iteration.</summary>
	public override States Tick()
	{
		foreach(TreeNode childNode in childNodes)
		{
			States childNodeState = childNode.Tick();

			if(childNodeState == States.Success || childNodeState == States.Running) return /*parentNode.state = */state = States.Success;
		}

		return /*parentNode.state = */state = States.Failure;
	}
}
