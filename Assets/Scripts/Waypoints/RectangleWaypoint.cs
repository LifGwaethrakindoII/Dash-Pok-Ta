using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RectangleWaypoint : Waypoint
{
	private BoxCollider _boxCollider; /// <summary>BoxCollider Component.</summary>

	public BoxCollider boxCollider
	{
		get
		{
			if(gameObject.Has<BoxCollider>() && _boxCollider == null)
			{
				_boxCollider = GetComponent<BoxCollider>();
			}

			return _boxCollider;
		}
	}


	/// <summary>Override Getter and Setter for collider property.</summary>
	public override Collider collider
	{
		get
		{
			if(GetComponent<BoxCollider>() != null)
			{
				collider = GetComponent<BoxCollider>() as BoxCollider;
			}

			return collider;
		}
	}

	void Update()
	{
		if(Application.isEditor && !Application.isPlaying)
		{
			boxCollider.size = waypointBaseDimension;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = waypointColor;
		Matrix4x4 gizmoTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Matrix4x4 priorMatrix = Gizmos.matrix;
		Gizmos.matrix *= gizmoTransform;
		//DrawNormalArrow();
		if(drawType == DrawTypes.Wired) Gizmos.DrawWireCube(Vector3.zero, waypointBaseDimension);
		else if(drawType == DrawTypes.Solid) Gizmos.DrawCube(Vector3.zero, waypointBaseDimension);
		Gizmos.matrix = priorMatrix;
	}
}
