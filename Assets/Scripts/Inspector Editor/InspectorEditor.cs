using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Playground))]
public class InspectorEditor : Editor
{
	Playground playground;

	public override void OnInspectorGUI()
    {
        playground = (Playground)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Create Zones"))
        {
            playground.CreateZones();
        }

        using (new EditorGUI.DisabledScope(true))
        {
            playground.overrideWaypointsColor = EditorGUILayout.ColorField("Color", playground.overrideWaypointsColor);
            /*playground.someString = EditorGUILayout.TextField("Text", playground.someString);
            playground.someNumber = EditorGUILayout.IntField("Number", playground.someNumber);*/
        }
    }
}
