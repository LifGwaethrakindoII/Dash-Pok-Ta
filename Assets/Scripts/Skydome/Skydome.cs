using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skydome : Singleton<Skydome>, IFiniteStateMachine<Skydome.Daytimes>
{
	[SerializeField] private float rotationsPerHour; 		/// <summary>Rotations per hour that the Skydome does, determines speed.</summary>
	[Space(5f)]
	[Header("Sky Colors:")]
	[SerializeField] private Color morningColor; 			/// <summary>Morning Sky Color.</summary>
	[SerializeField] private Color afternoonColor; 			/// <summary>Afternoon Sky Color.</summary>
	[SerializeField] private Color eveningColor; 			/// <summary>Evening Sky Color.</summary>
	[SerializeField] private Color nightColor; 				/// <summary>Morning Sky Color.</summary>
	[Space(5f)]
	[SerializeField] private float skyToneLerpDuration; 	/// <summary>Time it takes for the Sky Tone to interpolate.</summary>
	private Behavior skyToneLerper; 						/// <summary>ChangeSkyTone Coroutine Controller.</summary>
	private Daytimes _dayTime; 								/// <summary>Skydome's Daytime.</summary>

	/// <summary>Skydome Daytimes.</summary>
	public enum Daytimes
	{
		Morning, 											/// <summary>Morning Daytime.</summary>
		Afternoon, 											/// <summary>Afternoon Daytime.</summary>
		Evening, 											/// <summary>Evening Daytime.</summary>
		Night 												/// <summary>Night Daytime.</summary>
	}

	/// <summary>Gets and Sets dayTime property.</summary>
	public Daytimes dayTime
	{
		get { return _dayTime; }
		set
		{
			ExitState(_dayTime);
			EnterState(_dayTime = value);
		}
	}

#region FiniteStateMachine:
	/// <summary>Enters Daytimes State.</summary>
	/// <param name="_state">Daytimes State that will be entered.</param>
	public void EnterState(Skydome.Daytimes _dayTime)
	{
		switch(_dayTime)
		{
			case Daytimes.Morning:
			skyToneLerper = new Behavior(this, ChangeSkyTone(morningColor));
			break;

			case Daytimes.Afternoon:
			skyToneLerper = new Behavior(this, ChangeSkyTone(afternoonColor));
			break;

			case Daytimes.Evening:
			skyToneLerper = new Behavior(this, ChangeSkyTone(eveningColor));
			break;

			case Daytimes.Night:
			skyToneLerper = new Behavior(this, ChangeSkyTone(nightColor));
			break;

			default:
			break;
		}
	}

	/// <summary>Exited Daytimes State.</summary>
	/// <param name="_state">Daytimes State that will be left.</param>
	public void ExitState(Skydome.Daytimes _dayTime)
	{
		switch(_dayTime)
		{
			case Daytimes.Morning:
			break;

			case Daytimes.Afternoon:
			break;

			case Daytimes.Evening:
			break;

			case Daytimes.Night:
			break;

			default:
			break;
		}

		if(skyToneLerper != null) skyToneLerper.EndBehavior();
	}
#endregion

	/// <summary>Singleton's Instance Policies.</summary>
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
		else DontDestroyOnLoad(gameObject);
	}

	/// <summary>Skydome's rotation at each frame.</summary>
	void Update()
	{
		RotateSkydome();
	}

	/// <summary>Rotates Skydome.</summary>
	private void RotateSkydome()
	{

	}

	/// <summary>Lerps current Sky Tone to new Sky Tone.</summary>
	/// <param name="_newSkyTone">New Sky Tone.</param>
	IEnumerator ChangeSkyTone(Color _newSkyTone)
	{
		float normalizedTime = 0.0f;

		while(normalizedTime < 1.0f)
		{
			normalizedTime += (Time.deltaTime / skyToneLerpDuration);
			yield return null;
		}

		skyToneLerper.EndBehavior();
	}
}
