using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DashPokTa
{
public class VisitorTeamManager : TeamManager
{
	private static VisitorTeamManager _instance; ///<summary>Class Instance.</summary>

	///<summary>Gests Instance property.</summary>
	public static VisitorTeamManager Instance 
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
		goal = LocalGoal.Instance.transform;
	}

	/// <summary>Changes Paradigm depending what Team last collided with the Ball.</summary>
	/// <param name="_team">Team that last hitted the Ball.</summary>.
	public override void CheckParadigmShift(TeamManager _team)
	{
		if(_team is LocalTeamManager)
		{
			paradigm = Paradigms.Defensive;
		}
		else if(_team is VisitorTeamManager)
		{
			paradigm = Paradigms.Offensive;
		}
	}
}
}