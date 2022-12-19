using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;

public class TestSort : MonoBehaviour
{
	[SerializeField] private List<int> numbers; 	/// <summary>Meh, numbers.</summary>

	public void Sort()
	{
		numbers.LonkSort(true);
		//Debug.Log("[TestSort] Sort of...duh.");
	}
}