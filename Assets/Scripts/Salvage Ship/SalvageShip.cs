using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;

namespace DashPokTa
{
public class SalvageShip : Singleton<SalvageShip>, IFiniteStateMachine<SalvageShip.States>
{
	public delegate void OnBallDropped();
	public static event OnBallDropped onBallDropped;

	[SerializeField] private WaypointController shipWaypoints;
	[SerializeField] private Transform shipHoldingCenter;
	[SerializeField] private ParticleEffectController summoningPortalEfect;
	[SerializeField] [Range(0.1f, 15.0f)] private float distanceToGetClose;
	[SerializeField] [Range(0.1f, 1.0f)] private float distanceToGetCloseToWaypoint;
	[Space(5f)]
	[Header("Ship's Attributes:")]
	[SerializeField] private float minSpeed;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float attractionSpeed;
	[SerializeField] private float dealyBeforeDroppingSphere;
	[Space(5f)]
	[Header("Summoning Animation Attributes:")]
	[SerializeField] private float shipEntranceDuration;
	[SerializeField] private float shipExitDuration;
	[SerializeField] private float portalEntranceDuration;
	[SerializeField] private float portalExitDuration;
	[SerializeField] private float spawningProjectionOffset;
	private States _state;
	private Rigidbody _rigidbody;
	private Behavior summonShip;
	private Behavior unsummonShip;
	private Behavior spherePicker;

	public enum States
	{
		Unassigned,
		Summoned,
		ChasingBall,
		AttractingBall,
		ReturningBall,
		DroppingBall,
		Leaving
	}

#region Getters/Setters:
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

	/// <summary>Gets and Sets rigidbody property.</summary>
	public Rigidbody rigidbody
	{
		get
		{
			if(_rigidbody == null)
			{
				if(GetComponent<Rigidbody>() != null) _rigidbody = GetComponent<Rigidbody>();
			}

			return _rigidbody;
		}
	}
#endregion

#region FiniteStateMachine:
	/// <summary>Enters SalvageShip.States State.</summary>
	/// <param name="_state">SalvageShip.States State that will be entered.</param>
	public void EnterState(SalvageShip.States _state)
	{
		switch(_state)
		{
			case States.Unassigned:
			transform.localScale = Vector3.zero;
			gameObject.SetActive(false);
			summoningPortalEfect.gameObject.transform.localScale = Vector3.zero;
			summoningPortalEfect.gameObject.SetActive(false);
			break;

			case States.Summoned:
			gameObject.SetActive(true);
			transform.position = GetNewSpawnPosition();
			CoroutineController.Instance.Rotate(this, Quaternion.LookRotation(transform.position.GetDirectionTowards(BallModel.Instance.gameObject.transform.position)), dealyBeforeDroppingSphere, null);
			transform.localScale = Vector3.zero;
			summoningPortalEfect.gameObject.SetActive(true);
			summoningPortalEfect.gameObject.transform.position = transform.position;
			summoningPortalEfect.FadeInParticles(
			()=>
			{
				summonShip = new Behavior(this, SummonShip(
					()=>
					{
						state = States.ChasingBall;
					}));
			});
			break;

			case States.ChasingBall:
			summoningPortalEfect.FadeOutParticles(
			()=>
			{
				summoningPortalEfect.gameObject.SetActive(false);
			});
			break;

			case States.AttractingBall:
			if(spherePicker != null) spherePicker.EndBehavior();
			spherePicker = new Behavior(this, AttractSphere(
			()=>
			{
				state = States.ReturningBall;
			}));
			break;

			case States.ReturningBall:
			break;

			case States.DroppingBall:
			CoroutineController.Instance.Rotate(this, shipWaypoints.gameObject.transform.rotation, dealyBeforeDroppingSphere, 
			()=>
			{
				if(onBallDropped != null) onBallDropped();
				BallModel.Instance.state = BallModel.States.OnGame;
				state = States.Leaving;
			});
			break;

			case States.Leaving:
			summoningPortalEfect.gameObject.SetActive(true);
			summoningPortalEfect.gameObject.transform.position = transform.position;
			summoningPortalEfect.FadeInParticles(
			()=>
			{
				unsummonShip = new Behavior(this, UnsummonShip(
					()=>
					{
						summoningPortalEfect.FadeOutParticles(
							()=>
							{
								LocalGoal.Instance.goalScored = false;
								VisitorGoal.Instance.goalScored = false;
								summoningPortalEfect.gameObject.SetActive(false);
								state = States.Unassigned;
							});	
					}));
			});
			break;
	
			default:
			break;
		}
	}
	
	/// <summary>Leaves SalvageShip.States State.</summary>
	/// <param name="_state">SalvageShip.States State that will be left.</param>
	public void ExitState(SalvageShip.States _state)
	{
		switch(_state)
		{
			case States.Unassigned:
			break;

			case States.Summoned:
			if(summonShip != null) summonShip.EndBehavior();
			summonShip = null;
			break;

			case States.ChasingBall:
			break;

			case States.AttractingBall:
			break;

			case States.ReturningBall:
			break;

			case States.DroppingBall:
			break;

			case States.Leaving:
			break;
	
			default:
			break;
		}
	}
#endregion

#region UnityMethods:
	void OnEnable()
	{
		state = States.Summoned;
	}

	void OnDisable()
	{

	}

	/// <summary>SalvageShip's' instance initialization.</summary>
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
		else
		{
			state = States.Unassigned;

			Ball.onBallLeft += (bool _left)=>
			{
				if(_left) gameObject.SetActive(true);
			};
			BaseGoal.onGoalScored += (BaseGoal _goal)=>
			{
				LocalGoal.Instance.goalScored = true;
				VisitorGoal.Instance.goalScored = true;
				gameObject.SetActive(true);
			};

			distanceToGetClose *= distanceToGetClose; /// Elevate to the power of 2 to compare square magnitude.
			distanceToGetCloseToWaypoint *= distanceToGetCloseToWaypoint;
		}
	}

	/// <summary>SalvageShip's starting actions before 1st Update frame.</summary>
	void Start ()
	{
		
	}
	
	/// <summary>SalvageShip's tick at each frame.</summary>
	void Update ()
	{
		CheckState();
	}

	void OnDestroy()
	{
		Ball.onBallLeft -= (bool _left)=>
		{
			gameObject.SetActive(_left);
		};
		BaseGoal.onGoalScored += (BaseGoal _goal)=>
		{
			gameObject.SetActive(true);
		};
	}
#endregion

#region PublicMethods:
	
#endregion

#region PrivateMethods:
	private void CheckState()
	{
		switch(_state)
		{
			case States.ChasingBall:
			Vector3 direction = transform.position.GetDirectionTowards(Ball.Instance.transform.position);
			if(direction.sqrMagnitude > distanceToGetClose)
			{
				transform.rotation = Quaternion.LookRotation(direction);
				rigidbody.velocity += transform.TransformDirection(new Vector3(0f, 0f, maxSpeed * Time.deltaTime));
			}
			else
			{
				rigidbody.velocity = Vector3.zero;
				state = States.AttractingBall;
			}
			break;

			case States.ReturningBall:
			Vector3 waypointDirection = transform.position.GetDirectionTowards(shipWaypoints.transform.position);
			if(waypointDirection.sqrMagnitude > distanceToGetCloseToWaypoint)
			{
				transform.rotation = Quaternion.LookRotation(waypointDirection);
				rigidbody.velocity += transform.forward * maxSpeed;
			}
			else
			{
				rigidbody.velocity = Vector3.zero;
				state = States.DroppingBall;
			}
			break;

			default:
			break;
		}
	}

	private Vector3 GetNewSpawnPosition()
	{
		Vector3 newSpawnPosition = BallModel.Instance.gameObject.transform.position.GetDirectionTowards(shipWaypoints.gameObject.transform.position);
		return (newSpawnPosition.normalized * spawningProjectionOffset);
	}

	private IEnumerator SummonShip(Action onSummonEnds)
	{
		float normalizedTime = 0.0f;

		while(normalizedTime < 1.0f)
		{
			transform.localScale = VoidlessVector3.RegularVector3(normalizedTime);
			normalizedTime += (Time.deltaTime / shipEntranceDuration);
			yield return null;
		}

		transform.localScale = VoidlessVector3.RegularVector3(1f);
		if(onSummonEnds != null) onSummonEnds();
	}

	private IEnumerator UnsummonShip(Action onUnsummonEnds)
	{
		float normalizedTime = 1.0f;

		while(normalizedTime > 0.0f)
		{
			transform.localScale = VoidlessVector3.RegularVector3(normalizedTime);
			normalizedTime -= (Time.deltaTime / shipExitDuration);
			yield return null;
		}

		transform.localScale = Vector3.zero;
		if(onUnsummonEnds != null) onUnsummonEnds();
	}

	private IEnumerator AttractSphere(Action onSphereAttracted)
	{
		BallModel.Instance.state = BallModel.States.Grabbed;
		float normalizedTime = 0.0f;
		Vector3 originalBallPosition = Ball.Instance.transform.position;

		while(normalizedTime < 1.0f)
		{
			BallModel.Instance.transform.position = Vector3.Slerp(originalBallPosition, shipHoldingCenter.position, normalizedTime);
			normalizedTime += (Time.deltaTime / attractionSpeed);
			yield return null;
		}

		BallModel.Instance.transform.parent = transform;	

		if(onSphereAttracted != null) onSphereAttracted();
	}
#endregion
}
}