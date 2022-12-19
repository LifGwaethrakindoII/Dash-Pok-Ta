using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
	private PlayerModel _mainDefender; 			/// <summary>Team's Main Defender.</summary>
	private PlayerModel _supportDefender; 		/// <summary>Team's Support Defender.</summary>
	private PlayerModel _supportAttacker; 		/// <summary>Team's Support Attacker.</summary>
	private PlayerModel _mainAttacker; 			/// <summary>Team's Main Attacker.</summary>

	public enum TeamRoles 						/// <summary>Team Roles.</summary>
	{
		SupportDefender, 						/// <summary>Support Defender Team Role.</summary>
		MainDefender, 							/// <summary>Main Defender Team Role.</summary>
		SupportAttacker, 						/// <summary>Support Attacker Team Role.</summary>
		MainAttacker 							/// <summary>Main Attacker Team Role.</summary>
	}
}