using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestSort))]
public class TestSortInspector : Editor
{
	private TestSort testSort;

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		testSort = target as TestSort;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if(GUILayout.Button("Test Sort:"))
		{
			testSort.Sort();
		}
	}
}