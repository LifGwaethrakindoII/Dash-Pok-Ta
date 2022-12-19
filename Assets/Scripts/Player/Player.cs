using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DashPokTaGameData;
using DashPokTa;
using VoidlessUtilities;

public class Player : PlayerModel
{
#region UnityMethods:
	/// <summary>Shows Hit Normals Gizmos. Draws groundChecker Ray</summary>
	void OnDrawGizmosSelected()
	{
		ShowHitNormals();
	}

	/// <summary>Sets directionReference equal to Camera.</summary>
	void OnEnable()
	{
		directionReference = Camera.main.transform;
	}

	/// <summary>Initializes Players Attributes Data.</summary>
	void Awake()
	{
		InitializePlayerData();
		actions = Actions.Idle;
		AddActionFlag(Actions.Running);
		RemoveActionFlag(Actions.Running);
		Debug.Log("[Player] Actions: " + actions);
	}

	void Start()
	{
		//directionReference = GameplayCamera.Instance.fakeCameraRotation;
	}

	/// <summary>Tracks Player's Input while Alive.</summary>
	void Update()
	{
		if(state == States.Alive)
		{//Is Player Alive?
			TrackInput();
			ExecuteActions();
			if(animator != null) UpdateAnimatorControllerParameters();
		}
	}
#endregion

#region Methods:
	/// <summary>Initializes Player's Basic [Independent] Data.</summary>
	public override void InitializePlayerData()
	{
		state = States.Alive;
		hp = maxHp;
		stamina = maxStamina;
		speed = initialSpeed;
		jumpForce = initialJumpForce;
		dashForce = initialDashForce;
		verticalAxis = 0.0f;
		horizontalAxis = 0.0f;
		jumpCooldown = 0.0f;
		dashCooldown = 0.0f;

		//Define Normal Multipliers:
		hpNormalMultiplier = (1f / maxHp);
		staminaNormalMultiplier = (1 / maxStamina);
		jumpingNormalMultiplier = (1f / (maxJumpForce - initialJumpForce));
		runningNormalMultiplier = (1f / (maxSpeed - initialSpeed));
		dashingNormalMultiplier = (1f / (maxDashForce - initialDashForce));
	}

	/// <summary>Checks the input states on the actual frame, to enable actions Flags.</summary>
	public override void TrackInput()
	{
		verticalAxis = Input.GetAxis("Vertical");
		horizontalAxis = Input.GetAxis("Horizontal");
	
		if(!staminaRecharging)
		{//If the stamina is not recharging
			if(Input.GetKey(KeyCode.Z))
			{
				AddActionFlag(Actions.Running);
			}
			else RemoveActionFlag(Actions.Running);

			running = (Input.GetKey(KeyCode.Z));
			if(Input.GetKey(KeyCode.Z)) AddActionFlag(Actions.SpecialMove);
			specialMove = (Input.GetKey(KeyCode.C));

			if((Input.GetKey(KeyCode.Space)  && jumpCooldown == 0f && isGrounded))
			{
				AddActionFlag(Actions.Jumping);
			}
			else RemoveActionFlag(Actions.Jumping);

			if(!jumping) jumping = (Input.GetKey(KeyCode.Space)  && jumpCooldown == 0f && isGrounded);

			if((Input.GetKey(KeyCode.X) && dashCooldown == 0f))
			{
				AddActionFlag(Actions.Dashing);
			}
			else RemoveActionFlag(Actions.Dashing);

			if(!dashing) dashing = (Input.GetKey(KeyCode.X) && dashCooldown == 0f);
		}

		//Walking / Running:
		if(axisesBelowZero)
		{
			//direction = directionReference.TransformDirection( new Vector3((directionReference.right.x * h), 0f, (directionReference.forward.z * v)) );
			direction = new Vector3((team.movementDirection.x * horizontalAxis), 0f, (team.movementDirection.z * verticalAxis));
			FaceDirection(direction);
		}

		if((Input.GetKeyUp(KeyCode.Space) && jumping) || jumpForce >= maxJumpForce)
		{//If jumping was released, or it reached its maximum peak.
			jumping = false;		
			jumpCooldownController = new Behavior(this, StartJumpCooldown());
		}

		if((Input.GetKeyUp(KeyCode.X) && dashing) || dashForce >= maxDashForce)
		{//If dashing was released, or if it reached its maximum peak.
			dashing = false;	
			dashCooldownController = new Behavior(this, StartDashCooldown());
		}

		if(team != null)
		if(team.powerUpsInventory[0] != null)
		{
			usePowerUp = Input.GetKeyUp(KeyCode.P);
			if(usePowerUp) team.UsePowerUp(this);
		}
	}

	/// <summary>Executes enabled Flags actions. The Method Scope </summary>
	public override void ExecuteActions()
	{
		bool executedAction = false;

		if(axisesBelowZero)
		{//If there was any Axis Input recieved.
			executedAction = true;
			if(running) Run();
			else Walk();		
		}
		else Deaccelerate(StaminaTypes.Run);
		if(jumping)
		{//If Jump Input was recieved.
			executedAction = true;
			Jump();
		}
		else Deaccelerate(StaminaTypes.Jump);
		if(dashing)
		{//If Dash Input was recieved.
			executedAction = true;
			Dash();
		}
		else Deaccelerate(StaminaTypes.Dash);
		if(specialMove)
		{
			executedAction = true;
			SpecialAction();
		}
		if(!executedAction)
		{//If no Input was recieved.
			Wait();
		}
	}

	/// <summary>Udates Player's Animnator Controller parameters.</summary>
	public override void UpdateAnimatorControllerParameters()
	{
		animator.SetBool(GROUNDED, isGrounded);
		animator.SetBool(WALK, (axisesBelowZero && !running));
		animator.SetBool(WALK, (axisesBelowZero && !HasActionFlag(Actions.Running)));
		animator.SetBool(SPECIAL, specialMove);
		animator.SetBool(SPECIAL, HasActionFlag(Actions.SpecialMove));

		animator.SetFloat(RUN, ((speed - initialSpeed) * runningNormalMultiplier));
		animator.SetFloat(JUMP, ((jumpForce - initialJumpForce) * jumpingNormalMultiplier));
		animator.SetFloat(DASH, ((dashForce - initialDashForce) * dashingNormalMultiplier));
	}

	///< <summary>Turns the Gameobject to the given direction.</summary>
	/// <param name="_direction">The direction the player will be facing.</param>
	public override void FaceDirection(Vector3 _direction)
	{
		transform.rotation = Quaternion.LookRotation(_direction);
	}

	/// <summary>Stops the Player.</summary>
	public override void Wait()
	{
		movingDirection = Vector3.zero.SetY(rigidBody.velocity.y);
		rigidBody.velocity = movingDirection;
		RecoverStamina();
	}

	/// <summary>Move the GameObject relative to its forward normal, at the initialSpeed.</summary>
	public override void Walk()
	{
		movingDirection = transform.TransformDirection(new Vector3(0f, rigidBody.velocity.y, Deaccelerate(StaminaTypes.Run)));
		if(!dashing) rigidBody.velocity = movingDirection;
		if(!jumping && !dashing && !specialMove) RecoverStamina();
	}

	/// <summary>Move the GameObject relative to its forward normal, at an accelerating speed.</summary>
	public override void Run()
	{
		movingDirection = transform.TransformDirection(new Vector3(0f, rigidBody.velocity.y, Accelerate(StaminaTypes.Run)));
		if(!dashing) rigidBody.velocity = movingDirection;
		DecreaseStamina(runStaminaSpend);
	}

	/// <summary>Moves the GameObject relative to its up normal.</summary>
	public override void Jump()
	{
		//Vector3 jumpDirection = new Vector3(movingDirection.x, Accelerate(StaminaTypes.Jump), movingDirection.z);
		Vector3 jumpDirection = movingDirection.SetY(Accelerate(StaminaTypes.Jump));
		rigidBody.velocity = jumpDirection;
		DecreaseStamina(jumpStaminaSpend);
	}

	/// <summary>Adds force to the GameObject relative to its forward normal.</summary>
	public override void Dash()
	{
		Vector3 dashDirection = transform.TransformDirection(0f, 0f, (Accelerate(StaminaTypes.Dash)));	
		rigidBody.velocity += dashDirection;
		DecreaseStamina(dashStaminaSpend);
	}

	/// <summary>Pushes Ball at Player's Force.</summary>
	/// <param name="_ball">Ball to Push.</param>
	public override void PushBall(BallModel _ball)
	{
		Vector3 finalForce = Vector3.zero;

		foreach(Vector3 hitNormal in hitNormals)
		{
			Ray ray = new Ray(transform.position, (transform.TransformDirection(hitNormal) * hitNormalRayLength));
			RaycastHit hit;

			Debug.DrawRay(ray.origin, ray.direction * hitNormalRayLength, Color.blue, 5f);

			if(Physics.SphereCast(ray, hitNormalSphereRadius, out hit, hitNormalRayLength))
			{
				finalForce += hitNormal;
				Debug.DrawRay(ray.origin, ray.direction * hitNormalRayLength, Color.red, 10f);
				//Debug.Log("Final Force " + finalForce);
			}
		}

		_ball.rigidBody.AddForce(transform.TransformDirection(finalForce).normalized * dashForce);
	}

	/// <summary>Excecutes Character's Special Action.</summary>
	public override void SpecialAction()
	{
		Debug.LogError("Player Class has yet no Special Action defined.");
	}

	/// <summary>Accelerates the given value by its respective acceleration time.</summary>
	/// <param name="_value">The value that will be accelerated.</param>
	/// <returns>Given value accelerated.</returns>
	public override float Accelerate(StaminaTypes _staminaType)
	{
		switch(_staminaType)
		{
			case StaminaTypes.Run:
			return speed = Mathf.Clamp(speed += (Time.deltaTime / speedAccelerationTime) * maxSpeed, initialSpeed, maxSpeed);
			break;

			case StaminaTypes.Jump:
			return jumpForce = Mathf.Clamp(jumpForce += (Time.deltaTime / jumpAccelerationTime) * maxJumpForce, initialJumpForce, maxJumpForce);
			break;

			case StaminaTypes.Dash:
			return dashForce = Mathf.Clamp(dashForce += (Time.deltaTime / dashAccelerationTime) * maxDashForce, initialDashForce, maxDashForce);
			break;

			default:
			Debug.LogError("Stamina Type " + _staminaType + " cannot be accelerated due that it's not defined in method's switch.");
			break;
		}

		return 0f; //Default code path.
	}

	/// <summary>Checks if the Floor collided is below Player's feet.</summary>
	/// <param name="_floor">Floor that will be used to check the direction of the collision.</param>
	/// <returns>If the Floor collided is below Player's feet.</returns>
	public override bool FloorBelowPlayerFeet(Transform _floor)
	{
		RaycastHit[] hits = Physics.RaycastAll(groundChecker);

		foreach(RaycastHit hit in hits)
		{
			if(hit.transform == _floor) return true;
		}

		return false;
	}

	/// <summary>Increases Stamina at its normal recovery rate.</summary>
	public override void RecoverStamina()
	{	
		stamina = Mathf.Clamp(stamina += (Time.deltaTime / staminaRecoveryTime) * maxStamina, 0f, maxStamina);
	}

	/// <summary>Shows the Hit Normals on Editor Mode, as they are going to be during collision with the Ball.</summary>
	public override void ShowHitNormals()
	{
		int spheresOnGizmoLine = ((int)hitNormalRayLength / (int)((hitNormalSphereRadius) * 2f)); //Define the number of spheres that will be drawn relative to the diameter of the spherecast and teh raycast length.

		for(int j = 1 ; j < (spheresOnGizmoLine + 1); j++)
		{
			float normalizedPosition = ((1.0f * j) / (1.0f * spheresOnGizmoLine)); //Parse the variables to float by multiplying by a normalized float...

			for(int i = 0; i < hitNormals.Length; i++)
			{
				Vector3 normalizedProjection = (transform.position + transform.TransformDirection( hitNormals[i] * (hitNormalRayLength * normalizedPosition) ));
				Gizmos.color = hitNormalSphereColor;
				Gizmos.DrawWireSphere(normalizedProjection, hitNormalSphereRadius);

				if(j == spheresOnGizmoLine) //Just paint the line one time...
				{
					Vector3 projection = (transform.position + transform.TransformDirection( hitNormals[i] * hitNormalRayLength ));
					Gizmos.color = hitNormalRayColor;
					Gizmos.DrawLine(transform.position, projection);
				}
			}
		}

		//Ground Checker Ray:
		Gizmos.color = Color.red;
		Gizmos.DrawLine(groundChecker.origin, (groundChecker.origin + (-transform.up * distanceToGround)));
	}

	/// <summary>Deaccelerates the given value by its respective acceleration time.</summary>
	/// <param name="_staminaType">The value that will be deaccelerated [By StaminaLossRate Identifier].</param>
	/// <returns>Given value deaccelerated.</returns>
	public override float Deaccelerate(StaminaTypes _staminaType)
	{
		switch(_staminaType)
		{
			case StaminaTypes.Run:
			return speed = Mathf.Clamp(speed -= (Time.deltaTime / speedAccelerationTime) * maxSpeed, initialSpeed, maxSpeed);
			break;

			case StaminaTypes.Jump:
			return jumpForce = Mathf.Clamp(jumpForce -= (Time.deltaTime / jumpAccelerationTime) * maxJumpForce, initialJumpForce, maxJumpForce);
			break;

			case StaminaTypes.Dash:
			return dashForce = Mathf.Clamp(dashForce -= (Time.deltaTime / dashAccelerationTime) * maxDashForce, initialDashForce, maxDashForce);
			break;

			default:
			Debug.LogError("Value " + _staminaType + " cannot be deaccelerated and it's not defined in function.");
			break;
		}

		return 0f; //Default code path.
	}

	/// <summary>Deaccelerates the stamina by the given stamina loss rate.</summary>
	/// <param name="_staminaQuantity">The stamina quantity that will be lost.</param>
	/// <returns>Stamina deaccelerated.</returns>
	public override float DecreaseStamina(float _staminaQuantity)
	{
		stamina = Mathf.Clamp(stamina -= (Time.deltaTime / staminaRecoveryTime) * (maxStamina * _staminaQuantity), 0f, maxStamina);

		//If Stamina reached 0, the Stamina Recharge Cooldown process begins.
		if(stamina <= 0.0f)
		{
			if(staminaRechargeController != null) staminaRechargeController.EndBehavior();
			staminaRechargeController = new Behavior(this, WaitTillStaminaReloads());
		}

		return stamina;
	}

#region Coroutines:
	/// <summary>Starts Jump Cooldown process.</summary>
	public override IEnumerator StartJumpCooldown()
	{
		jumpCooldown = jumpCooldownTime;

		while(jumpCooldown > 0f)
		{
			jumpCooldown -= Time.deltaTime;

			yield return null;
		}

		jumpForce = initialJumpForce;
		jumpCooldown = 0.0f;

		jumpCooldownController.EndBehavior();
	}

	/// <summary>Starts Dash Cooldown process.</summary>
	public override IEnumerator StartDashCooldown()
	{
		dashCooldown = dashCooldownTime;

		while(dashCooldown > 0f)
		{
			dashCooldown -= Time.deltaTime;

			yield return null;
		}

		dashForce = initialDashForce;
		dashCooldown = 0.0f;

		dashCooldownController.EndBehavior();
	}

	/// <summary>Begins Stamina Recovery process if stamina reached 0.</summary>
	public override IEnumerator WaitTillStaminaReloads()
	{
		staminaRecharging = true;

		running = false;
		jumping = false;
		dashing = false;
		specialMove = false;

		while(stamina < maxStamina)
		{
			RecoverStamina();
			yield return null;
		}

		staminaRecharging = false;

		staminaRechargeController.EndBehavior();
	}

	/// <summary>Executes Status Condition.</summary>
	/// <param name="_statusCondition">Status Condition to be Executed.</param>
	public override IEnumerator ExecuteStatusCondition(StatusConditions _statusCondition)
	{
		switch(_statusCondition)
		{
			case StatusConditions.Unassigned:
			break;

			case StatusConditions.Invincible:
			yield return new WaitForSeconds(stats.invencibleDuration);
			break;
		}

		yield return null;
		statusCondition = StatusConditions.Unassigned;
	}
#endregion

#endregion

#region Collisions/Triggers:
	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	void OnCollisionEnter(Collision col)
	{
		GameObject obj = col.gameObject;
	
		switch(obj.tag)
		{
			case LayerMasks.FLOOR_LAYER_MASK_KEY:
			/*Debug.Log("[Player] Contacts Colliding: " + col.contacts.Length);
			foreach(ContactPoint contact in col.contacts)
			{
				Debug.Log("[Player] Normal: " + contact.normal);
			}*/
			//isGrounded = FloorBelowPlayerFeet(obj.transform);
			//isGrounded = true;
			break;
	
			default:
			break;
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	void OnCollisionExit(Collision col)
	{
		GameObject obj = col.gameObject;
	
		switch(obj.tag)
		{
			case Tags.BALL_TAG:
			break;

			case LayerMasks.FLOOR_LAYER_MASK_KEY:
			//isGrounded = false;
			break;
	
			default:
			break;
		}
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;
	
		switch(obj.tag)
		{
			case Tags.POWER_UP_TAG:
			team.AddPowerUp(obj.GetComponent<BasePowerUp>());
			break;
	
			default:
			break;
		}
	}
#endregion
}

/*
	Tests With InputManager:

		Vertical Axis:
		InputManager.Instance.Player1Controller.GetAxis(BaseController.InputCommands.VerticalAxis);

		Horizontal Axis:
		InputManager.Instance.Player1Controller.GetAxis(BaseController.InputCommands.HorizontalAxis);

		Run:
		if(InputManager.Instance.Player1Controller.GetInputDown(BaseController.InputCommands.Run)) Debug.LogWarning("Woooow, A pressed.");
*/