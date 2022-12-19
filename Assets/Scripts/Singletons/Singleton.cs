using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance; /// <summary>T Instance reference.</summary>
	
	/// <summary>Gets and Sets Instance property.</summary>
	public static T Instance
	{
		get
      {
         if(_instance == null)
         {
            _instance = (T) FindObjectOfType(typeof(T));
 
            if (_instance == null)
            {
               Debug.LogError("An Instance of " + typeof(T) + 
                  " is needed in the scene, but there is none.");
            }
         }
 
         return _instance;
      }
	}
}