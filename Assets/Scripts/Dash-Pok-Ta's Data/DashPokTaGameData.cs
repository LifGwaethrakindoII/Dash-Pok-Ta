using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashPokTaGameData
{
	public static class Keys
	{
		public const string FONT_CREDITS =  "Arcade Classic by Pizzadude "; 	/// <summary>Arcade Classic Font Credits.</summary>
		public const string BALL_TAG = "Ball"; 									/// <summary>Ball's GameObject Tag.</summary>
		public const string PLAYER_TAG = "Player"; 								/// <summary>Player's GameObject Tag.</summary>
		public const string CAMERA_WAYPOINT_TAG = "CameraWaypoint"; 			/// <summary>Camera Waypoint's GameObject Tag.</summary>
	}

	public class Tags
	{
		public const string BALL_TAG = "Ball"; 									/// <summary>Ball's Tag.</summary>
		public const string FLOOR_TAG = "Floor"; 								/// <summary>Floor's Tag.</summary>
		public const string POWER_UP_TAG = "Power-Up"; 							/// <summary>Power-Up's Tag.</summary>
	}

	public static class LayerMasks
	{
		public const string FLOOR_LAYER_MASK_KEY = "Floor"; 					/// <summary>Floor's Layer Mask.</summary>
	}
}