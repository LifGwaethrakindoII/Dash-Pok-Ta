using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kroek : Player
{
	/// <summary>Excecutes Character's Special Action.</summary>
	public override void SpecialAction()
	{
		if(animator != null) animator.SetBool(SPECIAL, true);
	}
}