using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DashPokTaGameData;

public abstract class BallModel : Singleton<BallModel>, IFiniteStateMachine<BallModel.States>, IPlaygroundZoneCheck
{
	[SerializeField] private float _sphereColliderRadius; 	/// <summary>SphereCast radius.</summary>
	[SerializeField] private float _raycastLength; 			/// <summary>SphereCast length.</summary>
	[SerializeField] private float _ballForce; 				/// <summary>GameObject force applied when colliding.</summary>
	[SerializeField] private Vector3 _directionalForce; 	/// <summary>Directional Force normal modifiers.</summary>
	private Rigidbody _rigidBody; 							/// <summary>Rigidbody of the GameObject.</summary>
	private Playground.PlaygroundZones _currentZone; 		/// <summary>Current Playground Zone the character is on.</summary>
	private States _state; 									/// <summary>BallModel's current State.</summary>

	public enum States 										/// <summary>BallModel's States.</summary>
	{
		Unassigned, 										/// <summary>Unassigned State.</summary>
		OnGame, 											/// <summary>On Game State.</summary>
		OffGame, 											/// <summary>Off Game State.</summary>
		Grabbed 											/// <summary>Grabbed State.</summary>
	}

	/// <summary>Normal identities where the SphereCasts will be casted.</summary>
	private Vector3[] _identities = new Vector3[]
	{
		new Vector3(1f, 0f, 0f),
		new Vector3(-1f, 0f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(0f, -1f, 0f),
		new Vector3(0f, 0f, 1f),
		new Vector3(0f, 0f, -1f),
		new Vector3(1f, 1f, 0f),
		new Vector3(-1f, 1f, 0f),
		new Vector3(1f, -1f, 0f),
		new Vector3(-1f, -1f, 0f),
		new Vector3(1f, 0f, 1f),
		new Vector3(-1f, 0f, 1f),
		new Vector3(1f, 0f, -1f),
		new Vector3(-1f, 0f, -1f),
		new Vector3(0f, 1f, 1f),
		new Vector3(0f, -1f, 1f),
		new Vector3(0f, 1f, -1f),
		new Vector3(0f, -1f, -1f),
		new Vector3(1f, 1f, 1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(-1f, -1f, -1f)
	};

#region Getters/Setters:

	/// <summary>Gets and Sets identities property.</summary>
	public Vector3[] identities
	{
		get { return _identities; }
		//set { _identities = value; }
	}

	/// <summary>Gets and Sets sphereColliderRadius property.</summary>
	public float sphereColliderRadius
	{
		get { return _sphereColliderRadius; }
		set { _sphereColliderRadius = value; }
	}

	/// <summary>Gets and Sets raycastLength property.</summary>
	public float raycastLength
	{
		get { return _raycastLength; }
		set { _raycastLength = value; }
	}

	/// <summary>Gets and Sets ballForce property.</summary>
	public float ballForce
	{
		get { return _ballForce; }
		set { _ballForce = value; }
	}

	/// <summary>Gets rigidbody property.</summary>
	public Rigidbody rigidBody
	{
		get
		{
			if(_rigidBody == null)
			{
				_rigidBody = GetComponent<Rigidbody>();
			}

			return _rigidBody;
		}
	}

	/// <summary>Gets and Sets directionalForce property.</summary>
	public Vector3 directionalForce
	{
		get { return _directionalForce; }
		set { _directionalForce = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public States state
	{
		get { return _state; }
		set
		{
			ExitState(_state);
			EnterState(_state = value);
		}
	}

	/// <summary>Gets and Sets currentZone property.</summary>
	public Playground.PlaygroundZones currentZone
	{
		get
		{
			Ray zoneRay = new Ray(transform.position, Vector3.down * Mathf.Infinity);
			RaycastHit[] hits = Physics.RaycastAll(zoneRay);

			if(Physics.Raycast(zoneRay.origin, (zoneRay.direction * Mathf.Infinity), Mathf.Infinity))
			{
				foreach(RaycastHit hit in hits)
				{
					if(hit.transform.gameObject.tag == "PlaygroundZone")
					{
						return _currentZone = hit.transform.gameObject.GetComponent<PlaygroundZone>().playgroundZone;
					}
				}
			}

			return _currentZone = Playground.PlaygroundZones.Unassigned;
		}
		set { _currentZone = value; }
	}
#endregion

#region FiniteStateMachine:
	/// <summary>Enters BallModel.States State.</summary>
	/// <param name="_state">BallModel.States State that will be entered.</param>
	public void EnterState(BallModel.States _state)
	{
		switch(_state)
		{
			case States.Unassigned:
			break;

			case States.OnGame:
			transform.parent = null;
			break;

			case States.OffGame:
			rigidBody.Sleep();
			break;

			case States.Grabbed:
			rigidBody.Sleep();
			rigidBody.useGravity = false;
			rigidBody.isKinematic = true;
			break;
	
			default:
			break;
		}

		Debug.Log("[BallModel] Entered State: " + _state.ToString());
	}
	
	/// <summary>Leaves BallModel.States State.</summary>
	/// <param name="_state">BallModel.States State that will be left.</param>
	public void ExitState(BallModel.States _state)
	{
		switch(_state)
		{
			case States.Unassigned:
			break;

			case States.OnGame:
			break;

			case States.OffGame:
			break;

			case States.Grabbed:
			rigidBody.useGravity = true;
			rigidBody.isKinematic = false;
			rigidBody.AddForce(-Vector3.up);
			break;
	
			default:
			break;
		}
	}
#endregion

#region Methods:
	/// < <summary>Pushes the ball on the normalized sumatory of the identity normals SphereCasts that hitted the ball.</summary>
	/// <param name="_obj">The GameObject where the Spherecasts will be casted.</param>
	public abstract void PushBall(GameObject _obj);
#endregion

}
