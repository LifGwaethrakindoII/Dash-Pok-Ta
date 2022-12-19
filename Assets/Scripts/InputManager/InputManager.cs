using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	/// <summary>OnControllerConnected event. Called each time a controller is connected/disconnected.</summary>
	/// <param name="_connected">Was the controll connected or disconnected?.</summary>
	public delegate void OnControllerConnected(bool _connected);
	public static OnControllerConnected onControllerConnected; 					/// <summary>OnControllerConnected subscription label.</summary>

	[SerializeField] [Range(0.3f, 5.0f)] private float inputHandlerWaitTime; 	/// <summary>Wait Time that the handler will wait to check controllers states.</summary>
	private Behavior inputHandler; 												/// <summary>Behavior that controls InputHandler Coroutine.</summary>

	private KeyboardController KeyboardController = new KeyboardController(); 	/// <summary>KeyboardController's reference.</summary>
	private Xbox360Controller Xbox360Controller = new Xbox360Controller(); 		/// <summary>Xbox360Controller's reference.</summary>

	private static InputManager _instance; 										/// <summary>InputManager Instance reference.</summary>
	private BaseController _player1Controller; 									/// <summary>Flag enabled when there is a Plauer 1 controller connected.</summary>
	private BaseController _player2Controller; 									/// <summary>Flag enabled when there is a Plauer 2 controller connected.</summary>

	/// <summary>Player Controller.</summary>
	public enum PlayerControllers
	{
		Unassigned,
		Keyboard,
		Xbox360,
		XboxOne,
		PlayStation3,
		PlayStasion4
	}

	/// <summary>Gets Instance property.</summary>
	public static InputManager Instance
	{
		get { return _instance; }
	}

	/// <summary>Gets and Sets Player1Controller.</summary>
	public BaseController Player1Controller
	{
		get { return _player1Controller; }
		set { _player1Controller = value; }
	}

	/// <summary>Gets and Sets Player2Controller.</summary>
	public BaseController Player2Controller
	{
		get { return _player2Controller; }
		set { _player2Controller = value; }
	}

	void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	void Start()
	{
		inputHandler = new Behavior(this, InputHandler());
	}

	public float GetAxis(BaseController.InputCommands _axis)
	{
		return Player1Controller.GetAxis(_axis);
	}

	public bool GetInputDown(BaseController.InputCommands _input)
	{
		return Player1Controller.GetInputDown(_input);
	}

	/// <summary>Handles Controllers Entries / Exits.</summary>
	IEnumerator InputHandler()
	{
		string[] joystickNames;

		while(true)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForSeconds(inputHandlerWaitTime);
			joystickNames = Input.GetJoystickNames();

			if(joystickNames.Length > 0)
			{
				for(int i = 0; i < joystickNames.Length; i++)
				{
					Debug.LogWarning("Joystic Name: " + joystickNames[i]);
					Debug.LogWarning("Joystic Length: " + joystickNames[i].Length);

					switch(joystickNames[i].Length)
					{
						case Xbox360Controller.JoystickNameLength:
						Player1Controller = new Xbox360Controller();
						break;

						default:
						Debug.LogError("This controller with name length " + joystickNames.Length + " is not on switch case. Update the switch");
						break;
					}
				}
			}
			else
			{
				Player1Controller = KeyboardController;
			}
			

		}
	}
}
