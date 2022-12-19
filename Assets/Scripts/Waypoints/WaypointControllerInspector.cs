using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointController))]
public class WaypointControllerInspector : Editor
{
    WaypointController waypointController;

	public override void OnInspectorGUI()
    {
    	waypointController = (WaypointController)target;

        DrawDefaultInspector();

		if(GUILayout.Button("Create Waypoints"))
        {
            waypointController.renderWaypoints();
        }
    }
}
