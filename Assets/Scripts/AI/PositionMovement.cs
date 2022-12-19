using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class PositionMovement : MonoBehaviour
{
	/*public GameObject ball;
	public Transform referencePlane;
	float planeDistance;
	public float dif = 0f;
	private PlayersStats playersStatsScrypts;
	private AIStateMachine aism; //To get both states and status...
	private bool goToGoalBool;
	private float normalizedScale;
	private bool onTargetRange; //Flag activated when the player is near the ball so it can hit it, and also stops it so it doesn't go literally where the ball is...
	private GameObject target;
	private Transform goal;
	private AIBehavior aib;
	private float distance;
	private bool playerExclusivity;
	private Transform referencePlayerPlaneName; //Stores the reference player plane, so it avoids re-checking...
	Vector3[] identities;
	private EnemyTeamManager etm;
	private PlayerTeamManager ptm;
	//Participants geters...
	private List<GameObject> team; //Ally team...
	private List<GameObject> enemyTeam; //Rival team...
	private GameObject game;

	//Reference Plane Checkers Handlers...
	string lastLevel;
	string actualLevel;


	////Testing the Turning /////
	private Quaternion _lookRotation;
    private Vector3 _direction;
    private float RotationSpeed = 3f;

    void OnEnable()
    {
    	game = GameObject.Find("Game");
    	this.ball = game.GetComponent<GameMechanics>().getBall();
    }

	void Start()
	{
		playerExclusivity = false;
		aib = this.GetComponent<AIBehavior>();
		aism = this.GetComponent<AIStateMachine>();
		ball = aism.getGame().GetComponent<GameMechanics>().getBall();
		normalizedScale = 1f;
		playersStatsScrypts = GetComponent<PlayersStats>();

		target = ball;

		referencePlayerPlaneName = null;

		identities = new Vector3[]
		{
			new Vector3(1, 0, 1),
			new Vector3(-1, 0, 1),
			new Vector3(-1, 0, -1),
			new Vector3(1, 0, -1)
		};

		referencePlane = null;
		planeDistance = 5f;

		game = aism.getGame();

		if(playersStatsScrypts.getIsEnemy())
		{
			etm = game.GetComponent<EnemyTeamManager>();
			ptm = game.GetComponent<PlayerTeamManager>();
			team = etm.getEnemyTeam();
		}
		else
		{
			ptm = game.GetComponent<PlayerTeamManager>();
			etm = game.GetComponent<EnemyTeamManager>();
			team = ptm.getPlayerTeam();
		}

	}

     float getPlaneDistance()
     {
     	float d =  new Vector2((this.transform.position.x - referencePlane.transform.position.x), this.transform.position.z - referencePlane.transform.position.z).magnitude;

		return d;
     }


	//Makes the player heads to the direction the ball is...
	public float headOnTarget(GameObject _target)
	{
		Vector3 playerPos = this.transform.position;
		Vector3 targetPos = _target.transform.position;

		float angle;
		float x = (playerPos.x - targetPos.x);
		float z = (playerPos.z - targetPos.z);

		if(playerPos.z < targetPos.z || playerPos.x < targetPos.x && playerPos.z < targetPos.z)
		{
			angle = (Mathf.Atan2(z, x) + (Mathf.PI * 2)) * Mathf.Rad2Deg;
		}
		else
		{
			angle = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
		}

		return angle;
	}

	public void checkLevelForState(string _level, string _state)
	{
		if(_level != null && _state != null)
		if(_state == "Idle")
		{
			switch(_level)
			{
				case "One":
				//Avoid multiple value setups...
				if(target != ball) setTarget(ball);
				if(referencePlane != null) referencePlane = null;
				if(!playerExclusivity)
				{
					if(getDistance() >= 3.0f)
					{
						determineBestTargetToHit();
						aimFromBallToTarget();
					}
					else if(getDistance() >= 0.5f && getDistance() < 3.0f)
					{
						goToTarget(target);
					}
					
				}
				break;

				case "Two":

				break;

				case "Three":

				break;
			}
		}

		else if(_state == "Defending")
		{
			switch(_level)
			{
				case "One":
				//Avoid multiple value setups...
				//if(target != ball) setTarget(ball);
				if(referencePlane != null) referencePlane = null;
				if(!playerExclusivity)
				{
					if(getDistance() >= 3.0f)
					{
						

						determineBestTargetToHit();
						aimFromBallToTarget();
					}
					else if(getDistance() >= 0.5f && getDistance() < 3.0f)
					goToTarget(target);
				}
				break;

				case "Two":
				if(referencePlane == null)
				{
					//Debug.Log("Dont have referencePlane");
						if(!playersStatsScrypts.getIsEnemy())
						{
							//if(referencePlayerPlaneName == null)
							{
								//Debug.Log("Proceed to establish referencePlane");
								referencePlayerPlaneName = ptm.determinePlanePlayerIs(ptm.getPlayerWithLevelIndex("One")); //Pass the level 1 player...
								//setReferencePlane(ptm.getReferencePlane(referencePlayerPlaneName));
								setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));
							}
								

							//if(referencePlayerPlaneName != null)
								
						}
						else
						{
							referencePlayerPlaneName = etm.determinePlanePlayerIs(etm.getPlayerWithLevelIndex("One")); //Pass the level 1 player...
							//setReferencePlane(ptm.getReferencePlane(referencePlayerPlaneName));
							setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));

						}				
				}

				else if(referencePlane != null)
				{
					goBetweenPlaneAndTarget();
				}
				break;

				case "Three":

				if(referencePlane == null)
				{
					//Debug.Log("Dont have referencePlane");
						if(!playersStatsScrypts.getIsEnemy())
						{
							//if(referencePlayerPlaneName == null)
							{
								//Debug.Log("Proceed to establish referencePlane");
								referencePlayerPlaneName = ptm.determinePlanePlayerIs(ptm.getPlayerWithLevelIndex("Two")); //Pass the level 1 player...
								//setReferencePlane(ptm.getReferencePlane(referencePlayerPlaneName));
								setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));
							}
								

							//if(referencePlayerPlaneName != null)
								
						}

						else
						{
							referencePlayerPlaneName = etm.determinePlanePlayerIs(etm.getPlayerWithLevelIndex("Two")); //Pass the level 1 player...
							//setReferencePlane(ptm.getReferencePlane(referencePlayerPlaneName));
							setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));
						}					
				}

				else if(referencePlane != null)
				{
					goBetweenPlaneAndTarget();
					
				}
				break;
			}
		}

		else if(_state == "Atacking")
		{
			switch(_level)
			{
				case "One":
				//Avoid multiple value setups...
				//if(target != ball) setTarget(ball);
				if(referencePlane != null) referencePlane = null;
				if(!playerExclusivity)
				{
					if(getDistance() >= 3.0f)
					{
						

						determineBestTargetToHit();
						aimFromBallToTarget();
					}
					else if(getDistance() >= 0.5f && getDistance() < 3.0f)
					{
						goToTarget(target);
					}
					
				}
				break;

				case "Two":

				//Plane Assignation
				if(referencePlane == null)
				{
					//Debug.Log("Dont have referencePlane");
						if(!playersStatsScrypts.getIsEnemy())
						{
							//if(referencePlayerPlaneName == null)
							{
								//Debug.Log("Proceed to establish referencePlane");
								referencePlayerPlaneName = ptm.determinePlanePlayerIs(ptm.getPlayerWithLevelIndex("One")); //Pass the level 1 player...
								setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));
							}
								

							//if(referencePlayerPlaneName != null)
								
						}

						else
						{
							referencePlayerPlaneName = etm.determinePlanePlayerIs(etm.getPlayerWithLevelIndex("One")); //Pass the level 1 player...
							//setReferencePlane(ptm.getReferencePlane(referencePlayerPlaneName));
							setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));
						}				
				}

				else if(referencePlane != null)
				{
					goBetweenPlaneAndTarget();
					
				}
					
				break;

				case "Three":
				if(referencePlane == null)
				{
					//Debug.Log("Dont have referencePlane");
						if(!playersStatsScrypts.getIsEnemy())
						{
							//if(referencePlayerPlaneName == null)
							{
								//Debug.Log("Proceed to establish referencePlane");
								referencePlayerPlaneName = ptm.determinePlanePlayerIs(ptm.getPlayerWithLevelIndex("Two")); //Pass the level 1 player...
								//setReferencePlane(ptm.getReferencePlane(referencePlayerPlaneName));
								setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));
							}
								

							//if(referencePlayerPlaneName != null)
								
						}

						else
						{
							referencePlayerPlaneName = etm.determinePlanePlayerIs(etm.getPlayerWithLevelIndex("Two")); //Pass the level 1 player...
							//setReferencePlane(ptm.getReferencePlane(referencePlayerPlaneName));
							setReferencePlane(getBestPlaneToPosition(referencePlayerPlaneName.name));
						}				
				}

				else if(referencePlane != null)
				{
					goBetweenPlaneAndTarget();
					
				}
				break;
			}
		}
	}

	public Transform getBestPlaneToPosition(string _playerReferencePlane)
	{
		Transform tempPlane = null;

		//Debug.Log("Player Reference Plane: " +_playerReferencePlane);

		if(aism.getCurrentState() == "Atacking")
		{
			switch(_playerReferencePlane)
			{
				case "Plane_A":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("B");
				}
				else
				{
					tempPlane = etm.getPlane("D");
				}
				break;

				case "Plane_B":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("A");
				}
				else
				{
					tempPlane = etm.getPlane("C");
				}
				break;

				case "Plane_C":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("A");
				}
				else
				{
					tempPlane = etm.getPlane("B");
				}
				break;

				case "Plane_D":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("B");
				}
				else
				{
					tempPlane = etm.getPlane("A");
				}
				break;

				default:
				Debug.LogError("Plane name " +_playerReferencePlane+ " not within the plane names...");
				break;
			}
		}

		else if(aism.getCurrentState() == "Defending")
		{
			switch(_playerReferencePlane)
			{
				case "Plane_A":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("C");
				}
				else
				{
					tempPlane = etm.getPlane("B");
				}
				break;

				case "Plane_B":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("C");
				}
				else
				{
					tempPlane = etm.getPlane("B");
				}
				break;

				case "Plane_C":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("B");
				}
				else
				{
					tempPlane = etm.getPlane("C");
				}
				break;

				case "Plane_D":
				if(!playersStatsScrypts.getIsEnemy())
				{
					tempPlane = ptm.getPlane("B");
				}
				else
				{
					tempPlane = etm.getPlane("D");
				}
				break;

				default:
				Debug.LogError("Plane name " +_playerReferencePlane+ " not within the plane names...");
				break;
			}
		}
		

		return tempPlane;
	}

	//Follows the behavior tree that determines if a player should target the ball to hit goal, or a player so it passes it the ball...
	public void determineBestTargetToHit()
	{
		Transform tempPlane = null;

		if(!playersStatsScrypts.getIsEnemy()) 
		tempPlane = etm.determinePlanePlayerIs(ptm.getPlayerWithLevelIndex(aism.getCurrentLevel())); //Pass the level of the player...
		else
		tempPlane = ptm.determinePlanePlayerIs(etm.getPlayerWithLevelIndex(aism.getCurrentLevel())); //Pass the level of the player...

		if(tempPlane != null)
		switch(tempPlane.name)
		{
				case "Plane_A":
				if(!playersStatsScrypts.getIsEnemy())
				{
					if(ptm.countPlayersOnPlane("A") >= 2)
					{
						Transform referencePlayer = ptm.getPlayerTeamMember(ptm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);
						//Debug.Log("Have more than 2 players on " +tempPlane.name+ ". Changed the target for a player");

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				else
				{
					if(etm.countPlayersOnPlane("C") >= 2)
					{
						Transform referencePlayer = etm.getEnemyTeamMemeber(etm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);
						//Debug.Log("Have more than 2 players on " +tempPlane.name+ ". Changed the target for a player");

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				break;

				case "Plane_B":
				if(!playersStatsScrypts.getIsEnemy())
				{
					if(ptm.countPlayersOnPlane("A") >= 2)
					{
						Transform referencePlayer = ptm.getPlayerTeamMember(ptm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				else
				{
					if(etm.countPlayersOnPlane("D") >= 2)
					{
						Transform referencePlayer = etm.getEnemyTeamMemeber(etm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);
						//Debug.Log("Have more than 2 players on " +tempPlane.name+ ". Changed the target for a player");

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				break;

				case "Plane_C":
				if(!playersStatsScrypts.getIsEnemy())
				{
					if(ptm.countPlayersOnPlane("A") >= 2)
					{
						Transform referencePlayer = ptm.getPlayerTeamMember(ptm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);
						//Debug.Log("Have more than 2 players on " +tempPlane.name+ ". Changed the target for a player");

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				else
				{
					if(etm.countPlayersOnPlane("D") >= 2)
					{
						Transform referencePlayer = etm.getEnemyTeamMemeber(etm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);
						//Debug.Log("Have more than 2 players on " +tempPlane.name+ ". Changed the target for a player");

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				break;

				case "Plane_D":
				if(!playersStatsScrypts.getIsEnemy())
				{
					if(ptm.countPlayersOnPlane("B") >= 2)
					{
						Transform referencePlayer = ptm.getPlayerTeamMember(ptm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);
						//Debug.Log("Have more than 2 players on " +tempPlane.name+ ". Changed the target for a player");

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				else
				{
					if(etm.countPlayersOnPlane("D") >= 2)
					{
						Transform referencePlayer = etm.getEnemyTeamMemeber(etm.getPlayerWithLevelIndex("Two")).transform;
						setGoal(referencePlayer);
						//Debug.Log("Have more than 2 players on " +tempPlane.name+ ". Changed the target for a player");

						playersStatsScrypts.setShooting(false);
					}
					else
					{
						setGoal(ball.transform);

						playersStatsScrypts.setShooting(true);
					}
				}
				break;

				default:
				Debug.LogError("Plane name " +tempPlane.name+ " not within the plane names...");
				break;
		}
	}
	
	void Update ()
	{

        //transform.LookAt(target.transform);


        //find the vector pointing from our position to the target
        _direction = (target.transform.position - transform.position).normalized;
        //_direction.x = 0f;
        //_direction.z = 0f;
        _direction.y = 0f;
 
         //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);
 
         //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

        //transform.eulerAngles = new Vector3(0f, headOnTarget(target), 0f);

        if(aism.getCurrentState() != "Unasigned" && aism.getCurrentLevel() != "") //Unasigned state avoids the players to move, used when the session restarts...
        {
	        checkLevelForState(aism.getCurrentLevel(), aism.getCurrentState());
	        //StartCoroutine(checkLevels());
        }

        if (this.GetComponent<Rigidbody>().velocity.y >= 10)
        {
        	this.GetComponent<Rigidbody>().velocity = new Vector3(0,-10,0);
        }
        //lastLevel = this.aism.getCurrentLevel();		
	}

	//NO!
	IEnumerator checkLevels()
	{
		yield return new WaitForSeconds(0.1f);
		checkLevelForState(aism.getCurrentLevel(), aism.getCurrentState());
	}

	///// Coroutines /////

	void goToTarget(GameObject _target)
	{
		//aism.setStatus("Doing");

		//if(aism.getCurrentStatus() != "Done")
		{
			if(playersStatsScrypts.getSpeedIA() >= playersStatsScrypts.getMaxSpeed())
			{
				playersStatsScrypts.setSpeedIA(playersStatsScrypts.getMaxSpeedIA());
			}
			playersStatsScrypts.accelerateIA(2f);
			Vector3 destination = new Vector3(_target.transform.position.x, this.transform.position.y, _target.transform.position.z);

			transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * playersStatsScrypts.getSpeedIA() * aism.getNormalizedScale());
		}

		//yield return null;	
	}

	void aimFromBallToTarget()
	{
		if(playersStatsScrypts.getSpeedIA() >= playersStatsScrypts.getMaxSpeed())
			{
				playersStatsScrypts.setSpeedIA(playersStatsScrypts.getMaxSpeedIA());
			}
			playersStatsScrypts.accelerateIA(1f); 
			Vector3 destination = getPositionToHitTowardsTarget(ball.transform, goal);

			transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * playersStatsScrypts.getSpeedIA() * aism.getNormalizedScale());
	}

	void goToPlane(Transform _target)
	{
		if(playersStatsScrypts.getSpeedIA() >= playersStatsScrypts.getMaxSpeed())
			{
				playersStatsScrypts.setSpeedIA(playersStatsScrypts.getMaxSpeedIA());
			}
			playersStatsScrypts.accelerateIA(2f);
			Vector3 destination = new Vector3(_target.position.x, this.transform.position.y, _target.position.z);

			transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * playersStatsScrypts.getSpeedIA() * aism.getNormalizedScale());
	}

	//It goes to the medium point between the target and the reference plane points...
	void goBetweenPlaneAndTarget()
	{
		float tempX = ((referencePlane.position.x + target.transform.position.x) / 2) * aism.getNormalizedScale();
		float tempZ = ((referencePlane.position.z + target.transform.position.z) / 2) * aism.getNormalizedScale();

		if(playersStatsScrypts.getSpeedIA() >= playersStatsScrypts.getMaxSpeed())
			{
				playersStatsScrypts.setSpeedIA(playersStatsScrypts.getMaxSpeedIA());
			}
			playersStatsScrypts.accelerateIA(2f);

		Vector3 destination = new Vector3(tempX, this.transform.position.y, tempZ);

		transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * playersStatsScrypts.getSpeedIA() * aism.getNormalizedScale());
	}

	void moveRelativeToPlane(Vector3 _pos)
	{
		if(playersStatsScrypts.getSpeedIA() >= playersStatsScrypts.getMaxSpeed())
			{
				playersStatsScrypts.setSpeedIA(playersStatsScrypts.getMaxSpeedIA());
			}
			playersStatsScrypts.accelerateIA(2f);
			//Vector3 destination = new Vector3(_pos.position.x, this.transform.position.y, _pos.position.z);

			transform.position = Vector3.MoveTowards(transform.position, _pos, Time.deltaTime * playersStatsScrypts.getSpeedIA() * aism.getNormalizedScale());
	}

	IEnumerator waitAndDetermine()
	{
		yield return new WaitForSeconds(0.45f);
		Debug.Log("Status is: " + aism.getCurrentStatus());

		StartCoroutine(waitAndDetermine());
		//calculateBallFarness();
	}

	///// Coroutines /////



	///// Geters and Seters /////

	public void setPlayerExclusivity(bool _playerExclusivity)
	{
		this.playerExclusivity = _playerExclusivity;
	}

	public void setGoal(Transform _goal)
	{
		this.goal = _goal;
	}

	public void setOnTargetRange(bool _onTargetRange)
	{
		this.onTargetRange = _onTargetRange;
	}

	public void setTarget(GameObject _target)
	{
		this.target = _target;
	}

	//Returns distance between the ball and the player...
	public float getDistance()
	{
		float d =  new Vector2((this.transform.position.x - target.transform.position.x), this.transform.position.z - target.transform.position.z).magnitude;

		return d;
	}

	public bool getPlayerExclusivity()
	{
		return this.playerExclusivity;
	}

	public GameObject getBall()
	{
		return this.ball;
	}

	public void setNormalizedScale(float _normalizedScale)
	{
		this.normalizedScale = _normalizedScale;
	}

	public void setReferencePlane(Transform _referencePlane)
	{
		this.referencePlane = _referencePlane;
	}

	///// Geters and Seters /////

	///// Hit to the Goal /////

	public float angleBetweenTarget(Transform _from, Transform _target)
    {
    	float angle = 0.0f;

    	Vector3 thisPos = _from.transform.position;
        Vector3 targetPos = _target.transform.position;
        float x = (thisPos.x - targetPos.x);
        float z = (thisPos.z - targetPos.z);

        if(thisPos.z < targetPos.z || thisPos.x < targetPos.x && thisPos.z < targetPos.z)
        {
            angle = (Mathf.Atan2(z, x) + (Mathf.PI * 2)) * Mathf.Rad2Deg;
        }
        else
        {
            angle = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
        }

        return angle;
    }

    float determineDirMod()
    {
    	float dirMod = -1f;
    	Transform tempPlane = null;

    	if(!playersStatsScrypts.getIsEnemy())
    	{
    		tempPlane = ptm.determinePlanePlayerIs(ptm.getPlayerWithLevelIndex(aism.getCurrentLevel()));
    		if(tempPlane != null)
    		if(tempPlane == ptm.getPlane("C") || tempPlane == ptm.getPlane("D"))
    		{
    			dirMod = 1f;
    		}
    		else return dirMod;

    	}
    	else if(playersStatsScrypts.getIsEnemy())
    	{
    		tempPlane = etm.determinePlanePlayerIs(etm.getPlayerWithLevelIndex(aism.getCurrentLevel()));
    		if(tempPlane != null)
    		if(tempPlane == etm.getPlane("B") || tempPlane == etm.getPlane("A"))
    		{
    			dirMod = 1f;
    		}
    		else return dirMod;
    	}

    	//Debug.Log("Temp Plane name:"+tempPlane.name);

    	return dirMod;
    }

    public Vector3 getPositionToHitTowardsTarget(Transform _from, Transform _target)
    {
    	float dirMod = determineDirMod();
    	//float dirMod = -1f;

    	Vector3 ballPos = new Vector3(_from.transform.position.x, this.transform.position.y, _from.transform.position.z);
    	float angle = angleBetweenTarget(_from, _target);

    	float tempX;
    	float tempZ;
    	Vector3 positionToHit = new Vector3(ballPos.x, ballPos.y, ballPos.z);

    	for(int i = 0; i < identities.Length; i++)
    	{
    		float minimRange = i * 90f;

    		if(angle >= minimRange && angle <= (minimRange + 90f)) //Checks the quadrant the angle belongs to...
    		{
    			//float quadrantVector = (identities[i].x * identities[i].z); 

    			tempX = Mathf.Abs(Mathf.Cos(angle)) * identities[i].x * dirMod;//To the contrary quadrant
    			tempZ = Mathf.Abs(Mathf.Sin(angle)) * identities[i].z * dirMod;//To the contrary quadrant

    			positionToHit = new Vector3((positionToHit.x - (tempX * 2f)), positionToHit.y, (positionToHit.z - (tempZ * 2f)));

    			return positionToHit;
    		}
    	}

    	return positionToHit;
    }*/

	///// Hit to the Goal /////
}