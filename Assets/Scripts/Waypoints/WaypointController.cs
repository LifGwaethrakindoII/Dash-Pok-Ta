using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class WaypointController : MonoBehaviour
{
	[SerializeField] private GameObject _waypoint; 												/// <summary>Waypoint prefab that will be instantiated.</summary>
	[SerializeField] private bool _activateWaypoints; 											/// <summary>Toggle that enables activation/deactivation of Waypoints.</summary>
	[SerializeField] private bool _waypointsGradualFading;										/// <summary>Toggle to gradually fade the first waypoint to the last one.</summary>
	[SerializeField] private bool _connectWaypoints; 											/// <summary>Toggle to connect waypoints [with a line].</summary>
	[SerializeField] private bool _overrideColors; 												/// <summary>Override Colors?.</summary>
	[SerializeField] private Color _overrideWaypointsColor;										/// <summary>Override waypoints color property.</summary>
	[SerializeField] private Vector3 _overrideWaypointsDimensions;								/// <summary>Override waypopints dimensions property.</summary>
	[SerializeField] private int _waypointsQuantity; 											/// <summary>Quantity of Waypoints that will be instantiated.</summary>
	[SerializeField] private List<Vector3> _waypointsPositions; 								/// <summary>Waypoints of the Controller.</summary>
	[SerializeField] private List<Quaternion> _waypointsRotations = new List<Quaternion>(); 	/// <summary>Stores the Waypoints rotations.</summary>

	//Buffer properties.	
	[HideInInspector] public List<Waypoint> bufferWaypoints;									/// <summary>Waypoints classes on last Frame.</summary>
	[HideInInspector] public List<Vector3> bufferWaypointsPositions;							/// <summary>Waypoints positions on last Frame.</summary>
	[HideInInspector] public List<Quaternion> bufferWaypointsRotations;							/// <summary>Waypoints rotations on last Frame.</summary>
	[HideInInspector] public int bufferWaypointsQuantity;										/// <summary>Quantity of Waypoints on last Frame.</summary>

	private List<Waypoint> _waypoints = new List<Waypoint>();									/// <summary>List of Waypoints Components from each waypoint object.</summary>
	private List<Behavior> waypointsCreator = new List<Behavior>();								/// <summary>Stack of Create Coroutines.</summary>
	private List<Behavior> waypointsKiller = new List<Behavior>();								/// <summary>Stack of Destroy Coroutines.</summary>
	private List<Behavior> priorWaypointsUpdater = new List<Behavior>(); 						/// <summary>Stack of UpdateBuffer Coroutines.</summary>
	private List<Behavior> waypointPropertiesOverrider = new List<Behavior>();					/// <summary>Stack of OverrideWaypointsProperties Coroutines.</summary>
	private Waypoint _destinationWaypoint; 														/// <summary>Controller's Destination Waypoint.</summary>
	private bool _editorCondition; 																/// <summary>Update Flag.</summary>

#region Getters/Setters:
	/// <summary> Gets and Sets activateWaypoints property.</summary>
	public bool activateWaypoints
	{
		get { return _activateWaypoints; }
		set { _activateWaypoints = value; }
	}

	/// <summary> Gets and Sets overrideWaypointsColor property.</summary>
	public Color overrideWaypointsColor
	{
		get { return _overrideWaypointsColor; }
		set { _overrideWaypointsColor = value; }
	}

	/// <summary> Gets and Sets waypointsPositions property.</summary>
	public List<Vector3> waypointsPositions
	{
		get { return _waypointsPositions; }
		set { _waypointsPositions = value; }
	}

	/// <summary> Gets and Sets waypointsRotations property.</summary>
	public List<Quaternion> waypointsRotations
	{
		get { return _waypointsRotations; }
		set { _waypointsRotations = value; }
	}

	/// <summary> Gets and Sets waypoints property.</summary>
	public List<Waypoint> waypoints
	{
		get { return _waypoints; }
		set { _waypoints = value; }
	}

	/// <summary> Gets and Sets overrideWaypointsDimensions property.</summary>
	public Vector3 overrideWaypointsDimensions
	{
		get { return _overrideWaypointsDimensions; }
		set { _overrideWaypointsDimensions = value; }
	}

	/// <summary> Gets and Sets waypointsQuantity property.</summary>
	public int waypointsQuantity
	{
		get { return _waypointsQuantity; }
		set { _waypointsQuantity = value; }
	}

	/// <summary> Gets and Sets waypoint property.</summary>
	public GameObject waypoint
	{
		get { return _waypoint; }
		set { _waypoint = value; }
	}

	/// <summary>Gets destinationWaypoint property.</summary>
	public Waypoint destinationWaypoint { get { return _destinationWaypoint = _waypoints.Count > 0 ? _waypoints[(_waypoints.Count - 1)] : null; } }

	/// <summary> Gets editorCondition property.</summary>
	private bool editorCondition 
	{
		get { return _editorCondition = (_activateWaypoints && Application.isEditor && !Application.isPlaying); }
	}
#endregion

	void Awake()
	{
		if(Application.isPlaying)
		{
			_waypoints = transform.GetComponentsFromChilds<Waypoint>();
		}
	}

	void Update()
	{
		if(!Application.isPlaying)
		{
			renderWaypoints();
		}
	}

	void OnDrawGizmos()
	{
		if(_connectWaypoints)
		{//Connect Waypoints Toggle Activated?
			Gizmos.color = _overrideWaypointsColor;

			for(int i = 0; i < _waypointsQuantity; i++)
			{
				for(int j = (i + 1); j < _waypointsQuantity; j++)
				{
					Gizmos.DrawLine(_waypointsPositions[i], _waypointsPositions[j]);
				}
			}
		}
	}

#region WaypointsRendering:
	void Start()
	{
		if(!editorCondition)
		{//Clear all Behaviors Stack and disable the script.
			clearBehaviorsStack();
		}	
	}

	/// <summary>When a value of the script changes while being on Editor, render updated Waypoints.</summary>
	public virtual void renderWaypoints()
	{
		updateWaypoints();
		overrideWaypointsProperties();

		if(_activateWaypoints) //Just if the Toggle is on, and if we are currently on Editor.
		{
			if(_waypointsQuantity != bufferWaypointsQuantity || transform.childCount < _waypointsPositions.Count)
			{
				cleanWaypointsBuffer(); //Destroy all childs.
				createNewWaypoints();	//Create new child Waypoints.
			}
			else
			{
				updateWaypoints();
				overrideWaypointsProperties(); //Colors and Dimensions.
			}		
			
			updatePriorWaypointsBuffer();
			bufferWaypointsQuantity = _waypointsQuantity;
		}
		else//If the toggle is not on, but we are on Editor.
		{
			cleanWaypointsBuffer();
			bufferWaypointsQuantity = 0; //Force a lateWaypointsQuantity reset.
		}	
	}

	/// <summary>Cleans all waypoints in transform and resets the list</summary>
	public virtual void cleanWaypointsBuffer()
	{
		waypointsKiller.Add(new Behavior(this, DestroyAtEndOfFrame(transform.GetChildsWith<Waypoint>())));	
	}

	/// <summary>Creates new Waypoints, restores data from priorWaipoints buffer.</summary>
	public virtual void createNewWaypoints()
	{
		waypointsCreator.Add(new Behavior(this, CreateAtEndOfFrame()));
	}

	/// <summary>Updates priorWaypoints buffer.</summary>
	public virtual void updatePriorWaypointsBuffer()
	{
		priorWaypointsUpdater.Add(new Behavior(this, UpdateWaypointBufferAtEndOfFrame()));
	}

	/// <summary>Updates Waypoints relative to childs in transform.</summary>
	public virtual void updateWaypoints()
	{
		_waypointsPositions = transform.GetChildsWith<Waypoint>().GetTransformListPositions();
		_waypointsRotations = transform.GetChildsWith<Waypoint>().GetTransformListRotations();
	}

	/// <summary>Ends all Behaviors Stack.</summary>
	public virtual void clearBehaviorsStack()
	{
		foreach(Behavior behavior in waypointsCreator)
		{
			behavior.EndBehavior();
		}
		waypointsCreator = new List<Behavior>();
		foreach(Behavior behavior in waypointsKiller)
		{
			behavior.EndBehavior();
		}
		waypointsKiller = new List<Behavior>();
		foreach(Behavior behavior in priorWaypointsUpdater)
		{
			behavior.EndBehavior();
		}
		priorWaypointsUpdater = new List<Behavior>();
		foreach(Behavior behavior in waypointPropertiesOverrider)
		{
			behavior.EndBehavior();
		}
		waypointPropertiesOverrider = new List<Behavior>();

		//enabled = false; //Avoid unnecessary Update calls on Unity's economy.
	}

	/// <summary>Updates Waypoints properties by the override values of the Controller.</summary>
	public virtual void overrideWaypointsProperties()
	{
		//waypointPropertiesOverrider.Add(new Behavior(this, OverridePropertiesAtEndOfFrame()));

		_waypoints = transform.GetComponentsFromChilds<Waypoint>();
		float normalizedValue = 0.0f;

		for(int i = 0; i < _waypoints.Count; i++)
		{
			normalizedValue = (1.0f * (i + 1)) / (1.0f * _waypointsQuantity);
			_waypoints[i].waypointBaseDimension = _overrideWaypointsDimensions;
			if(_overrideColors) _waypoints[i].waypointColor = _waypointsGradualFading ? new Color(_overrideWaypointsColor.r, _overrideWaypointsColor.g, _overrideWaypointsColor.b, normalizedValue) : _overrideWaypointsColor;
		}
	}

#region Behaviors/Coroutines:
	/// <summary>Waits till the end of frame to create new Waypoints.</summary>
	IEnumerator CreateAtEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		for(int i = 0; i < _waypointsQuantity; i++)
		{
			Vector3 newPos = i < bufferWaypointsPositions.Count ? bufferWaypointsPositions[i] : transform.position; //If there was a prior position stored at the current index, it is passed to the new waypoint.
			Quaternion newRot = i < bufferWaypointsRotations.Count ? bufferWaypointsRotations[i] : transform.rotation;
			
			GameObject newWaypoint = Instantiate(_waypoint, newPos, newRot) as GameObject;
			newWaypoint.transform.SetParent(transform);
			_waypointsPositions.Add(newWaypoint.transform.position);
			_waypointsRotations.Add(newWaypoint.transform.rotation);
		}

		//_waypoints = /*bufferWaypoints.ListFull<Waypoint>() ? bufferWaypoints : */transform.GetComponentsFromChilds<Waypoint>(); //Create new Waypoint List.
		overrideWaypointsProperties();

		StopCoroutine(CreateAtEndOfFrame());
	}

	/// <summary>Waits till the end of frame to clean all Waypoint childs in transform.</summary>
	/// <param name="_list">The list of Waypoints that will be destroyed.</param>
	IEnumerator DestroyAtEndOfFrame(List<Transform> _list)
	{
		yield return new WaitForEndOfFrame();
		_list.DestroyAllObjectsFromTransforms();
		_waypointsPositions.Clear();
		_waypointsPositions = new List<Vector3>();

		StopCoroutine(DestroyAtEndOfFrame(_list));
	}

	/// <summary>Waits till the end of frame to update priorWaypoints buffer.</summary>
	IEnumerator UpdateWaypointBufferAtEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		if(_waypointsPositions.ListFull<Vector3>() && _waypointsPositions != null)
		{
			bufferWaypointsPositions.Clear();
			bufferWaypointsPositions = new List<Vector3>();
			bufferWaypointsPositions = _waypointsPositions; //Store as the former waypoints.

			bufferWaypointsRotations.Clear();
			bufferWaypointsRotations = new List<Quaternion>();
			bufferWaypointsRotations = _waypointsRotations; //Store as the former rotations.

			bufferWaypoints.Clear();
			bufferWaypoints = new List<Waypoint>();
			bufferWaypoints = _waypoints;
		}

		StopCoroutine(UpdateWaypointBufferAtEndOfFrame());
	}

	/// <summary>Waits till the end of frame to override Waypoints properties.</summary>
	IEnumerator OverridePropertiesAtEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		_waypoints = transform.GetComponentsFromChilds<Waypoint>();
		float normalizedValue = 0.0f;

		for(int i = 0; i < _waypoints.Count; i++)
		{
			normalizedValue = (1.0f * (i + 1)) / (1.0f * _waypointsQuantity);
			_waypoints[i].waypointBaseDimension = _overrideWaypointsDimensions;
			_waypoints[i].waypointColor = _waypointsGradualFading ? new Color(_overrideWaypointsColor.r, _overrideWaypointsColor.g, _overrideWaypointsColor.b, normalizedValue) : _overrideWaypointsColor;
		}

		StopCoroutine(OverridePropertiesAtEndOfFrame());
	}
#endregion
#endregion

}