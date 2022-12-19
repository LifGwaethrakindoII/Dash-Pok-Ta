using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VoidlessUtilities;

namespace DashPokTa
{
public enum PlayerRoles 										/// <summary>Player Roles.</summary>
{
	SupportDefender, 											/// <summary>Support Defender Player Role.</summary>
	MainDefender, 												/// <summary>Main Defender Player Role.</summary>
	SupportAttacker, 											/// <summary>Support Attacker Player Role.</summary>
	MainAttacker 												/// <summary>Main Attacker Player Role.</summary>
}

public abstract class TeamManager : MonoBehaviour, ILoadable
{
	private const int TEAM_SIZE = 4; 							/// <summary>Size of the Team.</summary>
	public const int POWER_UP_INVENTORY_SIZE = 2; 				/// <summary>Size of the Power-Ups inventory.</summary>

	[Header("Team Data:")]
	[SerializeField] private TeamData _teamData; 				/// <summary>Team's Data.</summary>
	[Space(5f)]
	[Header("Players:")]
	[SerializeField] private List<GameObject> _teamMembers; 	/// <summary>Team Members.<summary>
	[Space(5f)]
	[Header("DirectionReference:")]
	[SerializeField] private Vector3 _movementDirection; 		/// <summary>Team's Movement Direction Reference.</summary>
	[Space(5f)]
	[Header("GUI Feedback:")]
	[SerializeField] private TeamGUI _teamGUI; 					/// <summary>Team Manager's GUI.</summary>
	private List<BasePowerUp> _powerUpsInventory; 					/// <summary>Team's Power-Ups inventory.</summary> 
	private Dictionary<PlayerRoles, Player> _teamPlayers; 		/// <summary>Team Members's Dictionary.</summary>
	private List<Player> _players;								/// <summary>Team Player scripts .</summary>
	private List<PositionTree> _positionTrees; 					/// <summary>Player's PositionTree Scripts.</summary>
	private List<PlayerVitalityGUI> _playerGUIs; 				/// <summary>Team PlayerVitalityGUIs for each member .</summary>
	private List<float> _playersHeigths; 						/// <summary>Holds the players Heights.</summary>
	private List<Vector3> _formationPositions; 					/// <summary>The formation positions assigned for each Team Member.</summary>
	private WaypointController _waypointController; 			/// <summary>Team Manager's WaypointController</summary>
	private Transform _goal; 									/// <summary>Goal of the Team.</summary>
	private GameObject _selectedPlayer;							/// <summary>Currently selected player.</summary>
	private GameObject _nearestPlayer;							/// <Summary>Player nearest to the ball.</summary>
	private GameObject _secondNearestPlayer; 					/// <Summary>Second Player nearest to the ball.</summary>
	private GameObject _ball; 									/// <Summary>The ball of the game session.</summary>
	private Paradigms _paradigm; 								/// <summary>Team's current Paradigm.</summary>
	private bool _pressedSwitch; 								/// <summary>Is the Player Switcher button being pressed?.</summary>
	private bool _loaded;

	public enum Paradigms 										/// <summary>Team attitude Paradigms</summary>
	{
		Offensive, 												/// <summary>Offensive attitude Paradigm</summary>
		Defensive, 												/// <summary>Defensive attitude Paradigm</summary>	
		Passive 												/// <summary>Passive attitude Paradigm</summary>		
	}

#region Getters/Setters:
	/// <summary>Gets and Sets teamData property.</summary>
	public TeamData teamData
	{
		get { return _teamData; }
		set { _teamData = value; }
	}

	/// <summary>Gets and Sets players property.</summary>
	public List<Player> players 
	{
		get { return _players; }
		set { _players = value; }
	}

	/// <summary>Gets and Sets movementDirection property.</summary>
	public Vector3 movementDirection 
	{
		get { return _movementDirection; }
		set { _movementDirection = value; }
	}

	/// <summary>Gets waypointController property.</summary>
	public WaypointController waypointController 
	{
		get
		{
			if(GetComponent<WaypointController>() != null)
			{
				_waypointController = GetComponent<WaypointController>();
			}

			return _waypointController;
		}
	}

	/// <summary>Gets and Sets playerGUIs property.</summary>
	public List<PlayerVitalityGUI> playerGUIs 
	{
		get { return _playerGUIs; }
		set { _playerGUIs = value; }
	}

	/// <summary>Gets and Sets teamMembers property.</summary>
	public List<GameObject> teamMembers 
	{
		get { return _teamMembers; }
		set { _teamMembers = value; }
	}

	/// <summary>Gets and Sets formationPositions property.</summary>
	public List<Vector3> formationPositions 
	{
		get { return _formationPositions; }
		set { _formationPositions = value; }
	}

	/// <summary>Gets and Sets goal property.</summary>
	public Transform goal 
	{
		get { return _goal; }
		set { _goal = value; }
	}

	/// <summary>Gets and Sets selectedPlayer property.</summary>
	public GameObject selectedPlayer 
	{
		get { return _selectedPlayer; }
		set { _selectedPlayer = value; }
	}

	/// <summary>Gets and Sets nearestPlayer property.</summary>
	public GameObject nearestPlayer 
	{
		get { return _nearestPlayer; }
		set { _nearestPlayer = value; }
	}

	/// <summary>Gets and Sets secondNearestPlayer property.</summary>
	public GameObject secondNearestPlayer 
	{
		get { return _secondNearestPlayer; }
		set { _secondNearestPlayer = value; }
	}

	/// <summary>Gets and Sets ball property.</summary>
	public GameObject ball 
	{
		get { return _ball; }
		set { _ball = value; }
	}

	/// <summary>Gets and Sets teamGUI property.</summary>
	public TeamGUI teamGUI { get { return _teamGUI; } }

	/// <summary>Gets and Sets powerUpsInventory property.</summary>
	public List<BasePowerUp> powerUpsInventory
	{
		get { return _powerUpsInventory; }
		set { _powerUpsInventory = value; }
	}

	/// <summary>Gets and Sets teamPlayers property.</summary>
	public Dictionary<PlayerRoles, Player> teamPlayers 
	{
		get { return _teamPlayers; }
		set { _teamPlayers = value; }
	}

	/// <summary>Gets and Sets paradigm property.</summary>
	public Paradigms paradigm 
	{
		get { return _paradigm; }
		set { _paradigm = value; }
	}

	/// <summary>Gets and Sets pressedSwitch property.</summary>
	public bool pressedSwitch 
	{
		get { return _pressedSwitch; }
		set { _pressedSwitch = value; }
	}

	/// <summary>Gets and Sets Loaded property.</summary>
	public bool Loaded
	{
		get { return _loaded; }
		set { _loaded = value; }
	}
#endregion

	void Awake()
	{
		TeamInitializationProceedures();
	}

	/// <summary>Check Input to Switch Player.</summary>
	void Update()
	{
		//pressedSwitch = (Input.GetKeyDown(KeyCode.Q));
		if(Input.GetKeyUp(KeyCode.Q)) SwitchPlayer();
		if(Input.GetKeyUp(KeyCode.L)) SwitchPowerUps();
	}

#region Methods:

	/// <summary>Excecutes Team Initialization Proceedures [Instantiate Players, assign positions, roles, teams, etc.].</summary>
	public void TeamInitializationProceedures()
	{
		_paradigm = Paradigms.Passive;

		//Initialize Lists.
		_teamPlayers = new Dictionary<PlayerRoles, Player>();
		_players = new List<Player>();
		_positionTrees = new List<PositionTree>();
		powerUpsInventory = new List<BasePowerUp>();
		for(int i = 0; i < POWER_UP_INVENTORY_SIZE; i++)
		{
			powerUpsInventory.Add(null);
		}
		if(teamGUI != null) teamGUI.UpdateGUI(this);
		//_playerGUIs = new List<PlayerVitalityGUI>();
		//_playersHeigths = new List<float>();

		Vector3 newPos; 			// Position relative to the waypoint Y and the player's height.
		float newY; 				// Y coordinate relative to the waypoint Y and the player's height.
		float actualPlayerHeight; 	// Current Player's Height

		AssignGoal();

		for(int i = 0; i < TEAM_SIZE; i++)
		{
			Vector3 waypointPosition = waypointController.waypointsPositions[i];

			//0. Store Players Heights from the prefabs scripts.
			//_playersHeigths.Add(_teamMembers[i].GetComponent<Player>().collider.bounds.size.y);
			actualPlayerHeight = _teamMembers[i].GetComponent<Player>().collider.bounds.size.y;
			//1. Store new Positions for each new Player instantiated.
			newY = (waypointPosition.y + actualPlayerHeight);
			newPos = new Vector3(waypointPosition.x, newY, waypointPosition.z);
			//2 Instantiate Players.
			//_teamMembers[i] = Instantiate(_teamMembers[i], newPos, transform.rotation) as GameObject;
			_teamMembers[i] = Instantiate(_teamData.players[i].gameObject, newPos, transform.rotation) as GameObject;
			//3. Store their Players and PositionTree Scripts.
			_players.Add(teamMembers[i].GetComponent<Player>());
			_positionTrees.Add(teamMembers[i].GetComponent<PositionTree>());
			//4. Disable Player and PositionTrees.
			_players[i].enabled = false;
			_positionTrees[i].enabled = true;
			//5. Assign Team and Roles.
			_players[i].team = this;
			_players[i].role = (PlayerRoles)(i); //The role of the current iterator.
			//6.Add To Team Players Dictionary.
			_teamPlayers.Add(_players[i].role, _players[i]);
			_positionTrees[i].SetBehaviorTree();
		}

		Loaded = true;
	}

	/// <summary>Measures all players distances, and updates the two nearest ones.</summary>
	public void MeasurePlayers()
	{
		List<float> distances = _teamMembers.GetDistancesFromObject(_ball);

		_nearestPlayer = _teamMembers[distances.IndexOf(Mathf.Min(distances.ToArray()))];
		if(_teamMembers.Count > 1) _secondNearestPlayer = teamMembers[distances.IndexOf(distances.GetSecondMinimum())];
	}

	/// <summary>Switches the selected player with the closest (or second closest) one player.</summary>
	public void SwitchPlayer()
	{
		MeasurePlayers();
		if(selectedPlayer != null)
		{
			selectedPlayer.GetComponent<PlayerModel>().enabled = false;
			selectedPlayer.GetComponent<PositionTree>().enabled = true;
		}
		selectedPlayer = selectedPlayer == nearestPlayer ? secondNearestPlayer : nearestPlayer;

		selectedPlayer.GetComponent<PositionTree>().enabled = false;
		selectedPlayer.GetComponent<PlayerModel>().enabled = true;
	}

	/// <summary>Adds Power-Up to the Inventory.</summary>
	/// <param name="_powerUp">Power-Up to add to the Inventory.</param>
	public virtual void AddPowerUp(BasePowerUp _powerUp)
	{
		for(int i = 0; i < powerUpsInventory.Count; i++)
		{
			if(powerUpsInventory[i] == null)
			{ /// If the current Item Box on the inventory is empty, add the Power-Up there.
				powerUpsInventory[i] = _powerUp;
				break;
			}
		}

		teamGUI.UpdateGUI(this);
	}

	/// <summary>Switches Power-Up's order on the Inventory.</summary>
	public virtual void SwitchPowerUps()
	{
		powerUpsInventory.LonkSort(false);
		teamGUI.UpdateGUI(this);
	}

	/// <summary>Activates first Power-Up on the Power-Up's Inventory.</summary>
	/// <param name="_player">Player who requested the Power-Up's activation.</param>
	public virtual void UsePowerUp(PlayerModel _player)
	{
		BasePowerUp powerUp = powerUpsInventory[0];
		powerUpsInventory.RemoveAt(0);
		powerUpsInventory.Add(null);

		teamGUI.UpdateGUI(this);
		powerUp.ActivatePowerUp(_player);
	}

	/// <summary>Assigns Goal relative to the Team [Local or Visitor].</summary>
	public abstract void AssignGoal();

	/// <summary>Changes Paradigm depending what Team last collided with the Ball.</summary>
	/// <param name="_team">Team that last hitted the Ball.</summary>.
	public abstract void CheckParadigmShift(TeamManager _team);
#endregion

	void OnEnable()
	{
		Ball.onBallCollidedWithPlayer += CheckParadigmShift;
	}

	void OnDisable()
	{
		Ball.onBallCollidedWithPlayer -= CheckParadigmShift;
	}

	/*void OnEnable()
	{
		SceneAdministrator.onSceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneAdministrator.onSceneLoaded -= OnSceneLoaded;
	}

	public abstract void OnSceneLoaded(Scene _scene);*/
}
}