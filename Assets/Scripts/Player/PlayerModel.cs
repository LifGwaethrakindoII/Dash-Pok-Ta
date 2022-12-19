using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidlessUtilities;
using DashPokTaGameData;
using DashPokTa;

public enum StatusConditions
{
	Unassigned, 															/// <summary>Default Status Condition.</summary>
	Paralized, 																/// <summary>Paralized Status Conditions.</summary>
	Invincible, 															/// <summary>Invincible Status Condition.</summary>
	Poisoned, 																/// <summary>Poisoned Status Condition.</summary>
	Burned, 																/// <summary>Burned Status Condition.</summary>
	Cursed 																	/// <summary>Cursed Status Condition.</summary>
}

public abstract class PlayerModel : MonoBehaviour, IFiniteStateMachine<PlayerModel.States>, IFiniteStateMachine<StatusConditions>, IPlaygroundZoneCheck
{

	/// \TODO Change all individual Action bools for BitWise operators [e.g., action = ActionTypes.Walk | ActionTypes.UsePowerUp | ActionTypes.Jumping]

#region Constants:
	protected const string GROUNDED = "grounded"; 							/// <summary>Animator Controller's Grounded Key ID.</summary>
	protected const string WALK = "walking"; 								/// <summary>Animator Controller's Walking Key ID.</summary>
	protected const string RUN = "running"; 								/// <summary>Animator Controller's Running Key ID.</summary>
	protected const string JUMP = "jumping"; 								/// <summary>Animator Controller's Jumping Key ID.</summary>
	protected const string DASH = "dashing"; 								/// <summary>Animator Controller's Dashing Key ID.</summary>
	protected const string RECOVER = "recovering"; 							/// <summary>Animator Controller's Racovering Key ID.</summary>
	protected const string HIT = "hit"; 									/// <summary>Animator Controller's Hit Key ID.</summary>
	protected const string SPECIAL = "specialMove"; 						/// <summary>Animator Controller's Special Move Key ID.</summary>
#endregion

#region Properties:
	[Header("Stats:")]
	[SerializeField] private PlayerStats _stats; 							/// <summary>Player's Stats.</summary>
	[Space(5f)]
	[Header("Vitality Atributes:")]
	[SerializeField] [Range(1.0f, 100.0f)] private float _maxHp;			/// <summary>The Maximum amount of Hp the player can have.</summary>
	[SerializeField] private float _hpRecoveryTime; 						/// <summary>The Time the Hp will take to recover.</summary>
	[SerializeField] [Range(1.0f, 100.0f)] private float _maxStamina; 		/// <summary>The Maximum amount of Stamina the player can have.</summary>
	[SerializeField] private float _staminaRecoveryTime; 					/// <summary>The time the Stamina will take if reached '0'.</summary>
	[Space(5)]
	[Header("Movement Speed Atributes:")]
	[SerializeField] private float _initialSpeed; 							/// <summary>The initial, also minimum, speed the player will begin with.</summary>
	[SerializeField] private float _speedAccelerationTime;					/// <summary>The time it takes to reach from the initialSpeed to the maxSpeed.</summary>
	[SerializeField] private float _maxSpeed;								/// <summary>The Maximum Speed the player can reach.</summary>
	[SerializeField] private float _runStaminaSpend; 						/// <summary>The Stamina loss rate when running.</summary>
	[Space(5)]
	[Header("Jump Atributes:")]
	[SerializeField] private float _initialJumpForce; 						/// <summary>The initial, also minimum, jump force the player will begin with.</summary>
	[SerializeField] private float _jumpAccelerationTime;					/// <summary>The time it takes to reach from the initialJumpForce to the maxJumpForce.</summary>
	[SerializeField] private float _maxJumpForce; 							/// <summary>The Maximum Jump Force the player can reach.</summary>
	[SerializeField] private float _jumpCooldownTime;						/// <summary>The time the jump cooldown will take before jumping again.</summary>
	[SerializeField] private float _jumpStaminaSpend;						/// <summary>The Stamina loss rate when jumping.</summary>
	[SerializeField] [Range(0.01f, 1.5f)] private float _distanceToGround; 	/// <summary>The minimum distance required from the player to the ground be considered to be on ground.</summary>
	[Space(5)]
	[Header ("Dash Atributes:")]
	[SerializeField] private float _initialDashForce; 						/// <summary>The initial, also the minimum, Dash Force the Player will be with</summary>
	[SerializeField] private float _dashAccelerationTime; 					/// <summary>The time it takes to reach from the initial Dash Force to the max Dash Force.</summary>
	[SerializeField] private float _maxDashForce; 							/// <summar>The maximum Dash Force amount the Player can exert.</summary>
	[SerializeField] private float _dashCooldownTime; 						/// <summary>The time the dash cooldown will take before dashing again.</summary>
	[SerializeField] private float _dashStaminaSpend; 						/// <summary>The Stamina loss rate when dashing.</summary>
	[Space(5)]
	[SerializeField] private float _specialMoveStaminaSpend; 				/// <summary>The stamina loss rate when doing special move.<summary>
	[Space(5f)]
	[SerializeField] private float _reactionTime; 							/// <summary>Time the AI player will take to react.</summary>
	[Space(5f)]
	[Header("Hit Normals Debug:")]
	[SerializeField] private Color _hitNormalRayColor; 						/// <summary>Normal Ray's Color.</summary>
	[SerializeField] private Color _hitNormalSphereColor; 					/// <summary>Normal Sphere's Color.</summary>
	[SerializeField] private float _hitNormalSphereRadius; 					/// <summary>SphereCast radius.</summary>
	[SerializeField] private float _hitNormalRayLength; 							/// <summary>SphereCast length.</summary>
	[Header("GUI Feedback:")]
	[SerializeField] private Sprite _characterImage; 						/// <summary>Character's Presentation image.</summary>
	private TeamManager _team;												/// <summary>The team the player is playing for.</summary>
	private Transform _directionReference; 									/// <summary>Reference where the player will move relative to the normals of the transform.</summary>
	private Transform _referenceZone; 										/// <summary>Reference Zone from where the player will be moving.</summary>
	private float _hp;														/// <summary>Current Hp of the player.</summary>
	private float _stamina;													/// <summary>Current Stamina of the player.</summary>
	private float _speed;													/// <summary>Current Speed of the player.</summary>
	private float _jumpForce;												/// <summary>Current Jump Force of the player.</summary>
	private float _jumpCooldown;											/// <summary>Current Jump Cooldown of the player.</summary>
	private float _hpNormalMultiplier; 										/// <summary>The Hp normal multiplier, equals [1 / maxHp], to avoid divisions on Update.</summary>
	private float _staminaNormalMultiplier; 								/// <summary>The Stamina normal multiplier, equals [1 / maxStamina], to avoid divisions on Update.</summary>
	private float _jumpingNormalMultiplier; 								/// <summary>The Jumping normal multiplier, equals [1 / maxJumping], to avoid divisions on Update.</summary>
	private float _runningNormalMultiplier; 								/// <summary>The Runnig normal multiplier, equals [1 / maxRunning], to avoid divisions on Update.</summary>
	private float _dashingNormalMultiplier; 								/// <summary>The Dashing normal multiplier, equals [1 / maxDashing], to avoid divisions on Update.</summary>
	private float _dashForce;												/// <summary>The Dash Force applied.</summary>
	private float _dashCooldown;											/// <summary>Current Dash Cooldown of the player.</summary>
	private float _verticalAxis; 											/// <summary>Input's Vertical Axis.</summary>
	private float _horizontalAxis; 											/// <summary>Input's Horizontal Axis.</summary>
	private Rigidbody _rigidBody;											/// <summary>Player's Rigidbody.</summary>
	private Animator _animator;												/// <summary>Player's Animator.</summary>
	private BoxCollider _collider;											/// <summary>Player's BoxCollider.</summary>
	private MeshRenderer _renderer; 										/// <summary>Player's Mesh Renderer.</summary>
	private Ray _groundChecker; 											/// <summary>Player's Ray. Casted from the inferior bounds of its collider.</summary>
	private bool _isGrounded; 												/// <summary>Player's Is Grounded Flag.</summary>
	private bool _axisesBelowZero; 											/// <summary>Axis (Horizontal and Vertical) Above 0.0f Flag.</summary>
	private bool _running; 													/// <summary>Player's Running Flag.</summary>
	private bool _jumping;													/// <summary>Player's Jumping Flag.</summary>
	private bool _dashing; 													/// <summary>Player's Dashing Flag.</summary>
	private bool _specialMove; 												/// <summary>Player's Special Move Flag.</summary>
	private bool _staminaRecharging; 										/// <summary>Player's Stamina Recovery Flag.</summary>
	private bool _usePowerUp; 												/// <summary>Player using Power-Up flag.</summary>
	private Vector3 _direction; 											/// <summary>Player's Direction facing.</summary>
	private Vector3 _movingDirection; 										/// <summary>Player's moving direction.</summary>
	private Behavior _jumpCooldownController; 								/// <summary>StartJumpCoolDown Coroutine Controller.</summary>
	private Behavior _dashCooldownController; 								/// <summary>StartDashCooldown Coroutine Controller.</summary>
	private Behavior _staminaRechargeController; 							/// <summary>Wair Till Stamina Reloads Coroutine Controller.</summary>
	private States _state; 													/// <summary>Player's current States state.</summary>
	private Actions _actions; 												/// <summary>Current set of Actions the Player is executing.</summary>
	private PlayerRoles _role; 												/// <summary>Player's Role on the Team.</summary>
	private Playground.PlaygroundZones _currentZone; 						/// <summary>Current Playground Zone the character is on.</summary>
	private StatusConditions _statusCondition; 								/// <summary>Current Player's Status Condition.</summary>
	
	private Vector3[] _hitNormals = new Vector3[] 							/// <summary>Normal identities where the SphereCasts will be casted.</summary>
	{
		new Vector3(1f, 0f, 0f), 											/// <summary>Right Hit Normal.</summary>
		new Vector3(-1f, 0f, 0f), 											/// <summary>Left Hit Normal.</summary>
		new Vector3(0f, 1f, 0f), 											/// <summary>Up Hit Normal.</summary>
		new Vector3(0f, -1f, 0f), 											/// <summary>Down Hit Normal.</summary>
		new Vector3(0f, 0f, 1f), 											/// <summary>Forward Hit Normal.</summary>
		new Vector3(0f, 0f, -1f), 											/// <summary>Backward Hit Normal.</summary>
		new Vector3(1f, 1f, 0f), 											/// <summary>Right & Up Hit Normal.</summary>
		new Vector3(-1f, 1f, 0f), 											/// <summary>Left & Up Hit Normal.</summary>
		new Vector3(1f, -1f, 0f), 											/// <summary>Right & Down Hit Normal.</summary>
		new Vector3(-1f, -1f, 0f), 											/// <summary>Left & Down Hit Normal.</summary>
		new Vector3(1f, 0f, 1f), 											/// <summary>Right & Forward Hit Normal.</summary>
		new Vector3(-1f, 0f, 1f), 											/// <summary>Left & Forward Hit Normal.</summary>
		new Vector3(1f, 0f, -1f), 											/// <summary>Right & Backward Hit Normal.</summary>
		new Vector3(-1f, 0f, -1f), 											/// <summary>Left & Backward Hit Normal.</summary>
		new Vector3(0f, 1f, 1f), 											/// <summary>Up & Forward Hit Normal.</summary>
		new Vector3(0f, -1f, 1f), 											/// <summary>Down & Forwrd Hit Normal.</summary>
		new Vector3(0f, 1f, -1f), 											/// <summary>Up & Backward Hit Normal.</summary>
		new Vector3(0f, -1f, -1f), 											/// <summary>Down & Backward Hit Normal.</summary>
		new Vector3(1f, 1f, 1f), 											/// <summary>Right & Up & Forward Hit Normal.</summary>
		new Vector3(-1f, 1f, 1f), 											/// <summary>Left & Up & Forward Hit Normal.</summary>
		new Vector3(1f, -1f, 1f), 											/// <summary>Right & Down & Forward Hit Normal.</summary>
		new Vector3(-1f, -1f, 1f), 											/// <summary>Left & Down & Forward Hit Normal.</summary>
		new Vector3(1f, 1f, -1f), 											/// <summary>Right & Up & Backward Hit Normal.</summary>
		new Vector3(-1f, 1f, -1f), 											/// <summary>Left & Up & Backward Hit Normal.</summary>
		new Vector3(1f, -1f, -1f), 											/// <summary>Right & Down & Backward Hit Normal.</summary>
		new Vector3(-1f, -1f, -1f) 											/// <summary>Left & Down & Backward Hit Normal.</summary>
	};

	public enum StaminaTypes 												/// <summary>Stamina Loss Rate Type.</summary>
	{
		Run, 																/// <summary>Run Loss Rate Type.</summary>
		Jump, 																/// <summary>Jump Loss Rate Type.</summary>
		Dash 																/// <summary>Dash Loss Rate Type.</summary>
	}

	public enum States 														/// <summary>Player's State.</summary>
	{
		Unassigned,															/// <summary>Player's Unassigned State.</summary>
		Alive, 																/// <summary>Player's Alive State.</summary>
		Hitted, 															/// <summary>Player's Hited State.</summary>
		Recovering, 														/// <summary>Player's Recovering State.</summary>
		Fainted 															/// <summary>Player's Fainted State.</summary>
	}

	[System.Flags]
	public enum Actions 													/// <summary>Actions the Player may execute.</summary>
	{
		Idle = 0, 															/// <summary>Default Unassigned Action.</summary>
		Walking = 1, 														/// <summary>Walking Action.</summary>
		Running = 2, 														/// <summary>Running Action.</summary>
		Jumping = 4, 														/// <summary>Jumping Action.</summary>
		Dashing = 8, 														/// <summary>Dashing Action.</summary>
		SpecialMove = 16, 													/// <summary>Special Move Action.</summary>
		UsingPowerUp = 32 													/// <summary>Using Power-Up Action.</summary>
	}
#endregion

#region Getters/Setters:
	/// <summary>Gets and Sets stats property.</summary>
	public PlayerStats stats { get { return _stats; } }

	/// <summary>Gets and Sets statusCondition property.</summary>
	public StatusConditions statusCondition
	{
		get { return _statusCondition; }
		protected set
		{
			ExitState(_statusCondition);
			EnterState(_statusCondition = value);
		}
	}

	/// <summary>Gets and Sets maxHp property.</summary>
	public float maxHp
	{
		get { return _maxHp; }
		set { _maxHp = value; }
	}

	/// <summary>Gets and Sets hpRecoveryTime property.</summary>
	public float hpRecoveryTime
	{
		get { return _hpRecoveryTime; }
		set { _hpRecoveryTime = value; }
	}

	/// <summary>Gets and Sets maxStamina property.</summary>
	public float maxStamina
	{
		get { return _maxStamina; }
		set { _maxStamina = value; }
	}

	/// <summary>Gets and Sets staminaRecoveryTime property.</summary>
	public float staminaRecoveryTime
	{
		get { return _staminaRecoveryTime; }
		set { _staminaRecoveryTime = value; }
	}

	/// <summary>Gets and Sets initialSpeed property.</summary>
	public float initialSpeed
	{
		get { return _initialSpeed; }
		set { _initialSpeed = value; }
	}

	/// <summary>Gets and Sets speedAccelerationTime property.</summary>
	public float speedAccelerationTime
	{
		get { return _speedAccelerationTime; }
		set { _speedAccelerationTime = value; }
	}

	/// <summary>Gets and Sets maxSpeed property.</summary>
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set { _maxSpeed = value; }
	}

	/// <summary>Gets and Sets runStaminaSpend property.</summary>
	public float runStaminaSpend
	{
		get { return _runStaminaSpend; }
		set { _runStaminaSpend = value; }
	}

	/// <summary>Gets and Sets speed property.</summary>
	public float speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	/// <summary>Gets and Sets jumpForce property.</summary>
	public float jumpForce
	{
		get { return _jumpForce; }
		set { _jumpForce = value; }
	}

	/// <summary>Gets and Sets initialJumpForce property.</summary>
	public float initialJumpForce
	{
		get { return _initialJumpForce; }
		set { _initialJumpForce = value; }
	}

	/// <summary>Gets and Sets jumpAccelerationTime property.</summary>
	public float jumpAccelerationTime
	{
		get { return _jumpAccelerationTime; }
		set { _jumpAccelerationTime = value; }
	}

	/// <summary>Gets and Sets maxJumpForce property.</summary>
	public float maxJumpForce
	{
		get { return _maxJumpForce; }
		set { _maxJumpForce = value; }
	}

	/// <summary>Gets and Sets jumpCoolDown property.</summary>
	public float jumpCooldown
	{
		get { return _jumpCooldown; }
		set { _jumpCooldown = value; }
	}

	/// <summary>Gets and Sets jumpCooldownTime property.</summary>
	public float jumpCooldownTime
	{
		get { return _jumpCooldownTime; }
		set { _jumpCooldownTime = value; }
	}

	/// <summary>Gets and jumpStaminaSpend maxHp property.</summary>
	public float jumpStaminaSpend
	{
		get { return _jumpStaminaSpend; }
		set { _jumpStaminaSpend = value; }
	}

	/// <summary>Gets and Sets initialDashForce property.</summary>
	public float initialDashForce
	{
		get { return _initialDashForce; }
		set { _initialDashForce = value; }
	}

	/// <summary>Gets and Sets dashAccelerationTime property.</summary>
	public float dashAccelerationTime
	{
		get { return _dashAccelerationTime; }
		set { _dashAccelerationTime = value; }
	}

	/// <summary>Gets and Sets dashForce property.</summary>
	public float dashForce
	{
		get { return _dashForce; }
		set { _dashForce = value; }
	}

	/// <summary>Gets and Sets maxDashForce property.</summary>
	public float maxDashForce
	{
		get { return _maxDashForce; }
		set { _maxDashForce = value; }
	}

	/// <summary>Gets and Sets dashCooldown property.</summary>
	public float dashCooldown	
	{
		get { return _dashCooldown; }
		set { _dashCooldown = value; }
	}

	/// <summary>Gets and Sets verticalAxis property.</summary>
	public float verticalAxis
	{
		get { return _verticalAxis; }
		set { _verticalAxis = value; }
	}

	/// <summary>Gets and Sets horizontalAxis property.</summary>
	public float horizontalAxis
	{
		get { return _horizontalAxis; }
		set { _horizontalAxis = value; }
	}

	/// <summary>Gets and Sets dashCooldownTime property.</summary>
	public float dashCooldownTime
	{
		get { return _dashCooldownTime; }
		set { _dashCooldownTime = value; }
	}

	/// <summary>Gets and Sets dashStaminaSpend property.</summary>
	public float dashStaminaSpend
	{
		get { return _dashStaminaSpend; }
		set { _dashStaminaSpend = value; }
	}

	/// <summary>Gets and Sets specialMoveStaminaSpend property.</summary>
	public float specialMoveStaminaSpend
	{
		get { return _specialMoveStaminaSpend; }
		set { _specialMoveStaminaSpend = value; }
	}

	/// <summary>Gets and Sets distanceToGround property.</summary>
	public float distanceToGround
	{
		get { return _distanceToGround; }
		set { _distanceToGround = value; }
	}

	/// <summary>Gets and Sets hp property.</summary>
	public float hp 
	{
		get { return _hp; }
		set { _hp = value; }
	}

	/// <summary>Gets and Sets stamina property.</summary>
	public float stamina 
	{
		get { return _stamina; }
		set { _stamina = value; }
	}

	/// <summary>Gets and Sets reactionTime property.</summary>
	public float reactionTime 
	{
		get { return _reactionTime; }
		set { _reactionTime = value; }
	}

	/// <summary>Gets and Sets hpNormalMultiplier property.</summary>
	public float hpNormalMultiplier 
	{
		get { return _hpNormalMultiplier; }
		set { _hpNormalMultiplier = value; }
	}

	/// <summary>Gets and Sets staminaNormalMultiplier property.</summary>
	public float staminaNormalMultiplier 
	{
		get { return _staminaNormalMultiplier; }
		set { _staminaNormalMultiplier = value; }
	}

	/// <summary>Gets and Sets jumpingNormalMultiplier property.</summary>
	public float jumpingNormalMultiplier 
	{
		get { return _jumpingNormalMultiplier; }
		set { _jumpingNormalMultiplier = value; }
	}

	/// <summary>Gets and Sets runningNormalMultiplier property.</summary>
	public float runningNormalMultiplier 
	{
		get { return _runningNormalMultiplier; }
		set { _runningNormalMultiplier = value; }
	}

	/// <summary>Gets and Sets dashingNormalMultiplier property.</summary>
	public float dashingNormalMultiplier 
	{
		get { return _dashingNormalMultiplier; }
		set { _dashingNormalMultiplier = value; }
	}

	/// <summary>Gets and Sets hitNormalSphereRadius property.</summary>
	public float hitNormalSphereRadius
	{
		get { return _hitNormalSphereRadius; }
		set { _hitNormalSphereRadius = value; }
	}

	/// <summary>Gets and Sets hitNormalRayLength property.</summary>
	public float hitNormalRayLength
	{
		get { return _hitNormalRayLength; }
		set { _hitNormalRayLength = value; }
	}

	/// <summary>Gets and Sets characterImage property.</summary>
	public Sprite characterImage { get { return _characterImage; } }

	/// <summary>Gets hitNormalRayColor property.</summary>
	public Color hitNormalRayColor { get { return _hitNormalRayColor; } }

	/// <summary>Gets hitNormalSphereColor property.</summary>
	public Color hitNormalSphereColor { get { return _hitNormalSphereColor; } }

	/// <summary>Gets rigidbody property.</summary>
	public Rigidbody rigidBody
	{
		get
		{
			if(_rigidBody == null)
			{
				_rigidBody = GetComponent<Rigidbody>();
			}

			return _rigidBody;
		}
	}

	/// <summary>Gets animator property.</summary>
	public Animator animator
	{
		get
		{
			if(_animator == null)
			{
				_animator = GetComponent<Animator>();
			}

			return _animator;
		}
	}

	/// <summary>Gets and Sets directionReference property.</summary>
	public Transform directionReference
	{
		get { return _directionReference; }
		set { _directionReference = value; }
	}

	/// <summary>Gets and Sets referenceZone property.</summary>
	public Transform referenceZone
	{
		get { return _referenceZone; }
		set { _referenceZone = value; }
	}

	/// <summary>Gets and Sets team property.</summary>
	public TeamManager team
	{
		get { return _team; }
		set { _team = value; }
	}

	public BoxCollider collider
	{
		get
		{
			if(_collider == null)
			{
				_collider = GetComponent<BoxCollider>();
			}

			return _collider;
		}
	}

	/// <summary>Gets renderer property.</summary>
	public MeshRenderer renderer
	{
		get
		{
			if(_renderer == null)
			{
				_renderer = GetComponent<MeshRenderer>();
			}

			return _renderer;
		}
	}

	/// <summary>Gets groundChecker property.</summary>
	public Ray groundChecker
	{
		get
		{
			//return _groundChecker = new Ray((transform.position + (-transform.up * (collider.bounds.extents.y + (collider.center.y * -1f)))), (-transform.up * ((collider.bounds.extents.y + (collider.center.y * -1f)) * _distanceToGround)));
			//return _groundChecker = new Ray((transform.position + (-transform.up * (collider.bounds.extents.y + (collider.center.y * -1f)))), (-transform.up * _distanceToGround));
			return _groundChecker = new Ray(transform.position + (-transform.up), ((-transform.up * collider.bounds.size.y ).AddToY(_distanceToGround)));
			//return _groundChecker = new Ray((transform.position + (-transform.up * (collider.bounds.size.y * 0.5f))), (-transform.up * (collider.bounds.extents.y * (collider.bounds.size.y * 0.5f)) * _distanceToGround) );
		}
	}

	/// <summary>Gets inGrounded Flag.</summary>
	public bool isGrounded
	{
		get
		{
			/*Debug.DrawRay(groundChecker.origin, (groundChecker.direction * _distanceToGround), Color.blue, 15f);
			RaycastHit[] hits = Physics.RaycastAll(groundChecker, _distanceToGround/*, LayerMask.NameToLayer(LayerMasks.FLOOR_LAYER_MASK_KEY));*/
			/*foreach(RaycastHit Hit in hits)
			{
				Debug.Log("[PlayerModel] Hit Info: " + Hit.transform.gameObject.tag);
				if(Hit.transform.gameObject.tag == LayerMasks.FLOOR_LAYER_MASK_KEY) return true;
			}

			return false;*/

			//return _isGrounded = Physics.Raycast(groundChecker.origin, (groundChecker.direction * _distanceToGround), _distanceToGround, LayerMask.NameToLayer(LayerMasks.FLOOR_LAYER_MASK_KEY));
			//return _isGrounded = Physics.CheckSphere(groundChecker.origin, (_distanceToGround * 2f), LayerMask.NameToLayer(L			ayerMasks.FLOOR_LAYER_MASK_KEY), QueryTriggerInteraction.UseGlobal);
			
			RaycastHit hitInfo;
			_isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hitInfo, (renderer.bounds.extents.y + stats.heightPadding), stats.groundLayer);

			return _isGrounded;
		}
		set { _isGrounded = value; }
	}

	/// <summary>Gets and Sets axisesBelowZero Flag.</summary>
	public bool axisesBelowZero 
	{
		get
		{
			return _axisesBelowZero = (_horizontalAxis != 0.0f || _verticalAxis != 0.0f);
		}
		set { _axisesBelowZero = value; }
	}

	/// <summary>Gets and Sets running Flag.</summary>
	public bool running 
	{
		get { return _running; }
		set { _running = value; }
	}

	/// <summary>Gets and Sets jumping Flag.</summary>
	public bool jumping 
	{
		get { return _jumping; }
		set { _jumping = value; }
	}

	/// <summary>Gets and Sets staminaRecharging Flag.</summary>
	public bool staminaRecharging 
	{
		get { return _staminaRecharging; }
		set { _staminaRecharging = value; }
	}

	/// <summary>Gets and Sets dashing Flag.</summary>
	public bool dashing 
	{
		get { return _dashing; }
		set { _dashing = value; }
	}

	/// <summary>Gets and Sets specialMove Flag.</summary>
	public bool specialMove 
	{
		get { return _specialMove; }
		set { _specialMove = value; }
	}

	/// <summary>Gets and Sets usePowerUp property.</summary>
	public bool usePowerUp
	{
		get { return _usePowerUp; }
		set { _usePowerUp = value; }
	}

	/// <summary>Gets and Sets direction property.</summary>
	public Vector3 direction
	{
		get { return _direction; }
		set { _direction = value; }
	}

	/// <summary>Gets and Sets movingDirection property.</summary>
	public Vector3 movingDirection
	{
		get { return _movingDirection; }
		set { _movingDirection = value; }
	}

	/// <summary>Gets and Sets jumpCooldownController property.</summary>
	public Behavior jumpCooldownController
	{
		get { return _jumpCooldownController; }
		set { _jumpCooldownController = value; }
	}

	/// <summary>Gets and Sets dashCooldownController property.</summary>
	public Behavior dashCooldownController
	{
		get { return _dashCooldownController; }
		set { _dashCooldownController = value; }
	}

	/// <summary>Gets and Sets staminaRechargeController property.</summary>
	public Behavior staminaRechargeController
	{
		get { return _staminaRechargeController; }
		set { _staminaRechargeController = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public States state
	{
		get { return _state; }
		set
		{
			ExitState(_state);
			EnterState(_state = value);
		}
	}

	/// <summary>Gets and Sets actions property.</summary>
	public Actions actions
	{
		get { return _actions; }
		set { _actions = value; }
	}

	/// <summary>Gets and Sets currentZone property.</summary>
	public Playground.PlaygroundZones currentZone
	{
		get
		{
			Ray zoneRay = new Ray((transform.position + Vector3.up), Vector3.down * 10f);
			RaycastHit[] hits = Physics.RaycastAll(zoneRay);

			//Debug.DrawRay((transform.position + Vector3.up), (Vector3.down * 10f), Color.blue, 5f);

			if(Physics.Raycast(zoneRay.origin, (zoneRay.direction * 10f), 10f))
			{
				foreach(RaycastHit hit in hits)
				{
					//Debug.LogWarning("Hitted tag: " + hit.transform.gameObject.tag);
					if(hit.transform.gameObject.tag == "PlaygroundZone")
					{
						return _currentZone = hit.transform.gameObject.GetComponent<PlaygroundZone>().playgroundZone;
					}
				}
			}

			return _currentZone = Playground.PlaygroundZones.Unassigned;
		}
		set { _currentZone = value; }
	}

	/// <summary>Gets and Sets role property.</summary>
	public PlayerRoles role
	{
		get { return _role; }
		set { _role = value; }
	}

	/// <summary>Gets hitNormals property.</summary>
	public Vector3[] hitNormals { get { return _hitNormals; } }
#endregion

#region FiniteStateMachine:
	/// <summary>Enters Player State.</summary>
	/// <param name="_state">Player State that will be entered.</param>
	public void EnterState(PlayerModel.States _state)
	{
		switch(_state)
		{
			case States.Unassigned:
			//...
			break;

			case States.Alive:
			//...
			break;

			case States.Fainted:
			//...
			break;
		}
	}

	/// <summary>Exited Player State.</summary>
	/// <param name="_state">Player State that will be left.</param>
	public void ExitState(PlayerModel.States _state)
	{
		switch(_state)
		{
			case States.Unassigned:
			//...
			break;

			case States.Alive:
			//...
			break;

			case States.Fainted:
			//...
			break;
		}
	}

	/// <summary>Enters StatusConditions State.</summary>
	/// <param name="_state">StatusConditions State that will be entered.</param>
	public void EnterState(StatusConditions _state)
	{
		switch(_state)
		{
			case StatusConditions.Unassigned:
			break;
	
			default:
			break;
		}
	}
	
	/// <summary>Leaves StatusConditions State.</summary>
	/// <param name="_state">StatusConditions State that will be left.</param>
	public void ExitState(StatusConditions _state)
	{
		switch(_state)
		{
			case StatusConditions.Unassigned:
			break;
	
			default:
			break;
		}
	}
#endregion

#region Methods:
	/// <summary>Initializes Player's Basic [Independent] Data.</summary>
	public abstract void InitializePlayerData();

	/// <summary>Checks the input states on the actual frame, to enable actions Flags.</summary>
	public abstract void TrackInput();

	/// <summary>Executes enabled Flags actions.</summary>
	public abstract void ExecuteActions();

	/// <summary>Udates Player's Animnator Controller parameters.</summary>
	public abstract void UpdateAnimatorControllerParameters();

	/// <summary>Turns the Gameobject to the given direction.</summary>
	/// <param name="_direction">The direction the player will be facing.</param>
	public abstract void FaceDirection(Vector3 _direction);

	/// <summary>Stops the Player.</summary>
	public abstract void Wait();

	/// <summary>Move the GameObject relative to its forward normal, at the initialSpeed.</summary>
	public abstract void Walk();

	/// <summary>Move the GameObject relative to its forward normal, at an accelerating speed.</summary>
	public abstract void Run();

	/// <summary>Moves the GameObject relative to its up normal.</summary>
	public abstract void Jump();

	/// <summary>Adds force to the GameObject relative to its forward normal.</summary>
	public abstract void Dash();

	/// <summary>Pushes Ball at Player's Force.</summary>
	/// <param name="_ball">Ball to Push.</param>
	public abstract void PushBall(BallModel _ball);

	/// <summary>Excecutes Character's Special Action.</summary>
	public abstract void SpecialAction();

	/// <summary>Accelerates the given value by its respective acceleration time.</summary>
	/// <param name="_staminaType">The value that will be accelerated [By StaminaLossRate Identifier].</param>
	/// <returns>Given value accelerated.</returns>
	public abstract float Accelerate(StaminaTypes _staminaType);

	/// <summary>Deaccelerates the given value by its respective acceleration time.</summary>
	/// <param name="_staminaType">The value that will be deaccelerated [By StaminaLossRate Identifier].</param>
	/// <returns>Given value deaccelerated.</returns>
	public abstract float Deaccelerate(StaminaTypes _staminaType);

	/// <summary>Deaccelerates the stamina by the given stamina loss rate.</summary>
	/// <param name="_staminaQuantity">The stamina quantity that will be lost.</param>
	/// <returns>Stamina deaccelerated.</returns>
	public abstract float DecreaseStamina(float _staminaQuantity);

	/// <summary>Checks if the Floor collided is below Player's feet.</summary>
	/// <param name="_floor">Floor that will be used to check the direction of the collision.</param>
	/// <returns>If the Floor collided is below Player's feet.</returns>
	public abstract bool FloorBelowPlayerFeet(Transform _floor);

	/// <summary>Increases Stamina at its normal recovery rate.</summary>
	public abstract void RecoverStamina();

	/// <summary>Shows the Hit Normals on Editor Mode, as they are going to be during collision with the Ball.</summary>
	public abstract void ShowHitNormals();

	protected virtual void AddActionFlag(Actions _action)
	{
		if(!HasActionFlag(_action)) actions |= _action;
	}

	protected virtual void AddActionFlags(params Actions[] _actions)
	{
		for(int i = 0; i < _actions.Length; i++)
		{
			AddActionFlag(_actions[i]);	
		}
	}

	protected virtual void RemoveActionFlag(Actions _action)
	{
		if(HasActionFlag(_action)) actions ^= _action;
	}

	protected virtual void RemoveActionFlags(params Actions[] _actions)
	{
		for(int i = 0; i < _actions.Length; i++)
		{
			RemoveActionFlag(_actions[i]);	
		}
	}

	protected virtual bool HasActionFlag(Actions _action)
	{
		return ((actions & _action) == _action);
	}

	protected virtual bool HasActionFlags(params Actions[] _actions)
	{
		for(int i = 0; i < _actions.Length; i++)
		{
			if(!HasActionFlag(_actions[i])) return false;	
		}

		return true;
	}

	/// <summary>Starts Jump Cooldown process.</summary>
	public abstract IEnumerator StartJumpCooldown();

	/// <summary>Starts Dash Cooldown process.</summary>
	public abstract IEnumerator StartDashCooldown();

	/// <summary>Begins Stamina Recovery process if stamina reached 0.</summary>
	public abstract IEnumerator WaitTillStaminaReloads();

	/// <summary>Executes Status Condition.</summary>
	/// <param name="_statusCondition">Status Condition to be Executed.</param>
	public abstract IEnumerator ExecuteStatusCondition(StatusConditions _statusCondition);
#endregion

}
