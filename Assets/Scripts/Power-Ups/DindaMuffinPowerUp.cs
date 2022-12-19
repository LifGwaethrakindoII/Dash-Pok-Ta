using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DashPokTa
{
public class DindaMuffinPowerUp : BasePowerUp
{
	/// <summary>Activates Power-Up.</summary>
	/// <param name="_player">Player that Activates this Power-Up.</param>
	public override void ActivatePowerUp(PlayerModel _player)
	{
		Debug.Log("[DindaMuffinPowerUp] Giving invencibility to player...");
	}
}
}