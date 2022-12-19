using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineController : Singleton<CoroutineController>
{
	void Awake()
	{
		if(Instance != this)
		{
			Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
		}
		
	}

#region CoroutineCallers:
	/// <summary>Creates Behavior that controls WaitSecondsCoroutine.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour caller reference to check if it's still alive at each ticking.</param>
	/// <param name="_seconds">Seconds to wait.</param>
	/// <param name="onWaitEnds">Action called when the wait ends.</param>
	/// <returns>Behaviour with WaitSecondsCoroutine reference on the constructor.</returns>
	public Behavior WaitSeconds(MonoBehaviour _monoBehaviour, float _seconds, Action onWaitEnds)
	{
		return new Behavior(this, WaitSecondsCoroutine(_monoBehaviour, _seconds, onWaitEnds));
	}

	/*public Behavior DoForSeconds(MonoBehaviour _monoBehaviour, float _seconds, Action actionDuringTick, Action onWaitEnds)
	{
		return new Behavior(this, Wait);
	}*/

	/// <summary>Creates Behavior that controls WaitUntilCoroutine.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour caller reference to check if it's still alive at each ticking.</param>
	/// <param name="_seconds">Seconds to wait.</param>
	/// <param name="onWaitEnds">Action called when the wait ends.</param>
	/// <returns>Behavior with WaitUntilCoroutine reference on the constructor.</returns>
	public Behavior WaitUntil(MonoBehaviour _monoBehaviour, Func<bool> _condition, Action onConditionGiven)
	{
		return new Behavior(this, WaitUntilCoroutine(_monoBehaviour, _condition, onConditionGiven));
	}

	public Behavior WaitWhile(MonoBehaviour _monoBehaviour, Func<bool> _condition, Action onConditionGiven)
	{
		return new Behavior(this, WaitWhileCoroutine(_monoBehaviour, _condition, onConditionGiven));
	}

	public Behavior Rotate(MonoBehaviour _monoBehaviour, Quaternion _rotation, float _seconds, Action onRotationEnds)
	{
		return new Behavior(this, RotateOnSeconds(_monoBehaviour, _rotation, _seconds, onRotationEnds));
	}
#endregion

#region Coroutines:
	/// <summary>Waits 'x' seconds, then executes an Action [if it was passed by parameter].</summary>
	/// <param name="_monoBehaviour">MonoBehaviour caller reference to check if it's still alive at each ticking.</param>
	/// <param name="_seconds">Seconds to wait.</param>
	/// <param name="onWaitEnds">Action called when the wait ends.</param>
	private IEnumerator WaitSecondsCoroutine(MonoBehaviour _monoBehaviour, float _seconds, Action onWaitEnds)
	{
		while(_monoBehaviour.gameObject != null)
		{
			yield return new WaitForSeconds(_seconds);
			if(onWaitEnds != null) onWaitEnds();
		}	
	}

	/// <summary>Waits until a condition is given, then executes an Action [if it was passed by parameter].</summary>
	/// <param name="_monoBehaviour">MonoBehaviour caller reference to check if it's still alive at each ticking.</param>
	/// <param name="_seconds">Seconds to wait.</param>
	/// <param name="onWaitEnds">Action called when the condition is given.</param>
	private IEnumerator WaitUntilCoroutine(MonoBehaviour _monoBehaviour, Func<bool> _condition, Action onConditionGiven)
	{
		while(_monoBehaviour.gameObject != null)
		{
			yield return new WaitUntil(_condition);
			if(onConditionGiven != null) onConditionGiven();
		}
	}

	/// <summary>Waits until a condition is given, then executes an Action [if it was passed by parameter].</summary>
	/// <param name="_monoBehaviour">MonoBehaviour caller reference to check if it's still alive at each ticking.</param>
	/// <param name="_seconds">Seconds to wait.</param>
	/// <param name="onWaitEnds">Action called when the condition is given.</param>
	private IEnumerator WaitWhileCoroutine(MonoBehaviour _monoBehaviour, Func<bool> _condition, Action onConditionGiven)
	{
		while(_monoBehaviour.gameObject != null)
		{
			yield return new WaitWhile(_condition);
			if(onConditionGiven != null) onConditionGiven();
		}
	}

	private IEnumerator RotateOnSeconds(MonoBehaviour _monoBehaviour, Quaternion _rotation, float _seconds, Action onRotationEnds)
	{
		Transform monoTransform = _monoBehaviour.gameObject.transform;
		Quaternion initialRotation = monoTransform.rotation;
		float normalizedTime = 0.0f;

		while((normalizedTime < 1.0f) && (_monoBehaviour != null))
		{
			monoTransform.rotation = Quaternion.Lerp(initialRotation, _rotation, normalizedTime);
			normalizedTime += (Time.deltaTime / _seconds);
			yield return null;
		}

		if(onRotationEnds != null) onRotationEnds();
	}
#endregion
}
