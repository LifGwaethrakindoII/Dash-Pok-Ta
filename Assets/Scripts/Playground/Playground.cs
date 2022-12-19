using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : WaypointController
{
	private static Playground _instance; 										/// <summary>Instance of the Playground.</summary>

	[SerializeField] private List<ZonesDictionary> zonesDictionary; 			/// <summary>Zones Dictionary shown on Editor.</summary>
	[SerializeField] private GridDimensions gridDimensions; 					/// <summary>Grid Dimensions by cell count.</summary>
	[SerializeField] private float overrideWaypointsHeight; 					/// <summary>Override Y property of the Size of Waypoints.</summary>

	//Cache Properties:
	[HideInInspector] private List<ZonesDictionary> cacheZonesDictionary; 		/// <summary>Zones Dictionary shown on Editor.</summary>
	[HideInInspector] public GridDimensions cacheGridDimensions; 				/// <summary>Cache's Grid Dimensions that stores the last GridDimensions state.</summary>
	[HideInInspector] public float cacheMeshSizeX; 								/// <summary>Cache's Mesh Size on X.</summary>
	[HideInInspector] public float cacheMeshSizeY; 								/// <summary>Cache's Mesh Size on Y.</summary>

	private MeshRenderer _meshRenderer; 										/// <summary>Meash Reneder Component.</summary>
	private Dictionary<PlaygroundZones, Waypoint> _zones; 						/// <summary>Zones Dictionary.</summary>
	private Behavior waypointPositionsSetter; 									/// <summary>Stack of SetZonesNewPositions Coroutines.</summary> 
	private float cellWidth; 													/// <summary>Width of the Cells, relative to the grid size on X and the Renderer Bounds X dimension.</summary>
	private float cellHeight; 													/// <summary>Height of the Cells, relative to the grid size on Y and the Renderer Bounds Y dimension.</summary>

	[System.Serializable]
	public class ZonesDictionary 												/// <summary>Inspector Zone's Dictionary.</summary>
	{
		public PlaygroundZones zoneID;											/// <summary>Zone's ID.</summary>
		public Waypoint waypoint;												/// <summary>Zone's Waypoint.</summary>
		public Waypoint.DrawTypes drawType; 									/// <summary>Zone's Draw Type.</summary>
		public Color zoneColor; 												/// <summary>Zone's Color.</summary>
	}

	[System.Serializable]
	public class GridDimensions
	{
		[SerializeField] public int gridX; 										/// <summary>Size in X of the grid.</summary>
		[SerializeField] public int gridY; 										/// <summary>Size in Y of the grid.</summary>
	}

	public enum PlaygroundZones 												/// <summary>Playground Zones.</summary>
	{
		Unassigned, 															/// <summary>Unassigned Playground zone.</summary>
		LocalGoal, 																/// <summary>Local Goal Playground zone.</summary>
		LocalMiddle, 															/// <summary>Local Middle Playground zone.</summary>
		LocalSupport, 															/// <summary>Local Support Playground zone.</summary>
		LocalWing, 																/// <summary>Local Wing Playground zone.</summary>
		Center, 																/// <summary>Center Playground zone.</summary>
		VisitorGoal, 															/// <summary>Visitor Goal Playground zone.</summary>
		VisitorMiddle, 															/// <summary>Visitor Middle Playground zone.</summary>
		VisitorSupport, 														/// <summary>Visitor Support Playground zone.</summary>
		VisitorWing 															/// <summary>Visitor Wing Playground zone.</summary>
	}

#region Getters/Setters:
	/// <summary>Gets Instance property.</summary>
	public static Playground Instance
	{
		get{ return _instance; }
	}

	/// <summary>Gets zones property.</summary>
	public Dictionary<PlaygroundZones, Waypoint> zones
	{
		get { return _zones; }
		set { _zones = value; }
	}

	/// <summary>Gets meshRenderer property.</summary>
	public MeshRenderer meshRenderer
	{
		get
		{
			if(_meshRenderer == null)
			{
				if(GetComponent<MeshRenderer>() != null)
				{
					_meshRenderer = GetComponent<MeshRenderer>();
				}
			}

			return _meshRenderer;
		}
	}
#endregion

	void Awake()
	{
		if(_instance == null) _instance = this;
		else Destroy(gameObject);

		InitializeDictionary();
	}

	/// <summary>Initializes PlaygroundZones Dictionary.</summary>
	private void InitializeDictionary()
	{
		_zones = new Dictionary<PlaygroundZones, Waypoint>();

		for(int i = 0; i < zonesDictionary.Count; i++)
		{
			_zones.Add(zonesDictionary[i].zoneID, zonesDictionary[i].waypoint.gameObject.GetComponent<PlaygroundZone>());	
		}

		Debug.LogWarning("Dictionary Size: " + zones.Count);
	}

    public void CreateZones()
    {
    	if(gridDimensions.gridX > 0 && gridDimensions.gridY > 0)
		{
			RecalculateCellDimensions();
			waypointsQuantity = (gridDimensions.gridX * gridDimensions.gridY);
			if(waypointPositionsSetter != null && waypointPositionsSetter.lastCoroutine != null)
			{
				waypointPositionsSetter.EndBehavior();
			}
			waypointPositionsSetter = new Behavior(this, SetNewZonesPositions());
		}
    }

	void RecalculateCellDimensions()
	{
		//Recalculate the Grid Cells dimensions.
		cellWidth = meshRenderer.bounds.size.x / gridDimensions.gridX;
		cellHeight = meshRenderer.bounds.size.z / gridDimensions.gridY;

		overrideWaypointsDimensions = new Vector3(cellWidth, overrideWaypointsHeight, cellHeight);
	}

	/// <summary>Calculates new Zones positions relative to the Grid Size and the Mesh Renderer Dimensions of the Floor.</summary>
	IEnumerator SetNewZonesPositions()
	{
		yield return new WaitForEndOfFrame();

		List<Vector3> newPositions = new List<Vector3>();

		//Set initial position from the left upper corner of the Floor Mesh.
		Vector3 initialPosition = (transform.position - meshRenderer.bounds.extents);

		for(int i = 0; i < gridDimensions.gridX; i++)
		{
			float newX = ((initialPosition.x + (cellWidth * 0.5f)) + ((cellWidth) * (i)));

			for(int j = 0; j < gridDimensions.gridY; j++)
			{
				float newY = ((initialPosition.z + (cellHeight * 0.5f)) + ((cellHeight) * (j)));
				newPositions.Add(new Vector3(newX, transform.position.y, newY));
			}
		}

		for(int i = 0; i < newPositions.Count; i++)
		{
			Transform waypoint = waypoints[i].transform;
			waypoint.position = newPositions[i];
			zonesDictionary[i].waypoint = waypoints[i];
			zonesDictionary[i].waypoint.waypointColor = zonesDictionary[i].zoneColor;
			zonesDictionary[i].waypoint.drawType = zonesDictionary[i].drawType;
			zonesDictionary[i].waypoint.gameObject.tag = "PlaygroundZone";
			zonesDictionary[i].waypoint.gameObject.GetComponent<PlaygroundZone>().playgroundZone = zonesDictionary[i].zoneID;
		}

		waypointPositionsSetter.EndBehavior();
	}

	/// <summary>Updates Waypoints properties by the override values of the Controller.</summary>
	public virtual void overrideWaypointsProperties()
	{
		//waypointPropertiesOverrider.Add(new Behavior(this, OverridePropertiesAtEndOfFrame()));
		List<Vector3> newPositions = new List<Vector3>();

		//Set initial position from the left upper corner of the Floor Mesh.
		Vector3 initialPosition = (transform.position - meshRenderer.bounds.extents);
		waypoints = transform.GetComponentsFromChilds<Waypoint>();
		float normalizedValue = 0.0f;

		for(int i = 0; i < zonesDictionary.Count; i++)
		{
			normalizedValue = (1.0f * (i + 1)) / (1.0f * waypointsQuantity);

			Transform waypoint = waypoints[i].transform;
			waypoint.position = newPositions[i];
			zonesDictionary[i].waypoint = waypoints[i];
			zonesDictionary[i].waypoint.waypointColor = zonesDictionary[i].zoneColor;
			zonesDictionary[i].waypoint.drawType = zonesDictionary[i].drawType;
			zonesDictionary[i].waypoint.gameObject.tag = "PlaygroundZone";
			zonesDictionary[i].waypoint.gameObject.GetComponent<PlaygroundZone>().playgroundZone = zonesDictionary[i].zoneID;
		}
	}
}
