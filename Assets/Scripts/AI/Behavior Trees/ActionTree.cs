using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;

public class ActionTree : BasePlayerBehaviorTree
{
	[SerializeField] private float distanceFromPosition; 	/// <summary>Distance from position.</summary>
	private Vector3 movePosition; 							/// <summary>Position where the player will move.</summary>
	private Transform referenceTransform; 					/// <summary>Reference Transform.</summary>

	/// <summary>Initializes Support Defender BehaviorTree.</summary>
	/// <returns>Support Defender Tree Setted.</summary>
	public override SelectorNode InitializeSupportDefenderTree()
	{
		return
		new SelectorNode
		(
			new SequenceNode
			(
				new ConditionTask<TreeNode.States>(IsPlayerCloseToBall)
			)
		);
	}

	/// <summary>Initializes Main Defender BehaviorTree.</summary>
	/// <returns>Main Defender Tree Setted.</summary>
	public override SelectorNode InitializeMaintDefenderTree()
	{
		return new SelectorNode();
	}

	/// <summary>Initializes Support Attacker BehaviorTree.</summary>
	/// <returns>Support Attacker Tree Setted.</summary>
	public override SelectorNode InitializeSupportAttackerTree()
	{
		return new SelectorNode();
	}

	/// <summary>Initializes Main Attacker BehaviorTree.</summary>
	/// <returns>Main Attacker Tree Setted.</summary>
	public override SelectorNode InitializeMainAttackerTree()
	{
		return new SelectorNode();
	}

	/// <summary>Initializes Action BehaviorTree.</summary>
	/// <returns>Main Attacker Tree Setted.</summary>
	public override SelectorNode InitializeActionTree()
	{
		return new SelectorNode();
	}

	/// <summary>Ticks the Position Tree.</summary>
	public override IEnumerator RunTree()
	{
		yield return null;
	}

#region ConditionLeafs:
	/// <summary>Checks if Player is clote to Ball.</summary>
	private TreeNode.States IsPlayerCloseToBall()
	{
		return TreeNode.States.Success;
	}
#endregion

#region ActionLeafs:
	/// <summary>Sets Reference Transform.</summary>
	private TreeNode.States SetReferenceTransform(Transform _referenceTransform)
	{
		referenceTransform = _referenceTransform;
		return TreeNode.States.Success;
	}

	/// <summary>Sets new Move Position.</summary>
	private TreeNode.States SetMovePosition()
	{
		movePosition = referenceTransform.position.GetDirectionTowards(transform.position).normalized * distanceFromPosition;
		return TreeNode.States.Success;
	}
#endregion
}
