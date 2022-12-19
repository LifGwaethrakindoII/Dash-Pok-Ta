using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform waypoint;

	private GameObject player;
	private GameObject ball;
	private Vector3 offset;
	private Vector3 waypointOffset;
	private Vector3 startingPos;

	private Quaternion waypointPlane;
	private Quaternion defaultRotation;

	private bool changing; //Flag that avoids that goToNewTarget function and the update following doesn't run simultaneously...
	private bool turning;

	private float originalHeight;
	private float dinamicHeight;

	private float cameraY;
	private float originalCameraY;

	MeshRenderer playerRenderer;
	MeshRenderer ballRenderer;

	Vector3 angle;
	Vector3 referenceOffset;

	public GameObject borderLimit;
	float borderOffsetZ;

	bool rotado;
	bool zooming;

	/*float offsetX;
	float offsetY;*/

	//Viewport conversions. It will check when an object is invisible to the camera...
	Vector3 playerView;
	Vector3 ballView;
	float fieldOfView;
	float maxFieldOfView;
	float minFieldOfView;
	float fovLerpTime;

	Camera camera;

	float offsetZ; //Sice apparently we want the camera to always be at the center, it will only take the offset between the ball and the camera position.z...
	float waypointOffsetZ;
	float referenceOffsetZ;

	void Start()
	{
		zooming = false;
		borderOffsetZ = 0.0f;
		playerRenderer = null;
		originalHeight = this.transform.position.y;
		dinamicHeight = 48f;
		turning = false;
		defaultRotation = this.transform.rotation;
		angle = transform.eulerAngles;
		this.changing = false;
		startingPos = this.transform.position;
		offset = startingPos - player.transform.position;
		waypointOffset = waypoint.position - player.transform.position;
		referenceOffset = offset;

		ballRenderer = ball.GetComponent<MeshRenderer>();
		
		cameraY = transform.position.y;
		originalCameraY = cameraY;

		offsetZ = startingPos.z - player.transform.position.z;
		waypointOffsetZ = waypoint.position.z - player.transform.position.z;
		referenceOffsetZ = offsetZ;
		/*offsetX = startingPos.x - player.transform.position.x;
		offsetY = startingPos.y - player.transform.position.y;
		offsetZ = startingPos.z - player.transform.position.z;*/

		camera = this.GetComponent<Camera>();
		fieldOfView = 50f;
		minFieldOfView = 50f;
		maxFieldOfView = 120f;
		fovLerpTime = 0.7f;
	}

	public void initScript()
	{
		originalHeight = this.transform.position.y;
		dinamicHeight = 48f;
		turning = false;
		defaultRotation = this.transform.rotation;
		angle = transform.eulerAngles;
		this.changing = false;
		startingPos = this.transform.position;
		offset = startingPos - player.transform.position;
		waypointOffset = waypoint.position - player.transform.position;
		referenceOffset = offset;
		cameraY = transform.position.y;
		originalCameraY = cameraY;
	}

	void resetOffset() //resets the offset. Called just when the camera is assigned to a new rotation...
	{
		//offset = this.transform.position - player.transform.position;
		Vector3 middlePoint = followSelectedPlayerAndBall();
		offsetZ = this.transform.position.z - middlePoint.z;
	}

	/*public void changeTarget()
	{
		offset = transform.position - this.player.transform.position;
	}*/

	public void followTarget()
	{
		Vector3 tempPos = new Vector3(transform.position.x, cameraY, player.transform.position.z + offsetZ);
		//Vector3 newPos = new Vector3(0, cameraY, transform.position.z);
		transform.position = tempPos;
	}

	public Vector3 followSelectedPlayerAndBall()
	{
		float tempZ = (player.transform.position.z + ball.transform.position.z) * 0.5f;
		float tempX = (player.transform.position.x + ball.transform.position.x) * 0.5f;

		Vector3 tempPos = new Vector3(tempX, cameraY, tempZ + offsetZ);
		//Vector3 newPos = new Vector3(0, cameraY, transform.position.z);
		//transform.position = tempPos;

		return tempPos;
	}

	public void goToNewTarget()
	{
		this.changing = true;
		Vector3 goTo = this.player.transform.position + offset;
		this.transform.position = Vector3.Lerp(this.transform.position, goTo, 0.5f);
		this.changing = false;
	}



	public void setTarget(GameObject _player)
	{
		this.player = _player;
		playerRenderer = _player.GetComponent<MeshRenderer>();
		StartCoroutine(goToNewMiddlePoint());
	}

	public void setBall(GameObject _ball)
	{
		this.ball = _ball;
	}

	public void setBorderOffset()
	{
		Vector3 middlePoint = followSelectedPlayerAndBall();

		borderOffsetZ = Mathf.Abs(middlePoint.z - borderLimit.transform.position.z);
	}

	public void setNewWaypointOffsetZ()
	{
		Vector3 middlePoint = followSelectedPlayerAndBall();

		waypointOffsetZ = waypoint.position.z - middlePoint.z;
	}

	public GameObject getTarget()
	{
		return this.player;
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "referencePlane")
		{
			Debug.Log("Collided with Waypoint Plane");		
			
			//if(!turning)
			{
				//Transform tempWaypoint = col.gameObject.GetComponent<WaypointPlane>().getWaypointPlane();
				//Debug.Log("Waypoint Y: " + tempWaypoint.position.y);
				turning = true;
				//waypointPlane = tempWaypoint.rotation; //Get the quaternion rotation of the reference plane you are colliding with...
				//setBorderOffset();
				//StartCoroutine(changeBorderOffset());
				//StartCoroutine(rotateCamera(tempWaypoint));
				//StopCoroutine(returnOriginalRotation());
			}	
		}

		if(col.gameObject.tag == "returnPlane")
		{
			//if(!turning)
			{
				turning = true;
				StartCoroutine(returnOriginalRotation());
				//StopCoroutine("rotateCamera");
			}		
		}
	}

	/*void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == "referencePlane")
		{
			turning = true;
		}
	}*/

	/*void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "waypointPlane")
		{
			Debug.Log("Exited Collision with Waypoint Plane");
			StartCoroutine(returnOriginalRotation());

			if(turning)
			{
				turning = false;
			}
		}
	}*/

	IEnumerator changeBorderOffset()
	{
		float duration = 0.3f;
		float normalizedTime = 0.0f;

		while(normalizedTime < 1.0f)
		{
			offsetZ = Mathf.Lerp(offsetZ, waypointOffsetZ, normalizedTime);
			normalizedTime += Time.deltaTime / duration;

			yield return null;
		}
	}

	IEnumerator rotateCamera(Transform _waypoint)
	{
		//setNewWaypointOffsetZ();
		float rotationSpeed = 1f;
		Vector3 newPos = new Vector3(followSelectedPlayerAndBall().x, _waypoint.position.y, transform.position.z);
		rotado = true;
		float normalizedTime = 0.0f;
      	while (normalizedTime < 1.0f)
		{
			newPos = new Vector3(followSelectedPlayerAndBall().x, _waypoint.position.y, transform.position.z);
			setNewWaypointOffsetZ();
			transform.rotation = Quaternion.Lerp(transform.rotation, waypointPlane, normalizedTime);
			transform.position = Vector3.Lerp(transform.position, newPos, normalizedTime);
			//offset = Vector3.Lerp(offset, waypointOffset, normalizedTime);
			offsetZ = Mathf.Lerp(offsetZ, waypointOffsetZ, normalizedTime);
			cameraY = Mathf.Lerp(cameraY, _waypoint.position.y, normalizedTime);

			normalizedTime += Time.deltaTime / rotationSpeed;
			
			//if(offsetZ != waypointOffsetZ)
			//resetOffset();
			

			yield return null;
		}

		//resetOffset();

		turning = false;
		StopCoroutine(rotateCamera(_waypoint));
	}

	IEnumerator returnOriginalRotation()
	{
		//setNewWaypointOffsetZ();
		float rotationSpeed = 1f;
		Vector3 newPos = new Vector3(followSelectedPlayerAndBall().x, originalHeight, transform.position.z);
		rotado = false;
		float normalizedTime = 0.0f;
      	while (normalizedTime < 1.0f)
		{
			newPos = new Vector3(followSelectedPlayerAndBall().x, originalHeight, transform.position.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, normalizedTime);
			transform.position = Vector3.Lerp(transform.position, newPos, normalizedTime);
			//offset = Vector3.Lerp(offset, referenceOffset, normalizedTime);
			offsetZ = Mathf.Lerp(offsetZ, referenceOffsetZ, normalizedTime);
			cameraY = Mathf.Lerp(cameraY, originalCameraY, normalizedTime);

			normalizedTime += Time.deltaTime / rotationSpeed;

			//if(offsetZ != referenceOffsetZ)
			//resetOffset();

			yield return null;
		}

		//resetOffset();

		turning = false;
		StopCoroutine(returnOriginalRotation());
	}

	IEnumerator goToNewMiddlePoint()//Assings new middle point smoothly. Called when switching player...
	{
		float duration = 0.5f;
		Vector3 _pos = followSelectedPlayerAndBall();
		this.changing = true;

		float normalizedTime = 0.0f;

		while(normalizedTime < 1.0f)
		{
			_pos = followSelectedPlayerAndBall();
			this.transform.position = Vector3.Lerp(this.transform.position, _pos, normalizedTime);

			normalizedTime += Time.deltaTime / duration;

			yield return null;
		}

		this.changing = false;
	}
	
	void LateUpdate()
	{
		if(player != null && ball != null) //Those null checkers. (PS. If you're reading dis...nutz, whoooooo gotcha!!)
		{
			if(!changing)
			{
				//followTarget();
				transform.position = followSelectedPlayerAndBall();
			}
			
			/*playerView = camera.WorldToViewportPoint(player.transform.position);
			ballView = camera.WorldToViewportPoint(ball.transform.position);*/

			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, (player.transform.IsVisibleToCamera() && ball.transform.IsVisibleToCamera() ? minFieldOfView : maxFieldOfView) , (Time.deltaTime * fovLerpTime));

			//Debug.Log("Player View: " + playerView);
			//Debug.Log("Ball View: " + ballView);

			/*if(playerView.x < 0.0f || playerView.x > 1.0f || playerView.y < 0.0f || playerView.y > 1.0f
			|| ballView.x < 0.0f || ballView.x > 1.0f || ballView.y < 0.0f || ballView.y > 1.0f && camera.fieldOfView < maxFieldOfView)
			{
				//Mathf.Clamp(fieldOfView, minFieldOfView, maxFieldOfView);
				//Debug.Log("Should sum field of view...");
				//fieldOfView += 0.5f;
				
				//camera.fieldOfView = fieldOfView;
				camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, maxFieldOfView, Time.deltaTime * fovLerpTime);
			}
			else if(playerView.x > 0.0f && playerView.x < 1.0f && playerView.y > 0.0f && playerView.y < 1.0f
			&& ballView.x > 0.0f && ballView.x < 1.0f && ballView.y > 0.0f && ballView.y < 1.0f && camera.fieldOfView > minFieldOfView)
			{
				//Debug.Log("Should diminish field of view...");
				//Mathf.Clamp(fieldOfView, minFieldOfView, maxFieldOfView);
				//fieldOfView -= 0.5f;
				
				//camera.fieldOfView = fieldOfView;
				camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, minFieldOfView, Time.deltaTime * fovLerpTime);
			}*/
		}
	}
}