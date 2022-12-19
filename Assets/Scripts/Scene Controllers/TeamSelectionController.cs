using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashPokTa
{
public class TeamSelectionController : SceneController
{
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
	}

	/// <summary>.</summary>
	/// <param name="_teamMember">.</param>
	public static void AddPlayer(GameObject _teamMember)
	{
		LocalTeamManager.Instance.teamMembers.Add(_teamMember);
	}

	protected override void OnObjectsLoaded(){}
}
}