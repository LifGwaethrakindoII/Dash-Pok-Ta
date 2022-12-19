using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerGUI : BaseGUI<TimeClock>
{
	[SerializeField] private Text timerText; 	/// <summary>Timer Text.</summary>

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_timeClock">Data that the GUI will recieve. Being a TimeClock class</param>
	public override void UpdateGUI(TimeClock _timeClock)
	{
		timerText.text = _timeClock.leftMinutes + ":" + _timeClock.leftSeconds.ToString("00");
	}
}
