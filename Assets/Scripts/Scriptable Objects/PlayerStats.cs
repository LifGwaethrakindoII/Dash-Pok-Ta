using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dash-Pok-Ta / Create / Player Stats")]
public class PlayerStats : ScriptableObject
{

#region Properties:
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
	[SerializeField] private float _heightPadding; 							/// <summary>Height Padding when casting a ground check Ray.</summary> 
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
	[Header("Status Condition Attributes:")]
	[SerializeField] private float _invencibleDuration; 					/// <summary>Invencible Status Condition's duration.</summary>
	[Space(5f)]
	[Header("Layer Masks:")]
	[SerializeField] private LayerMask	_groundLayer; 						/// <summary>Ground's layer mask.</summary>
	[Space(5f)]
	[Header("Hit Normals Debug:")]
	[SerializeField] private Color _hitNormalRayColor; 						/// <summary>Normal Ray's Color.</summary>
	[SerializeField] private Color _hitNormalSphereColor; 					/// <summary>Normal Sphere's Color.</summary>
	[SerializeField] private float _hitNormalSphereRadius; 					/// <summary>SphereCast radius.</summary>
	[SerializeField] private float _hitNormalRayLength; 					/// <summary>SphereCast length.</summary>
#endregion

#region Getters/Setters:
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

	/// <summary>Gets and Sets heightPadding property.</summary>
	public float heightPadding { get { return _heightPadding; } }

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

	/// <summary>Gets and Sets maxDashForce property.</summary>
	public float maxDashForce
	{
		get { return _maxDashForce; }
		set { _maxDashForce = value; }
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

	/// <summary>Gets and Sets reactionTime property.</summary>
	public float reactionTime 
	{
		get { return _reactionTime; }
		set { _reactionTime = value; }
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

	/// <summary>Gets and Sets invencibleDuration property.</summary>
	public float invencibleDuration { get { return _invencibleDuration; } }

	/// <summary>Gets hitNormalRayColor property.</summary>
	public Color hitNormalRayColor { get { return _hitNormalRayColor; } }

	/// <summary>Gets hitNormalSphereColor property.</summary>
	public Color hitNormalSphereColor { get { return _hitNormalSphereColor; } }

	/// <summary>Gets and Sets groundLayer property.</summary>
	public LayerMask groundLayer { get { return _groundLayer; } }
#endregion

}