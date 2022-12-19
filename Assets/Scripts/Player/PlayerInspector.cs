using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerModel))]
public class PlayerInspector : Editor
{
	PlayerModel playerModel;

	public override void OnInspectorGUI()
    {
        playerModel = (PlayerModel)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Check if Grounded"))
        {
            Debug.Log("[PlayerInspector] Is Grounded: " + playerModel.isGrounded);
        }
    }
}
