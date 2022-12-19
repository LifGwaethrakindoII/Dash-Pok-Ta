using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TeamData : ScriptableObject
{
	[SerializeField] private PlayerModel[] _players = new PlayerModel[4]; 	/// <summary>Team Players selected.</summary>

	/// <summary>Gets and Sets players property.</summary>
	public PlayerModel[] players
	{
		get { return _players; }
		set { _players = value; }
	}
}