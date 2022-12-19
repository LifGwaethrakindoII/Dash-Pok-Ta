using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereWaypoint : Waypoint
{
	/// <summary>Override Getter and Setter for collider property.</summary>
	public override Collider collider
	{
		get
		{
			if(gameObject.Has<SphereCollider>())
			{
				collider = GetComponent<SphereCollider>();
			}

			return collider;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = waypointColor;
		Matrix4x4 gizmoTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Matrix4x4 priorMatrix = Gizmos.matrix;
		Gizmos.matrix *= gizmoTransform;
		DrawNormalArrow();
		if(drawType == DrawTypes.Wired) Gizmos.DrawWireSphere(Vector3.zero, waypointBaseDimension.GetMaxVectorProperty());
		else if(drawType == DrawTypes.Solid) Gizmos.DrawSphere(Vector3.zero, waypointBaseDimension.GetMaxVectorProperty());
		Gizmos.matrix = priorMatrix;
	}
}
