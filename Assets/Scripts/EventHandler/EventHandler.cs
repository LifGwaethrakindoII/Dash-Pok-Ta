using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour
{
	public delegate void eventHandler(GameObject _player);

	public static eventHandler targetAssignHandler;
	public static eventHandler planeAssignHandler;
		
}
