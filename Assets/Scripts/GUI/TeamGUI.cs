using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DashPokTa
{
public class TeamGUI : BaseGUI<TeamManager>
{
	/// \TODO Automatic UI instantiation depending on the Team's Inventory size.
	[SerializeField] private List<Image> _powerUpBoxes;

	/// <summary>Gets and Sets powerUpBoxes property.</summary>
	public List<Image> powerUpBoxes { get { return _powerUpBoxes; } }

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_teamManager">Data that the GUI will recieve.</param>
	public override void UpdateGUI(TeamManager _teamManager)
	{
		for(int i = 0; i < _teamManager.powerUpsInventory.Count; i++)
		{
			powerUpBoxes[i].sprite = _teamManager.powerUpsInventory[i] != null ? _teamManager.powerUpsInventory[i].powerUpImage : null;
		}
	}

	/// <summary>Method invoked when GUI's Data is assigned.</summary>
	protected override void OnDataAssigned()
	{
		for(int i = 0; i < TeamManager.POWER_UP_INVENTORY_SIZE; i++)
		{
			/// Instantiate a Power-Up Item Box and parent it to this GUI:
		}
	}
}
}