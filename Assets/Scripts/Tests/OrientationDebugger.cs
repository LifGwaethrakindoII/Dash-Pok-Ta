using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationDebugger : MonoBehaviour
{
	void Start()
	{
		Debug.Log("[" + gameObject.name + "]" + " Forward: " + transform.forward);
		Debug.Log("[" + gameObject.name + "]" + " Right: " + transform.right);
		Debug.Log("[" + gameObject.name + "]" + " Up: " + transform.up);
	}	
}