using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;
using DashPokTaGameData;

namespace DashPokTa
{
public class GameplayCamera : BaseCamera, ILoadable
{
	private static GameplayCamera _instance; 	/// <summary>GameplayCamera current Instance.</summary>

	[Space(5f)]
	[Header("Camera Waypoints:")]
	[SerializeField] private WaypointController _localOriginWaypoint; 		/// <summary>Original Position Waypoint Controller reference.</summary>
	[SerializeField] private WaypointController _localTopWaypoint; 			/// <summary>Local Top-View Waypoint Controller.</summary>
	[SerializeField] private WaypointController _visitorOriginWaypoint; 	/// <summary>Visitor Origin Waypoint Controller.</summary>
	[SerializeField] private WaypointController _visitorTopWaypoint; 		/// <summary>Visitor Top-View Waypoint Controller.</summary>
	private LocalTeamManager _localTeam; 	 								/// <summary>LocalTeamManager's Instance.</summary>
	private VisitorTeamManager _visitorTeam; 								/// <summary>VisitorTeamManager's Instance.</summary>
	private Transform _ball;				 								/// <summary>Ball transform reference.</summary>
	private Vector3 middlePoint; 											/// <summary>Gameplay Camera's current Middle Point.</summary>
	private Vector3 offsetDistance;											/// <summary>3D distance vector between Camera and Middle Point.</summary>
	private Behavior pointChanger;											/// <summary>Behavior that controls the point target switching coroutines.</summary>
	private Behavior waypointTraverse;										/// <summary>Behavior that controls the wypoint traveling coroutines.</summary>
	private bool enlargeFieldOfView; 										/// <summary>Enlarge Field of View Flag. If false, it simishe it.</summary>
	private bool _loaded; 													/// <summary>ILoaded Loaded Flag.</summary>

#region Getters/Setters:
	/// <summary>Gets Instance property</summary> 
	public static GameplayCamera Instance 
	{
		get { return _instance; }
	}

	/// <summary>Gets and Sets localOriginWaypoint property.</summary>
	public WaypointController localOriginWaypoint
	{
		get { return _localOriginWaypoint; }
		set { _localOriginWaypoint = value; }
	}

	/// <summary>Gets and Sets localTopWaypoint property.</summary>
	public WaypointController localTopWaypoint
	{
		get { return _localTopWaypoint; }
		set { _localTopWaypoint = value; }
	}

	/// <summary>Gets and Sets visitorOriginWaypoint property.</summary>
	public WaypointController visitorOriginWaypoint
	{
		get { return _visitorOriginWaypoint; }
		set { _visitorOriginWaypoint = value; }
	}

	/// <summary>Gets and Sets visitorTopWaypoint property.</summary>
	public WaypointController visitorTopWaypoint
	{
		get { return _visitorTopWaypoint; }
		set { _visitorTopWaypoint = value; }
	}

	/// <summary>Gets and Sets localTeam property.</summary>
	public LocalTeamManager localTeam
	{
		get { return _localTeam; }
		set { _localTeam = value; }
	}

	/// <summary>Gets and Sets visitorTeam property.</summary>
	public VisitorTeamManager visitorTeam
	{
		get { return _visitorTeam; }
		set { _visitorTeam = value; }
	}

	/// <summary>Gets and Sets ball property.</summary>
	public Transform ball
	{
		get { return _ball; }
		set { _ball = value; }
	}

	/// <summary>Gets and Sets Loaded property.</summary>
	public bool Loaded
	{
		get { return _loaded; }
		set { _loaded = value; }
	}
#endregion

	void OnEnable()
	{
		SceneController.onObjectsOnSceneLoaded += ()=>
		{
			ball = Ball.Instance.transform;
			localTeam = LocalTeamManager.Instance;
			visitorTeam = VisitorTeamManager.Instance; 
		};
	}

	void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
			Loaded = true;
		}
		else Destroy(gameObject);
	}

	void Start()
	{
		GetMiddlePoint();
		offsetDistance = middlePoint.GetDirectionTowards(localOriginWaypoint.destinationWaypoint.transform.position).SetX(0f);
	}

	void LateUpdate()
	{
		if(LocalTeamManager.Instance != null && VisitorTeamManager.Instance != null) UpdateCamera();
	}

	/// <summary>Updates Gameplay Camera.</summary>
	private void UpdateCamera()
	{
		GetMiddlePoint();

		//If the Waypoint Traverse Corroutine is not running, follow the middle point.
		if(waypointTraverse == null) FollowMiddlePoint();

		//Interpolate Field Of View Proceedures.
		if(enlargeFieldOfView)
		{//If Flag Activated, enlarge Field of View.
			if(camera.fieldOfView < maximumFieldOfView)
			{//While the Field Of View hasn't reached the maximumFieldOfView value.
				camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, maximumFieldOfView, (Time.deltaTime * fieldOfViewLerpTime));
			}
		}
		else
		{//If not, diminish it to its default state.
			if(camera.fieldOfView > minimumFieldOfView)
			{//While the Field of View hasn't reached tje minimumFieldOfView value.
				camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, minimumFieldOfView, (Time.deltaTime * fieldOfViewLerpTime));
			}
		}
	}

	/// <summary>Gets middlePoint.</summary>
	private void GetMiddlePoint()
	{
		//Good Camera Position: (0f, 35f, 60f)

		if(localTeam.selectedPlayer != null && visitorTeam.selectedPlayer != null)
		{//If there is both selected Local and Visitor Team Player.
			middlePoint = VoidlessMath.GetMiddlePointBetween(localTeam.selectedPlayer.transform.position, visitorTeam.selectedPlayer.transform.position, ball.position);
			//If either the Local, Visitor Team Player or the Ball is invisible to the Gameplay Camera.
			enlargeFieldOfView = (!localTeam.selectedPlayer.transform.IsVisibleToCamera() || !visitorTeam.selectedPlayer.transform.IsVisibleToCamera() || !ball.IsVisibleToCamera());
		}
		else if(visitorTeam.selectedPlayer == null && localTeam.selectedPlayer != null)
		{//If there is just Local Team Player Selected.
			middlePoint = VoidlessMath.GetMiddlePointBetween(localTeam.selectedPlayer.transform.position, ball.position);
			//If either the Local Team Player or the Ball is invisible to the Gameplay Camera.
			enlargeFieldOfView = (!localTeam.selectedPlayer.transform.IsVisibleToCamera() || !ball.IsVisibleToCamera());
		}
		else if(localTeam.selectedPlayer == null && visitorTeam.selectedPlayer != null)
		{//If there is just Visitor Team Player Selected.
			middlePoint = VoidlessMath.GetMiddlePointBetween(visitorTeam.selectedPlayer.transform.position, ball.position);
			//If either the Visitor Team Player or the Ball is invisible to the Gameplay Camera.
			enlargeFieldOfView = (!visitorTeam.selectedPlayer.transform.IsVisibleToCamera() || !ball.IsVisibleToCamera());
		}
		else
		{//If both Teams have no selected Players, just follow the ball.
			middlePoint = ball.position;
			enlargeFieldOfView = false;
		}
	}

	/// <summary>Follows the middle point between the main players and the ball, considering the distance offset and heigth of the Camera.</summary>
	private void FollowMiddlePoint()
	{
		transform.position = Vector3.Lerp(transform.position, middlePoint.SetYAndZ(offsetDistance.y, (middlePoint.z + offsetDistance.z)), (cameraSpeed * Time.deltaTime));
	}

	/// <summary>[TEST ONLY] Test Travel to designated Waypoint.</summary>
	/// <param name="_waypointController">WaypointController's Waypoints to travel.</param>
	public void TestWaypointTravel(WaypointController _waypointController)
	{
		Transform waypoint = _waypointController.gameObject.transform.GetChild((_waypointController.gameObject.transform.childCount - 1));
		
		transform.position = waypoint.position;
		transform.rotation = waypoint.rotation;
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;

		switch(obj.tag)
		{
			case Keys.CAMERA_WAYPOINT_TAG:
			if(waypointTraverse != null) waypointTraverse.EndBehavior();
			waypointTraverse = new Behavior(this, TravelWaypoints(obj.GetComponent<WaypointController>()));
			break;

			default:
			//...
			Debug.Log("Collided with tag " + obj.tag);
			break;
		}
	}

	/// <summary>Translates one by one the waypoints of the WaypointController.</summary>
	/// <param name="_waypointController">The Controller of Waypoints.</param>
	IEnumerator TravelWaypoints(WaypointController _waypointController)
	{
		float normalizedTime;
		float traverseDuration = (waypointsTravelingTime / (_waypointController.waypointsQuantity * 1.0f));
		Vector3 newPosition;

		for(int i = 0; i < _waypointController.waypointsQuantity; i++)
		{
			normalizedTime = 0.0f;

			while(normalizedTime < (1.0f / _waypointController.waypointsQuantity))
			{
				yield return new WaitForEndOfFrame();

				newPosition = new Vector3(middlePoint.x, _waypointController.waypointsPositions[i].y, transform.position.z);

				offsetDistance = Vector3.Lerp(offsetDistance, (_waypointController.waypointsPositions[i] - middlePoint), normalizedTime);
				transform.rotation = Quaternion.Lerp(transform.rotation, _waypointController.waypointsRotations[i], normalizedTime);
				transform.position = Vector3.Lerp(transform.position, newPosition, normalizedTime);
				
				normalizedTime += (Time.deltaTime / traverseDuration);
				yield return null;
			}
		}

		waypointTraverse.EndBehavior();
		waypointTraverse = null;
	}
}
}