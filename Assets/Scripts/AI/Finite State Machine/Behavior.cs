using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Behavior
{
	private IEnumerator _iEnumerator; 		/// <summary>Behavior's IEnumerator.</summary>
	private Coroutine _lastCoroutine; 		/// <summary>Last Coroutine store, allows control of the Behavior Class.</summary>
	private MonoBehaviour _monoBehaviour; 	/// <summary>MonoBehaviour from where Behavior is instantiated.</summary>
	private bool _running; 					/// <summary>Flag that tells if the Behavior has currently a coroutine running.</summary>

	/// <summary>Gets and Sets iEnumerator property.</summary>
	public IEnumerator iEnumerator
	{
		get { return _iEnumerator; }
		set { _iEnumerator = value; }
	}

	/// <summary>Gets and Sets lastCoroutine property.</summary>
	public Coroutine lastCoroutine
	{
		get { return _lastCoroutine; }
	}

	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		set { _monoBehaviour = value; }
	}

	/// <summart>Gets running property.</summary>
	public bool running
	{
		get { return (_lastCoroutine != null); }
	}

	/// <summary>Behavior class Constructor.</summary>
	/// <param name="_mono">MonoBehaviour from where the coroutine belongs.</param>
	/// <param name="_coroutine">Coroutine that will be initialized.</param>
	public Behavior(MonoBehaviour _mono, IEnumerator _coroutine)
	{
		_monoBehaviour = _mono;
		_iEnumerator = _coroutine;

		if(_lastCoroutine == null)
		{
			_lastCoroutine = _monoBehaviour.StartCoroutine(_iEnumerator);
		}
	}

	/// <summary>Behavior Class destructor.</summary>
	~Behavior()
	{
		//...
	}

	/// <summary>Stops the current Coroutine, then it starts it again.</summary>
	public void ResetBehavior()
	{
		if(_lastCoroutine != null)
		{
			_monoBehaviour.StopCoroutine(_lastCoroutine);
			_lastCoroutine = _monoBehaviour.StartCoroutine(_iEnumerator);
		}
	}

	/// <summary>Kills the Behavior.</summary>
	public void EndBehavior()
	{
		if(_lastCoroutine != null)
		{
			_monoBehaviour.StopCoroutine(lastCoroutine);
			_lastCoroutine = null;
		}
	}
}
