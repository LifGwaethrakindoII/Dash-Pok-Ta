using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGoal : BaseGoal
{
	private static LocalGoal _instance; /// <summary>LocalGoal Instance.<summary>

	/// <summary>Gets Instance property
	public static LocalGoal Instance 
	{
		get { return _instance; }
	}

	void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
		}
		else Destroy(gameObject);
	}
}
