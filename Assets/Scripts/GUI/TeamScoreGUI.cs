using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamScoreGUI : BaseGUI<Game>
{
	[SerializeField] private Text localTeamScore; 		/// <summary>Score Text of the Local Team.</summary>
	[SerializeField] private Text visitorTeamScore; 	/// <summary>Score Text of the Visitor Team.</summary>

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_game">Game that the GUI will recieve.</param>
	public override void UpdateGUI(Game _game)
	{
		localTeamScore.text = _game.localTeamScore.ToString();
		visitorTeamScore.text = _game.visitorTeamScore.ToString();
	}
}
