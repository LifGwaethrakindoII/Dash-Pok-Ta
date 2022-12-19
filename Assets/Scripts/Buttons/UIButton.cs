using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashPokTa
{
public class UIButton : BaseButton
{
	public GameObject _player; /// </summary>Temporal Player reference. Shall move to another script.</summary>

	public override void ExcecuteAction()
	{
		switch(buttonAction)
		{
			case ButtonActions.GoToJosQuest:
			SceneAdministrator.Instance.LoadScene("Jos's Quest");
			break;

			case ButtonActions.AddPlayer:
			TeamSelectionController.AddPlayer(_player);
			break;

			default:
			Debug.LogError("Button Action "  + buttonAction.ToString() + " is not defined on switch. Be sure to update the switch.");
			break;
		}
	}
}
}