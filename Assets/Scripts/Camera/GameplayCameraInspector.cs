using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DashPokTa
{
[CustomEditor(typeof(GameplayCamera))]
public class GameplayCameraInspector : Editor
{
	GameplayCamera gameplayCamera;

	public override void OnInspectorGUI()
    {
        gameplayCamera = (GameplayCamera)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Test Local Origin Waypoint Travel"))
        {
            gameplayCamera.TestWaypointTravel(gameplayCamera.localOriginWaypoint);
        }
        if(GUILayout.Button("Test Local Top Waypoint Travel"))
        {
            gameplayCamera.TestWaypointTravel(gameplayCamera.localTopWaypoint);
        }
    }
}
}
