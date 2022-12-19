using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DashPokTa;

public abstract class BasePlayerBehaviorTree : MonoBehaviour
{
	private PlayerModel _player; 				/// <summary>GameObject's PlayerModel.</summary>
	private Behavior _treeTicker; 				/// <summary>Position Tree Coroutine Manager.</summary>
	private SelectorNode _rootSelector; 		/// <summary>Main Root Selector.</summary>
	private SelectorNode _supportDefenderRoot; 	/// <summary>Support Defender Root Selector.</summary>
	private SelectorNode _mainDefenderRoot; 	/// <summary>Main Defender Root Selector.</summary>
	private SelectorNode _supportAttackerRoot; 	/// <summary>Support Attacker Root Selector.</summary>
	private SelectorNode _mainAttackerRoot; 	/// <summary>Main Attacker Root Selector.</summary>
	private SelectorNode _actionRoot; 			/// <summary>Main Action Selector Root, usually is the one that gets ticked each frame.</summary>
	private MovementActions _movementAction; 	/// <summary>Current Movement Action.</summary>

	/// <summary>Movement Actions.</summary>
	public enum MovementActions
	{
		Wait, 									/// <summary>Wait Movement Action.</summary>
		Walk, 									/// <summary>Walk Movement Action.</summary>
		Run 									/// <summary>Run Movement Action.</summary>
	}

#region Getters/Setters:
	/// <summary>Gets and Sets player property.</summary>
	public PlayerModel player
	{
		get
		{
			if(_player == null)
			{
				_player = GetComponent<PlayerModel>();
			}

			return _player;
		}
	}

	/// <summary>Gets and Sets treeTicker property.</summary>
	public Behavior treeTicker
	{
		get { return _treeTicker; }
		set { _treeTicker = value; }
	}

	/// <summary>Gets and Sets rootSelector property.</summary>
	public SelectorNode rootSelector
	{
		get { return _rootSelector; }
		set { _rootSelector = value; }
	}

	/// <summary>Gets and Sets supportDefenderRoot property.</summary>
	public SelectorNode supportDefenderRoot
	{
		get { return _supportDefenderRoot; }
		set { _supportDefenderRoot = value; }
	}

	/// <summary>Gets and Sets mainDefenderRoot property.</summary>
	public SelectorNode mainDefenderRoot
	{
		get { return _mainDefenderRoot; }
		set { _mainDefenderRoot = value; }
	}

	/// <summary>Gets and Sets supportAttackerRoot property.</summary>
	public SelectorNode supportAttackerRoot
	{
		get { return _supportAttackerRoot; }
		set { _supportAttackerRoot = value; }
	}

	/// <summary>Gets and Sets mainAttackerRoot property.</summary>
	public SelectorNode mainAttackerRoot
	{
		get { return _mainAttackerRoot; }
		set { _mainAttackerRoot = value; }
	}

	/// <summary>Gets and Sets actionRoot property.</summary>
	public SelectorNode actionRoot
	{
		get { return _actionRoot; }
		set { _actionRoot = value; }
	}

	/// <summary>Gets and Sets movementAction property.</summary>
	public MovementActions movementAction
	{
		get { return _movementAction; }
		set { _movementAction = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>Create Player's Behavior Tree [by Role].</summary>
	void Awake()
	{
		//SetBehaviorTree();
	}

	void Start()
	{
		//SetBehaviorTree();
	}
#endregion

#region Methods:
	/// <summary>Sets role Behavior Tree.</summary>
	public void SetBehaviorTree()
	{
		switch(player.role)
		{
			case PlayerRoles.SupportDefender:
			_rootSelector = InitializeSupportDefenderTree();
			break;

			case PlayerRoles.MainDefender:
			_rootSelector = InitializeMaintDefenderTree();
			break;

			case PlayerRoles.SupportAttacker:
			_rootSelector = InitializeSupportAttackerTree();
			break;

			case PlayerRoles.MainAttacker:
			_rootSelector = InitializeMainAttackerTree();
			break;

			default:
			Debug.LogWarning("Have not role assigned.");
			break;
		}

		_actionRoot = InitializeActionTree();

		Debug.Log("Player Role: " + player.role.ToString());
	}

	/// <summary>Initializes Support Defender BehaviorTree.</summary>
	/// <returns>Support Defender Tree Setted.</summary>
	public abstract SelectorNode InitializeSupportDefenderTree();

	/// <summary>Initializes Main Defender BehaviorTree.</summary>
	/// <returns>Main Defender Tree Setted.</summary>
	public abstract SelectorNode InitializeMaintDefenderTree();

	/// <summary>Initializes Support Attacker BehaviorTree.</summary>
	/// <returns>Support Attacker Tree Setted.</summary>
	public abstract SelectorNode InitializeSupportAttackerTree();

	/// <summary>Initializes Main Attacker BehaviorTree.</summary>
	/// <returns>Main Attacker Tree Setted.</summary>
	public abstract SelectorNode InitializeMainAttackerTree();

	/// <summary>Initializes Action BehaviorTree.</summary>
	/// <returns>Main Attacker Tree Setted.</summary>
	public abstract SelectorNode InitializeActionTree();

	/// <summary>Ticks the Position Tree.</summary>
	public abstract IEnumerator RunTree();
#endregion

}