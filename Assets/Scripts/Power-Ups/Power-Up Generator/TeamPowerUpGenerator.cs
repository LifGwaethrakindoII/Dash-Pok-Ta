using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPowerUpGenerator : MonoBehaviour
{
	[SerializeField] private ParticleEffectController generatorEnergySource; 	/// <summary>Power-Up Generator's particle effect that depicts the generator source.</summary>
	[SerializeField] private Transform generatorHoldingCenter; 					/// <summary>Place where the Power-Up will be spawned until a character picks it.</summary>

#region UnityMethods:
	void OnEnable()
	{

	}

	void OnDisable()
	{

	}

	/// <summary>TeamPowerUpGenerator's' instance initialization.</summary>
	void Awake()
	{
		
	}
#endregion

#region PublicMethods:
	
#endregion

#region PrivateMethods:
	
#endregion
}