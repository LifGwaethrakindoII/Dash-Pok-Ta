using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaygroundZoneCheck
{
	Playground.PlaygroundZones currentZone{ get; set; } 	/// <summary>Stores the current zone the character is.</summary>

	/*/// <summary>Returns Zone the character is on.</summary>
	/// <returns>Playground Zone the character is on.</returns>
	Playground.PlaygroundZones GetZoneCharacterIs();*/
}
