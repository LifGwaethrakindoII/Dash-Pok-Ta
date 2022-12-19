using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
	[SerializeField] [Range(15.0f, 50.0f)] private float _minimumFieldOfView; 		/// <summary>The minimum Field of View the Camera can reach.
	[SerializeField] [Range(55.0f, 150.0f)] private float _maximumFieldOfView; 		/// <summary>The maximum Field of View the Camera can reach.
	[SerializeField] [Range(0.1f, 1.5f)] private float _fieldOfViewLerpTime;		/// <summary>Time it takes each frame for the Field of View Interpolation</summary>
	[SerializeField] [Range(0.25f, 100f)] private float _cameraSpeed; 				/// <summary>Speed of the camera when interpolating.</summary>
	[SerializeField] [Range(0.25f, 10f)] private float _waypointsTravelingTime; 	/// <summary>Time the camera takes to travel through waypoints.</summary>
	[SerializeField] private Transform _fakeCameraRotation; 						/// <summary>Fake Camera's Rotation [To fix TransformDirection's Forward].</summary>
	private Camera _camera; 														/// <summary>Camera Component.</summary>
	private float _cameraHeight; 													/// <summary>Height of the Camera Transform.</summary>

	/// <summary>Gets and Sets minimumFieldOfView property.</summary>
	public float minimumFieldOfView
	{
		get { return _minimumFieldOfView; }
		set { _minimumFieldOfView = value; }
	}

	/// <summary>Gets and Sets maximumFieldOfView property.</summary>
	public float maximumFieldOfView
	{
		get { return _maximumFieldOfView; }
		set { _maximumFieldOfView = value; }
	}

	/// <summary>Gets and Sets fieldOfViewLerpTime property.</summary>
	public float fieldOfViewLerpTime
	{
		get { return _fieldOfViewLerpTime; }
		set { _fieldOfViewLerpTime = value; }
	}

	/// <summary>Gets and Sets cameraSpeed property.</summary>
	public float cameraSpeed
	{
		get { return _cameraSpeed; }
		set { _cameraSpeed = value; }
	}

	/// <summary>Gets and Sets waypointsTravelingTime property.</summary>
	public float waypointsTravelingTime
	{
		get { return _waypointsTravelingTime; }
		set { _waypointsTravelingTime = value; }
	}

	/// <summary>Gets and Sets camera property.</summary>
	public Camera camera
	{
		get
		{
			if(gameObject.Has<Camera>() && _camera == null)
			{
				_camera = GetComponent<Camera>();
			}

			return _camera;
		}
	}

	/// <summary>Gets and Sets fakeCameraRotation property.</summary>
	public Transform fakeCameraRotation
	{
		get { return _fakeCameraRotation; }
		set { _fakeCameraRotation = value; }
	}

	/// <summary>Gets and Sets cameraHeight property.</summary>
	public float cameraHeight 
	{
		get
		{
			return _cameraHeight = transform.position.y;
		}
	}
}
