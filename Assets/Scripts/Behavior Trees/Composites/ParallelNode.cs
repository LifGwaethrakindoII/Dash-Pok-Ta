using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : CompositeNode
{
	/// <summary>ParallelNode Constructor.</summary>
	public ParallelNode()
	{
		//...
	}

	/// <summary>Overload ParallelNode Constructor.</summary>
	/// <paranm name="_treeNodes">Collection of TreNodes that will be added as childs of the ParallelNode.</summary>
	public ParallelNode(params TreeNode[] _treeNodes)
	{
		AddNodes(_treeNodes);
	}

	/// <summary>ParallelNode Destructor.</summary>
	~ParallelNode()
	{
		//...
	}

	/*public override States Tick()
	{
		//...
	}*/
}
