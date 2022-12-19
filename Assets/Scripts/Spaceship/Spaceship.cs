using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;
using DashPokTaGameData;

namespace DashPokTa
{
public class Spaceship : Singleton<Spaceship>, IFiniteStateMachine<Spaceship.States>
{
	[SerializeField] [Range(1.0f, 25.0f)] private float minSpaceshipSpeed; 		/// <summary>Spaceship's traveling speed.</summary>
	[SerializeField] [Range(3.0f, 50.0f)] private float maxSpaceshipSpeed;		/// <summary>Maximum Spaceship's traveling speed.</summary>
	[SerializeField] [Range(0.1f, 5.0f)] private float reactionTime;	 		/// <summary>Spaceship's reaction time to update ball's position.</summary>
	[SerializeField] [Range(0.1f, 1.0f)] private float distanceToGetClose; 		/// <summary>Minimum Distance from Spaceship to ball to be considered "Close".</summary>
	[SerializeField] [Range(5.0f, 20.0f)] private float atractionFieldForce; 	/// <summary>Spaceship's ray atraction field force.</summary>
	[SerializeField] [Range(1.5f, 5f)] private float timeBeforeDropBall; 		/// <summary>Time that the Spaceship waits before dropping the Ball [on Unassigned State].</summary>
	[SerializeField] [Range(5f, 10f)] private float attractionFieldLength; 		/// <summary>Spaceship's Attraction Field Length.</summary>
	[SerializeField] [Range(1f, 5f)] private float attractionFieldRadius; 		/// <summary>Spaceship's Attraction Field Radius.</summary>
	[SerializeField] private float minHeightFromBall; 							/// <summary>Minimum distance on Y that the Spaceship will have from the ball [Not used on Roaming].</summary>
	[SerializeField] private Transform ballHoldingCenter; 						/// <summary>Position where the ball will be held while it is on the Spaceship.</summary>
	[SerializeField] private GameObject spaceshipRay; 							/// <summary>Spaceship's atraction field ray.</summary>
	[SerializeField] private Transform spaceshipHoldingCenter; 					/// <summary>Spaceship's Holding Center.</summary>
	[SerializeField] private WaypointController spaceshipWaypointController;	/// <summary>Spaceship's waypoints.</summary>
	[SerializeField] private States initialState;								/// <summary>Initial State. Setted on Editor.</summary>
	private States _state;														/// <summary>Current State.</summary>
	private Rigidbody _rigidbody; 												/// <summary>Rigidbody's Component</summary>	
	private BallModel _ball;											 		/// <summary>Game session's ball.</summary>
	private Vector3 moveDirection; 												/// <summary>Gets the tracked direction the spaceship has to move.</summary>
	private bool ballLeftScenario;												/// <summary>Is ball on scenario Flag.</summary>
	private bool ballGrabbed; 													/// <summary>Flag Activated when the spaceship's ray caughts the ball.</summary>
	private Behavior _unassignedSensor; 										/// <summary>Sensor for Unassigned State.</summary>
	private Behavior _unassignedTask; 											/// <summary>Task for Unassigned State.</summary>
	private Behavior _roamingSensor; 											/// <summary>Sensor for Roaming State.</summary>
	private Behavior _roamingTask; 												/// <summary>Task for Roaming State.</summary>
	private Behavior _followBallSensor;											/// <summary>Sensor for FollowBall State.</summary>
	private Behavior _followBallTask; 											/// <summary>Task for FollowBall State.</summary>
	private Behavior _returnBallSensor;											/// <summary>Sensor for ReturnBall State.</summary>
	private Behavior _returnBallTask; 											/// <summary>Task for ReturnBall State.</summary>	

	public enum States 															/// <summary>States enumerator.</summary>
	{
		Unassigned, 															/// <summary>Unassigned State.</summary>
		DropBall, 																/// <summary>Drop Ball State.</summary>
		Roaming, 																/// <summary>Roaming State. Here the Spaceship roams [on a moderated speed] on the XZ Vectorial Space Ball's Position.</summary>
		FollowBall, 															/// <summary>Follow State. Here the Spaceship goes directly towards the ball [At maximum speed] to capture it.</summary>
		ReturnBall 																/// <summary>Return Ball State. Here the Spaceship goes [Having the Ball captured] to the deposit waypoint, to resume the game.</summary>
	}; 


#region Getters/Setters:
	/// <summary>Gets and Sets state property.</summary>
	public States state
	{
		get { return _state; }
		set
		{
			ExitState(_state); //First it exits the current State.
			EnterState(_state = value); //Then enters the newly assigned State.
		}
	}

	/// <summary>Gets rigidbody property.<summary>
	public Rigidbody rigidbody
	{
		get
		{
			if(_rigidbody == null)
			{
				if(GetComponent<Rigidbody>() != null)
				_rigidbody = GetComponent<Rigidbody>();
			}
			
			return _rigidbody;
		}
	}

	/// <summary>Gets and Sets ball property.<summary>
	public BallModel ball
	{
		get { return _ball; }
		set { _ball = value; }
	}

	/// <summary>Gets and Sets unassignedSensor property.</summary>
	public Behavior unassignedSensor
	{
		get { return _unassignedSensor; }
		set { _unassignedSensor = value; }
	}

	/// <summary>Gets and Sets unassignedTask property.</summary>
	public Behavior unassignedTask
	{
		get { return _unassignedTask; }
		set { _unassignedTask = value; }
	}

	/// <summary>Gets and Sets roamingSensor property.</summary>
	public Behavior roamingSensor
	{
		get { return _roamingSensor; }
		set { _roamingSensor = value; }
	}

	/// <summary>Gets and Sets roamingTask property.</summary>
	public Behavior roamingTask
	{
		get { return _roamingTask; }
		set { _roamingTask = value; }
	}

	/// <summary>Gets and Sets followBallSensor property.</summary>
	public Behavior followBallSensor
	{
		get { return _followBallSensor; }
		set { _followBallSensor = value; }
	}

	/// <summary>Gets and Sets followBallTask property.</summary>
	public Behavior followBallTask
	{
		get { return _followBallTask; }
		set { _followBallTask = value; }
	}

	/// <summary>Gets and Sets returnBallSensor property.</summary>
	public Behavior returnBallSensor
	{
		get { return _returnBallSensor; }
		set { _returnBallSensor = value; }
	}

	/// <summary>Gets and Sets returnBallTask property.</summary>
	public Behavior returnBallTask
	{
		get { return _returnBallTask; }
		set { _returnBallTask = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>OnBallLeft and OnGoalScored Events Subscriptions.</summary>
	void OnEnable()
	{
		Ball.onBallLeft += (bool _flag)=> { ballLeftScenario = _flag; };
		BaseGoal.onGoalScored +=  (BaseGoal _goal)=> { state = States.ReturnBall; };
	}

	/// <summary>OnBallLeft and OnGoalScored Events Unsubscriptions.</summary>
	void OnDisable()
	{
		Ball.onBallLeft -= (bool _flag)=> { ballLeftScenario = _flag; };
		BaseGoal.onGoalScored -= (BaseGoal _goal)=> { state = States.ReturnBall; };
	}

	/// <summary>Instance proceedure.</summary>
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
	}

	/// <summary>Dependient variables initialization.</summary>
	void Start()
	{
		moveDirection = Vector3.zero;
		distanceToGetClose = Mathf.Pow(distanceToGetClose, 2);
		ball = Ball.Instance;
		state = initialState;
		spaceshipRay.SetActive(false);
	}

	/// <summary>Checks current State at each frame.</summary>
	void Update()
	{
		CheckState();
	}

	/// <summary>Draws Spherecast visualization.</summary>
	/*void OnDrawGizmosSelected()
	{
		//int spheresOnGizmoLine = ((int)raycastLength / (int)(/*Mathf.Floor*[DIAGONAL](sphereColliderRadius) * 2f)); //Define the number of spheres that will be drawn relative to the diameter of the spherecast and teh raycast length.

		for(int j = 1 ; j < (spheresOnGizmoLine + 1); j++)
		{
			float normalizedPosition = ((1.0f * j) / (1.0f * spheresOnGizmoLine)); //Parse the variables to float by multiplying by a normalized float...

			for(int i = 0; i < identities.Length; i++)
			{
				Vector3 normalizedProjection = (transform.position + transform.TransformDirection( identities[i] * (raycastLength * normalizedPosition) ));
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(normalizedProjection, sphereColliderRadius);

				if(j == spheresOnGizmoLine) //Just paint the line one time...
				{
					Vector3 projection = (transform.position + transform.TransformDirection( identities[i] * raycastLength ));
					Gizmos.color = Color.blue;
					Gizmos.DrawLine(transform.position, projection);
				}
			}
		}
	}*/
#endregion

#region FiniteStateMachine:
	/// <summary>Enters State.</summary>
	/// <param name="_state">The State that will be entered.</param>
	public void EnterState(Spaceship.States _state)
	{
		switch(state)
		{
			case States.Unassigned:
			unassignedSensor = new Behavior(this, WaitBeforeDropBall(
				()=> //Wait n seconds before dropping Ball.
				{
					unassignedTask = new Behavior(this, DropBall(
						()=> //Drop the Ball.
						{
							state = States.Roaming;
						}));
				}));
			break;

			case States.Roaming: //Roams around the ball.
			roamingSensor = new Behavior(this, TrackBall(
				()=> //Track Ball's Position.
				{
					roamingTask = new Behavior(this, GoToClosestWaypoint(
						()=>
						{
							state = States.FollowBall;
						}));
				}));
			break;

			case States.FollowBall: //Choses waypoint, then goes for ball.
			followBallSensor = new Behavior(this, GoToClosestWaypoint(
				()=> //Get Closest Waypoint.
				{	
					followBallTask = new Behavior(this, TrackBall(
					()=> //TrackBall and cast the Spaceship's Sight.
					{
						followBallTask = new Behavior(this, CaptureBall(
						()=> //Capture the Ball.
						{
							state = States.ReturnBall;
						}));
						
					}));			
				}));
			break;

			case States.ReturnBall:
			returnBallSensor = new Behavior(this, AtractBall(
				()=> //Attract the Ball towards the Spaceship.
				{
					returnBallTask = new Behavior(this, RepositionBall(
						()=> //Go to Central Zone Waypoint.
						{
							moveDirection = spaceshipWaypointController.transform.position;
							state = States.Unassigned;
						}));
				}));
			break;
		}

		Debug.Log("Entered State: " + _state.ToString());
	}

	/// <summary>Exits State. Ends Behaviors of respective State.</summary>
	/// <param name="_state">The State that will be left.</param>
	public void ExitState(Spaceship.States _state)
	{
		switch(state)
		{
			case States.Unassigned:
			if(_unassignedSensor != null) _unassignedSensor.EndBehavior();
			if(_unassignedTask != null) _unassignedTask.EndBehavior();
			break;

			case States.Roaming:
			if(_roamingSensor != null) _roamingSensor.EndBehavior();
			if(_roamingTask != null) _roamingTask.EndBehavior();
			break;

			case States.FollowBall:
			if(_followBallSensor != null) _followBallSensor.EndBehavior();
			if(_followBallTask != null) _followBallTask.EndBehavior();
			break;

			case States.ReturnBall:
			if(_returnBallSensor != null) _returnBallSensor.EndBehavior();
			if(_returnBallTask != null) _returnBallTask.EndBehavior();
			break;

			default:
			Debug.LogError("State " + state.ToString() + " not yet on switch checking. Update FiniteStateMachine class.");
			break;
		}

		Debug.Log("Left State: " + _state.ToString());
	}
#endregion

#region Methods:
	/// <summary>Checks current State, then executes that State's Actions.</summary>
	private void CheckState()
	{
		switch(state)
		{
			case States.Unassigned:
			//MoveAtMinimumSpeed();
			break;

			case States.Roaming:
			MoveAtMinimumSpeed();
			break;

			case States.FollowBall:
			MoveAtMaximumSpeed();
			break;

			case States.ReturnBall:
			MoveAtMaximumSpeed();
			break;

			default:
			break;
		}
	}


	/// <summary>Roams above Ball [following at moderate speed the XZ vectorial space position].</summary>
	private void MoveAtMinimumSpeed()
	{
		rigidbody.velocity = (moveDirection.sqrMagnitude < distanceToGetClose) ? Vector3.zero : (moveDirection.normalized * minSpaceshipSpeed);
	}

	/// <summary>Chases the Ball [following at maximum speed the XZ vectorial space position].</summary>
	private void MoveAtMaximumSpeed()
	{
		rigidbody.velocity = (moveDirection.sqrMagnitude < distanceToGetClose) ? Vector3.zero : (moveDirection.normalized * maxSpaceshipSpeed);
	}

	/// <summary>Cast Ray on transform.down direction, looking if it collides with the Ball.</summary>
	/// <returns>If the Ray casted collided with the Ball.</returns>
	private bool BallSighted()
	{
		Ray ray = new Ray(transform.position, (-transform.up * attractionFieldLength));
		RaycastHit[] hits = Physics.RaycastAll(ray);

		if(Physics.SphereCast(ray, attractionFieldRadius, attractionFieldLength))
		{
			foreach(RaycastHit hit in hits)
			{
				if(hit.transform.gameObject.tag == Keys.BALL_TAG)
				{
					return true;
				}
			}		
		}

		return false;
	}
#endregion

#region Coroutines:
	/// <summary>Updates each reactionTime.</summary>
	private IEnumerator UpdateEachReactionTime()
	{
		while(true)
		{
			yield return new WaitForSeconds(reactionTime);
		}
		
	}

	/// <summary>Waits timeBeforeDropBall seconds.</summary>
	/// <param name="onTimePassed">Anonymous action called when the time passed.</param>
	private IEnumerator WaitBeforeDropBall(Action onTimePassed)
	{
		yield return new WaitForSeconds(timeBeforeDropBall);
		if(onTimePassed != null) onTimePassed();
	}

	/// <summary>Drops the Ball into the Center Zone of the Playground.</summary>
	/// <param name="onBallDroped">Anonymous Action called when the Ball was dropped.</param>
	private IEnumerator DropBall(Action onBallDroped)
	{
		BallModel ballAtributes = ball.GetComponent<BallModel>();
		ballAtributes.state = BallModel.States.OnGame;
		ball.transform.parent = null;
		yield return null;

		if(onBallDroped != null) onBallDroped();
	}

	/// <summary>Tracks the Ball each reactionTime seconds and checks whether the ball left or not the stage, to transition the FollowBall State.</summary>
	/// <param name="onBallLeft">Anonymus action called when ballLeftScenario Flag is true.</param>
	private IEnumerator TrackBall(Action onBallLeft)
	{
		while(true)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForSeconds(reactionTime);
			moveDirection = transform.position.GetDirectionTowards(ball.transform.position).SetY(0f);
			if(ballLeftScenario && onBallLeft != null) onBallLeft();
		}		
	}

	/// <summary>Tracks the nearest Spaceship's waypoint and turns the atraction ray on.</summary>
	/// <param name="onReachedWaypoint">Anonymus action called when spaceship has arrived on nearest waypoint.</param>
	private IEnumerator GoToClosestWaypoint(Action onReachedWaypoint)
	{
		spaceshipRay.SetActive(true); //Turn on the spaceship's Ray.
		List<float> waypointsDistances = spaceshipWaypointController.waypointsPositions.GetSquareMagnitudesFromVectors(ball.transform.position);
		Vector3 nearestWaypoint = spaceshipWaypointController.waypointsPositions[waypointsDistances.IndexOf(Mathf.Min(waypointsDistances.ToArray()))];

		moveDirection = transform.position.GetDirectionTowards(nearestWaypoint);

		while(moveDirection.sqrMagnitude > distanceToGetClose)
		{
			yield return new WaitForEndOfFrame();
			moveDirection = transform.position.GetDirectionTowards(nearestWaypoint);
			yield return null;
		}

		if(onReachedWaypoint != null) onReachedWaypoint();
	}

	/// <summary>Goes where the ball is to capture it..</summary>
	/// <param name="onCaptured">Anonymus action called when spaceship has captured the ball.</param>
	private IEnumerator CaptureBall(Action onCaptured)
	{
		while(true)
		{
			yield return new WaitForEndOfFrame();
			moveDirection = transform.position.GetDirectionTowards(ball.transform.position.AddToY(minHeightFromBall));
			if(ballGrabbed && onCaptured != null)
			{
				ball.transform.parent = spaceshipRay.transform;
				onCaptured();
			}
			yield return null;
		}
	}

	/// <summary>Applies an atraction ball towards the spaceship while it is emparented.</summary>
	/// <param name="onBallAttracted">Anonymus action called when the Ball reached the Spaceship's Holding Center.</param>
	private IEnumerator AtractBall(Action onBallAttracted)
	{
		bool attract = true;
		Ball ballAtributes = ball.GetComponent<Ball>();
		ballAtributes.state = BallModel.States.Grabbed;

		while(attract)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForSeconds(reactionTime);
			ballAtributes.rigidBody.velocity = ball.transform.position.GetDirectionTowards(spaceshipHoldingCenter.position) * atractionFieldForce;

			if((spaceshipHoldingCenter.position.GetDirectionTowards(ball.transform.position).sqrMagnitude < distanceToGetClose) && onBallAttracted != null)
			{//If Ball is close to Holding Center.
				ball.transform.parent = spaceshipHoldingCenter;
				onBallAttracted();
				attract = false;
			}

			yield return null;
		}
	}

	/// <summary>Places Ball on default zone.</summary>
	/// <param name="onRepositioned">Anonymous action called when ballRepositionedScenario Flag is true.</param>
	private IEnumerator RepositionBall(Action onRepositioned)
	{
		float normalizedTime = 0.0f;
		Transform defaultZone = spaceshipWaypointController.transform;

		while(normalizedTime < 1.0f)
		{
			transform.position = Vector3.Lerp(transform.position, defaultZone.position, normalizedTime);

			normalizedTime += (Time.deltaTime / 5f);
			yield return null;
		}

		if(onRepositioned != null) onRepositioned();
	}
#endregion
	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;

		switch(obj.tag)
		{
			case Keys.BALL_TAG:
			ballGrabbed = true;
			break;

			default:
			break;
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	void OnTriggerExit(Collider col)
	{
		GameObject obj = col.gameObject;

		switch(obj.tag)
		{
			case Keys.BALL_TAG:
			ballGrabbed = false;
			break;

			default:
			break;
		}
	}
}
}