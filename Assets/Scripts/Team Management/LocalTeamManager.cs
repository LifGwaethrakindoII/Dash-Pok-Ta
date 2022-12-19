using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace DashPokTa
{
public class LocalTeamManager : TeamManager
{
	private static LocalTeamManager _instance; ///<summary>Class Instance.</summary>

	///<summary>Gests Instance property.</summary>
	public static LocalTeamManager Instance 
	{
		get{ return _instance; }
	}

	void Awake()
	{
		if(_instance == null)
		{
			Loaded = false;
			_instance = this;
			ball = Ball.Instance.gameObject;
			TeamInitializationProceedures();
			MeasurePlayers();
			SwitchPlayer();
			selectedPlayer = nearestPlayer;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	/// <summary>Assigns Goal relative to the Team [Local or Visitor].</summary>
	public override void AssignGoal()
	{
		goal = VisitorGoal.Instance.transform;
	}

	/// <summary>Changes Paradigm depending what Team last collided with the Ball.</summary>
	/// <param name="_team">Team that last hitted the Ball.</summary>.
	public override void CheckParadigmShift(TeamManager _team)
	{
		if(_team is LocalTeamManager)
		{
			paradigm = Paradigms.Offensive;
		}
		else if(_team is VisitorTeamManager)
		{
			paradigm = Paradigms.Defensive;
		}
	}
}
}