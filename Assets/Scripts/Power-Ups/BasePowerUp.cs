using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidlessUtilities;

namespace DashPokTa
{
public abstract class BasePowerUp : MonoBehaviour, IPoolObject
{
	[SerializeField] private float _rotationSpeed; 		/// <summary>Power-Up's Rotation speed when on Pickable state.</summary>
	[Space(5f)]
	[Header("Power-Up Stats:")]
	[SerializeField] private ScriptableObject _data; 	/// <summary>Particular data for this Power-Up.</summary>
	[Header("GUI Feedback:")]
	[SerializeField] private Sprite _powerUpImage; 		/// <summary>Power-Up's presentation image on the GUI.</summary>
	private States _state; 								/// <summary>Power-Up's current state.</summary>

	public enum States
	{
		Pickable
	}

	/// <summary>Gets and Sets rotationSpeed property.</summary>
	public float rotationSpeed { get { return _rotationSpeed; } }

	/// <summary>Gets and Sets powerUpImage property.</summary>
	public Sprite powerUpImage { get { return _powerUpImage; } }

	/// <summary>Gets and Sets state property.</summary>
	public States state
	{
		get { return _state; }
		set { _state = value; }
	}

	void Update()
	{
		switch(state)
		{
			case States.Pickable:
			transform.Rotate(0.0f, (rotationSpeed * Time.deltaTime), 0.0f);
			break;
		}
	}

	/// <summary>Activates Power-Up.</summary>
	/// <param name="_player">Player that Activates this Power-Up.</param>
	public abstract void ActivatePowerUp(PlayerModel _player);

	/// <summary>Independent Actions made when this Pool Object is being created.</summary>
	public virtual void OnObjectCreation()
	{
		//...
	}

	/// <summary>Actions made when this Pool Object is being activated.</summary>
	public virtual void OnObjectActivation()
	{
		//...
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public virtual void OnObjectReset()
	{
		//...
	}

	/// <summary>Actions made when this Pool Object is being destroyed.</summary>
	public virtual void OnObjectDestruction()
	{
		//...
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;
	
		switch(obj.tag)
		{
			case "Player":
			if(state == States.Pickable) gameObject.SetActive(false);
			break;
	
			default:
			break;
		}
	}
}
}