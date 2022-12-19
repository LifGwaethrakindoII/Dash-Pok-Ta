using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidlessUtilities;

public abstract class Waypoint : MonoBehaviour
{
	private const float ARROW_LENGTH = 2.15f; 					/// <summary>Arrow's Length.</summary>
	private const float SIDELINE_LENGTH = 1.35f; 				/// <summary>Arrow's Sideline's Length.</summary>
	private const float SIDELINE_ANGLE = 150f; 					/// <summary>Arrow's Sideline Aperture's Angle [in Degrees].</summary>

	[SerializeField] private DrawTypes _drawType; 				/// <summary>Type of Drawn of the Waypoint.</summary>
	[SerializeField] private Vector3 _waypointBaseDimension; 	/// <summary>Waypoint Space Dimensions.</summary>
	[SerializeField] private Color _waypointColor; 				/// <summary>Color of the Waypoint.</summary>
	private Collider _collider; 								/// <summary>Collider class of the Waypoint.</summary>

	public enum DrawTypes 										/// <summary>Types of Draw of the Waypoint.</summary>
	{
		Wired, 													/// <summary>Wired DrawType.</summary>
		Solid													/// <summary>Solid DrawType</summary>
	}

#region Getters/Setters:
	/// <summary>Gets and Sets drawType property.</summary>
	public DrawTypes drawType
	{
		get { return _drawType; }
		set { _drawType = value; }
	}

	/// <summary>Gets and Sets waypointBaseDimension property.</summary>
	public Vector3 waypointBaseDimension
	{
		get { return _waypointBaseDimension; }
		set { _waypointBaseDimension = value; }
	}

	/// <summary>Gets and Sets waypointColor property.</summary>
	public Color waypointColor
	{
		get { return _waypointColor; }
		set { _waypointColor = value; }
	}

	/// <summary>Gets and Sets collider property.</summary>
	public virtual Collider collider
	{
		get
		{
			if(gameObject.Has<Collider>())
			{
				_collider = GetComponent<Collider>();
			}

			return _collider;
		}

		set { _collider = value; }
	}
#endregion

	protected void DrawNormalArrow()
	{
		Vector3 arrowTipPosition = ( (transform.forward * (waypointBaseDimension.z * ARROW_LENGTH)) );
		Vector3 arrowSidePosition = new Vector3(
									(arrowTipPosition.x + (Mathf.Cos(SIDELINE_ANGLE) * SIDELINE_LENGTH)),
									arrowTipPosition.y,
									(arrowTipPosition.z + (Mathf.Sin(SIDELINE_ANGLE) * SIDELINE_LENGTH)));

		Gizmos.color = waypointColor;
		Gizmos.DrawLine((transform.forward * waypointBaseDimension.z), arrowTipPosition);
		Gizmos.DrawLine( arrowTipPosition,  arrowSidePosition);
		Gizmos.DrawLine( arrowTipPosition,  arrowSidePosition.InvertX());
	}

	/*void OnValidate()
	{

	}*/
}
