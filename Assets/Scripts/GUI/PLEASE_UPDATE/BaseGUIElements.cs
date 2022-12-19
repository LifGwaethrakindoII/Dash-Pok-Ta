using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGUIElements : MonoBehaviour
{
	[SerializeField] private float _offsetFromAnchor; /// <summary>Offset relative to anchorReference.</summary>
	private Transform _anchorReference; /// <summary>Transform reference for the GUI.</summary>

	/// <summary>Gets and Sets offsetFromAnchor property.</summary>
	public float offsetFromAnchor
	{
		get { return _offsetFromAnchor; }
		set { _offsetFromAnchor = value; }
	}

	/// <summary>Gets and Sets anchorReference property.</summary>
	public Transform anchorReference
	{
		get { return _anchorReference; }
		set { _anchorReference = value; }
	}

	/// <summary>Updates the GUI position and rotation relative to its reference object.</summary>
	public abstract void updateGUI();

	/// <summary>Positions the GUI relative to the anchorReference.</summary>
	public abstract void setPositionRelativeToAnchorReference();
}
