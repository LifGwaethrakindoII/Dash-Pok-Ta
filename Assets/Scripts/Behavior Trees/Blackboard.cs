using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
	private static Blackboard _instance;								/// <summary>Blackboard's Instance.</summary>
	private Dictionary<string, Dictionary<string, TreeNode>> _subTrees; /// <summary>Registered Sub-Trees for direct access.</summary>

	/// <summary>Gets Instance property.</summary>
	public static Blackboard Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new Blackboard();
			}

			return _instance;
		}
	}

	/// <summary>Gets and Sets SubTrees property.</summary>
	public Dictionary<string, Dictionary<string, TreeNode>> SubTrees 
	{
		get { return _subTrees; }
		set { _subTrees = value; }
	}

	/// <summary>Blackboard Constructor.</summary>
	public Blackboard()
	{
		//...
	}

	/// <summary>Gets Dictionary TreeNode with the given ID.</summary>
	/// <param name="_treeNodeID">ID of the registered TreeNode.</param>
	/// <param name="_treeDisctionaryID">ID of the registered TreeNode Dictionary.</param>
	/// <returns>TreeNode with ID given.</returns>
	public TreeNode getTreeNode(string _treeDisctionaryID, string _treeNodeID)
	{
		return _subTrees[_treeDisctionaryID][_treeNodeID];
	}
}
