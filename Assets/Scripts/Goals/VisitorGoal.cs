using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorGoal : BaseGoal
{
	private static VisitorGoal _instance; /// <summary>VisitorGoal Instance.<summary>

	/// <summary>Gets Instance property
	public static VisitorGoal Instance 
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
