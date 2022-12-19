using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGoal : MonoBehaviour
{
	///<summary>Event triggered when the ball scores goal.</summary>
	/// <param name="_goal">Goal (Local or Visitor) that recieved the goal.</param>
	public delegate void OnGoalScored(BaseGoal _goal);
	public static OnGoalScored onGoalScored; 	/// <summary>Subscription delegate.</summary>

	private bool _goalScored = false;	 		/// <summary>Flag that avoids possible multiple goal trigger detections</summary>

	/// <summary>Gets and Sets goalScored property.<summary>
	public bool goalScored 
	{
		get { return _goalScored; }
		set { _goalScored = value; }
	}

	void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;

		switch(obj.tag)
		{
			case "Ball":
			if(!_goalScored)
			{
				onGoalScored(this);
				_goalScored = true;
			}	
			break;

			default:
			Debug.Log("Tag: " + obj.tag);
			break;
		}
	}
}
