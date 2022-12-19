using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game>
{
	[Header("Game Time:")]
	[SerializeField] [Range(0, 10)] private int _minutes; 			/// <summary>Minutes the Game Session will last.</summary> 
	[SerializeField] [Range(0, 59)] private int _seconds; 			/// <summary>Seconds [Additional to the Minutes] the Game Session will last.</summary> 
	[Space(5f)]
	[SerializeField] private int _localTeamScore;					/// <summary>Score of the Local Team on current Game session.</summary>
	[SerializeField] private int _visitorTeamScore;					/// <summary>Score of the Visitor Team on current Game session.</summary>
	[Space(5f)]
	[SerializeField] private Vector3 _localMovementDirection; 		/// <summary>Local Team's Movement Direction Reference.</summary>
	[SerializeField] private Vector3 _visitorMovementDirection; 	/// <summary>Visitor Team's Movement Direction Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets minutes property.</summary>
	public int minutes
	{
		get { return _minutes; }
		set { _minutes = value; }
	}

	/// <summary>Gets and Sets seconds property.</summary>
	public int seconds
	{
		get { return _seconds; }
		set { _seconds = value; }
	}

	/// <summary>Gets and Sets localTeamScore property.</summary>
	public int localTeamScore
	{
		get { return _localTeamScore; }
		set { _localTeamScore = value; }
	}

	/// <summary>Gets and Sets visitorTeamScore property.</summary>
	public int visitorTeamScore
	{
		get { return _visitorTeamScore; }
		set { _visitorTeamScore = value; }
	}

	/// <summary>Gets and Sets localMovementDirection property.</summary>
	public Vector3 localMovementDirection
	{
		get { return _localMovementDirection; }
		set { _localMovementDirection = value; }
	}

	/// <summary>Gets and Sets visitorMovementDirection property.</summary>
	public Vector3 visitorMovementDirection
	{
		get { return _visitorMovementDirection; }
		set { _visitorMovementDirection = value; }
	}
#endregion

	/// <summary>Instance proceedures.</summary>
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
	}
}
