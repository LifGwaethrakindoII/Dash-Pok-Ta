using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundZone : RectangleWaypoint
{
	[SerializeField] private Playground.PlaygroundZones _playgroundZone; 	/// <summary>Playground Zone.</summary>

	/// <summary>Gets and Sets playgroundZone property
	public Playground.PlaygroundZones playgroundZone
	{
		get {return _playgroundZone; }
		set { _playgroundZone = value; }
	}
}
