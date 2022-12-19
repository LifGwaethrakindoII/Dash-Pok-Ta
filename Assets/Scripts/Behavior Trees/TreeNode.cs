using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public abstract class TreeNode
{
	/// <summary>Return States of the nodes.<summary>
	public enum States
	{
		Unassigned, 					 						/// <summary>Unassigned State.</summary>
		Success, 						 						/// <summary>Success State.</summary>
		Failure, 						 						/// <summary>Failure State.</summary>
		Running, 						 						/// <summary>Running State.</summary>
		Error							 						/// <summary>Error State.</summary>
	}

	private States _state;				 						/// <summary>Current State of the node.</summary>
	private TreeNode _parentNode; 		 						/// <summary>Parent of the Tree Node.</summary>
	private List<TreeNode> _childNodes = new List<TreeNode>();	/// <summary>Child Nodes on this node</summary>
	private string _parentTag;						 			/// <summary>TreeNode's Parent tag. For Blackboard dictionary.</summary>
	private string _tag;						 				/// <summary>TreeNode's tag. For tests Blackboard dictionary.</summary>
	private Blackboard _blackboard; 							/// <summary>Blackboard's reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets state property.</summary>
	public States state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets parentNode property.</summary>
	public TreeNode parentNode 
	{
		get { return _parentNode; }
		set { _parentNode = value; }
	}

	/// <summary>Gets and Sets childNodes property.</summary>
	public List<TreeNode> childNodes 
	{
		get { return _childNodes; }
		set { _childNodes = value; }
	}

	/// <summary>Gets and Sets tag property.</summary>
	public string tag
	{
		get { return _tag; }
		set { _tag = value; }
	}

	/// <summary>Gets and Sets parentTag property.</summary>
	public string parentTag
	{
		get { return _parentTag; }
		set { _parentTag = value; }
	}

	/// <summary>Gets and Sets blackboard property.</summary>
	public Blackboard blackboard
	{
		get
		{
			if(_blackboard == null)
			{//Try to get Blackboard Instance.
				try { _blackboard = Blackboard.Instance; }
				catch { Debug.LogWarning("Blackboard find returned failure. Trying again..."); }
			}
			
			return _blackboard;
		}
	}
#endregion

	///<summary>Default TreeNode Class constructor.</summary>
	public TreeNode()
	{
		//...
	}

	///<summary>Override TreeNode Class constructor.</summary>
	/// <param name="_treeNodes">TreeNode n collection that will be added to the children Node list.</param>
	public TreeNode(params TreeNode[] _treeNodes)
	{
		AddNodes(_treeNodes);
	}

	///<summary>TreeNode Class destructor.</summary>
	~TreeNode()
	{
		//...
	}

	///<summary>Adds a new TreeNode to the child collection, and presents itself as their Parent Node.</summary>
	/// <param name="_treeNode">TreeNode that will be added to the children Node list.</param>
	public void AddNode(TreeNode _treeNode)
	{
		_treeNode.parentTag = _parentTag;
		_treeNode.parentNode = this;
		_childNodes.Add(_treeNode);
	}

	///<summary>Adds a new TreeNode to the child collection, and presents itself as their Parent Node.</summary>
	/// <param name="_treeNodes">TreeNodes collection that will be added to the children Node list.</param>
	public void AddNodes(params TreeNode[] _treeNodes)
	{
		foreach(TreeNode treeNode in _treeNodes)
		{
			AddNode(treeNode);
		}
	}

	/// <summary>Waits for node to get an assigned State.</summary>
	/// <param name="onStateChanged">Anonymus function called when state changed from unassigned.</summary>
	public IEnumerator WaitNode(Func<States> onStateChanged)
	{
		while(state == States.Unassigned)
		{
			yield return null;
		}

		if(onStateChanged != null) onStateChanged();
	}

	/// <summary>Ticks to the next Child Node (The priority must be from left to right).</summary>
	public abstract States Tick();
	/*{
		foreach(TreeNode childNode in _childNodes)
		{
			if(childNode.state == States.Unassigned || childNode.state == States.Running)
			{
				States childNodeState = childNode.Tick();
				if(childNodeState == States.Running) return state = childNodeState;
			}		
		}

		//If reached the end of iteration
		ResetState();

		return state = States.Success;
	}*/

	/// <summary>Resets the state to its default State (Unassigned) then calls all Child Nodes to reset their States.</summary>
	public void ResetState()
	{
		state = States.Unassigned;

		foreach(TreeNode childNode in _childNodes)
		{
			childNode.ResetState();
		}
	}
}
