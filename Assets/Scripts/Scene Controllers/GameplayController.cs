using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidlessUtilities;

namespace DashPokTa
{
public enum GameplayStates
{
	Unassigned,
	Playing,
	ScoreMade,
	GameOver
}

public class GameplayController : SceneController, IFiniteStateMachine<GameplayStates>
{
	[Space(5f)]
	[Header("Player Markers:")]
	[SerializeField] private PlayerMarkerGUI player1MarkerGUI; 		/// <summary>Player 1's Marker GUI.</summary>
	[SerializeField] private PlayerMarkerGUI player2MarkerGUI; 		/// <summary>Player 2's Marker GUI.</summary>
	[Space(5f)]
	[SerializeField] private TimerGUI timerGUI; 					/// <summary>Timer's GUI.</summary>
	[Space(5f)]
	[SerializeField] private TeamScoreGUI teamScoreGUI; 			/// <summary>Team Score's GUI.</summary>
	[Space(5f)]
	[Header("Prefabs:")]
	[SerializeField] private PlayerVitalityGUI playerVitalityGUI; 	/// <summary>Prefab reference to instantiante a playerVitalityGUI per player.</summary>
	[Space(5f)]
	[SerializeField] private float countdownDuration; 				/// <summary>Countdown's duration before resuming Game.</summary>
	private List<PlayerVitalityGUI> localTeamPlayerVitalityBars; 	/// <summary>PlayerVitalityGUIs for each Local Team member.</summary>
	private List<PlayerVitalityGUI> visitorTeamPlayerVitalityBars; 	/// <summary>PlayerVitalityGUIs for each Visitor Team member.</summary>
	private Game game; 												/// <summary>Game's Instance reference.</summary>
	private BallModel ball; 										/// <summary>BallModel's Instance reference.</summary>
	private GameplayCamera gameplayCamera; 							/// <summary>GameplayCamera's Instance reference.</summary>
	private LocalTeamManager localTeam; 							/// <summary>LocalVisitorTeam's Intance reference.</summary>
	private VisitorTeamManager visitorTeam; 						/// <summary>VisitorTeamManager's Instance reference.</summary>
	private TimeClock timer; 										/// <summary>Game Session's Timer Clock.</summary>
	private GameplayStates _gameplayState; 							/// <summary>Current Gameplay State.</summary>
	private Behavior onHoldCountdown; 								/// <summary>OnHoldCountdown coroutine controller [executed when waiting before resuming the game].</summary>

	/// <summary>Gets and Sets gameplayState property.</summary>
	public GameplayStates gameplayState
	{
		get { return _gameplayState; }
		set
		{
			ExitState(gameplayState);
			EnterState(_gameplayState = value);
		}
	}

#region FiniteStateMachine:
	/// <summary>Enters GameplayStates State.</summary>
	/// <param name="_state">GameplayStates State that will be entered.</param>
	public void EnterState(GameplayStates _state)
	{
		switch(_state)
		{
			case GameplayStates.Unassigned:
			onHoldCountdown = new Behavior(this, OnHoldCountdown(
			()=>
			{
				gameplayState = GameplayStates.Playing;
			}));
			/*this.WaitSeconds(countdownDuration,
			()=>
			{
				gameplayState = GameplayStates.Playing;
			});*/
			break;

			case GameplayStates.Playing:
			break;

			case GameplayStates.ScoreMade:
			break;

			case GameplayStates.GameOver:
			OnGameOver();
			break;
	
			default:
			break;
		}
	}
	
	/// <summary>Leaves GameplayStates State.</summary>
	/// <param name="_state">GameplayStates State that will be left.</param>
	public void ExitState(GameplayStates _state)
	{
		switch(_state)
		{
			case GameplayStates.Unassigned:
			if(onHoldCountdown != null) onHoldCountdown.EndBehavior();
			break;

			case GameplayStates.Playing:
			break;

			case GameplayStates.ScoreMade:
			break;

			case GameplayStates.GameOver:
			break;
	
			default:
			break;
		}
	}
#endregion

	/// <summary>Define Instance references.</summary>
	void Start()
	{
		/*while(game == null && ball == null && gameplayCamera == null && localTeam == null && visitorTeam == null)
		{
			game = Game.Instance;
			ball = BallModel.Instance;
			gameplayCamera = GameplayCamera.Instance;
			localTeam = LocalTeamManager.Instance;
			visitorTeam = VisitorTeamManager.Instance;
			yield return null;
		}*/
	}

	/// <summary>OnGoalScored Event subscription.</summary>
	void OnEnable()
	{
		BaseGoal.onGoalScored += GoalScored;
		SalvageShip.onBallDropped += ()=> { gameplayState = GameplayStates.Playing; };
	}

	/// <summary>OnGoalScored Event unsubscription.</summary>
	void OnDisable()
	{
		BaseGoal.onGoalScored -= GoalScored;
		SalvageShip.onBallDropped -= ()=> { gameplayState = GameplayStates.Playing; };
	}

	void OnDestroy()
	{
		timer.onTimeEnds -= ()=> { gameplayState = GameplayStates.GameOver; };
		timer = null;
	}

	/// <summary>Tracks Player's Inputs.</summary>
	void Update()
	{
		switch(gameplayState)
		{
			case GameplayStates.Playing:
			UpdateTimer();
			UpdatePlayerMarkers();
			UpdatePlayerVitalityGUIs();
			break;
		}
	}

	/// <summary>Sums score to the Team who made the goal and updates TeamScoreGUI.</summary>
	/// <param name="_goal">The goal who recieved the ball.</param>
	public void GoalScored(BaseGoal _goal)
	{
		gameplayState = GameplayStates.ScoreMade;

		if(_goal is LocalGoal) game.localTeamScore++;
		else if(_goal is VisitorGoal) game.visitorTeamScore++;
		else Debug.Log("None of both.");

		teamScoreGUI.UpdateGUI(game);
	}

	private void OnGameOver()
	{
		//...
	}

	/// <summary>Creates one PlayerVitalityGUI Prefab per each Player in both Local and Visitor Teams.</summary>
	private void CreatePlayerVitalityGUIs()
	{
		localTeamPlayerVitalityBars = new List<PlayerVitalityGUI>();

		foreach(PlayerModel player in localTeam.players)
		{
			PlayerVitalityGUI playerBar = Instantiate(playerVitalityGUI.GetComponent<PlayerVitalityGUI>(), transform.position, transform.rotation) as PlayerVitalityGUI;
			localTeamPlayerVitalityBars.Add(playerBar);
			playerBar.Data = player as PlayerModel;
		}

		visitorTeamPlayerVitalityBars = new List<PlayerVitalityGUI>();

		foreach(PlayerModel player in visitorTeam.players)
		{
			PlayerVitalityGUI playerBar = Instantiate(playerVitalityGUI.GetComponent<PlayerVitalityGUI>(), transform.position, transform.rotation) as PlayerVitalityGUI;
			localTeamPlayerVitalityBars.Add(playerBar);
			playerBar.Data = player as PlayerModel;
		}
	}

	/// <summary>Updates Timer and TimerGUI.</summary>
	private void UpdateTimer()
	{
		if(timer != null)
		{
			timer.Tick();
			timerGUI.UpdateGUI(timer);

			if(!timer.running) gameplayState = GameplayStates.GameOver;
		}
	}

	/// <summary>Updates Selected Player Markers for each Team.</summary>
	private void UpdatePlayerMarkers()
	{
		//player1MarkerGUI.UpdateGUI(localTeam.selectedPlayer.transform);
	}

	/// <summary>Updates all the PlayerVitalityGUIs.</summary>
	private void UpdatePlayerVitalityGUIs()
	{
		if(localTeamPlayerVitalityBars != null)
		{
			foreach(PlayerVitalityGUI playerBar in localTeamPlayerVitalityBars)
			{
				playerBar.UpdateGUI(playerBar.Data);
			}
		}
		
		if(visitorTeamPlayerVitalityBars != null)
		{
			foreach(PlayerVitalityGUI playerBar in visitorTeamPlayerVitalityBars)
			{
				playerBar.UpdateGUI(playerBar.Data);
			}
		}
	}

	/// <summary>Method Invoked when all of the ILoadable objects are loaded.</summary>
	protected override void OnObjectsLoaded()
	{
		foreach(MonoBehaviour loadable in loadableObjects)
		{
			Debug.Log("TEST..." + loadable.gameObject.name + " Loaded: " + loadable.gameObject.GetComponent<ILoadable>().Loaded);
		}

		game = Game.Instance;
		ball = BallModel.Instance;
		gameplayCamera = GameplayCamera.Instance;
		localTeam = LocalTeamManager.Instance;
		visitorTeam = VisitorTeamManager.Instance;

		timer = new TimeClock(game.minutes, game.seconds);
		timer.onTimeEnds += ()=> { gameplayState = GameplayStates.GameOver; };
		timerGUI.UpdateGUI(timer);
		CreatePlayerVitalityGUIs();
		teamScoreGUI.UpdateGUI(game);

		gameplayCamera.localTeam = localTeam;
		gameplayCamera.visitorTeam = visitorTeam;
		gameplayCamera.ball = ball.transform;
		gameplayCamera.enabled = true;

		gameplayState = GameplayStates.Unassigned;
	}

	/// <summary>Waits for 'n' time, then resumes gameplay.</summary>
	/// <param name="onCountdownEnds">Action called when the countdown ends.</param>
	private IEnumerator OnHoldCountdown(Action onCountdownEnds)
	{
		yield return new WaitForSeconds(countdownDuration);
		if(onCountdownEnds != null) onCountdownEnds();
	}
}
}
