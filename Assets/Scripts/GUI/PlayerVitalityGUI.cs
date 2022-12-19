using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVitalityGUI : BaseGUI<PlayerModel>
{
	[Header("Bar Gauges:")]
	[SerializeField] private RectTransform hpBarGauge; 			/// <summary>The Hp gauge RectTransform Componenr.</summary>
	[SerializeField] private RectTransform staminaBarGauge; 	/// <summary>The Stamina gauge RectTransform Componenr.</summary>
	[Space(5f)]
	[Header("Vitality Bars:")]
	[SerializeField] private GameObject hpBar; 					/// <summary>The Hp bar container.</summary>
	[SerializeField] private GameObject staminaBar; 			/// <summary>The Stamina bar container.</summary>
	[Space(5f)]
	[Header("Vitality Bars Colors:")]
	[SerializeField] private Color hpBarMaxValueColor; 			/// <summary>Hp Bar's Color at Maximum Value.</summary>
	[SerializeField] private Color staminaBarMaxValueColor; 	/// <summary>Stamina Bar's Color at Maximum Value.</summary>
	[SerializeField] private Color hpBarMidValueColor; 			/// <summary>Hp Bar's Color at Medioum Value.</summary>
	[SerializeField] private Color staminaMidValueBarColor; 	/// <summary>Stamina Bar's Color at Medioum Value.</summary>
	[SerializeField] private Color hpBarMinValueColor; 			/// <summary>Hp Bar's Color at Minimum Value.</summary>
	[SerializeField] private Color staminaMinValueBarColor; 	/// <summary>Stamina Bar's Color at Minimum Value.</summary>
	[Space(5f)]
	[SerializeField] private float offsetFromPlayer; 			/// <summary>Offset from Player.</summary>	
	private Image hpBarImage; 									/// <summary>The Hp gauge Image Componenr.</summary>
	private Image staminaBarImage; 								/// <summary>The Stamina gauge Image Componenr.</summary>
	private CanvasGroup hpBarGroup; 							/// <summary>Hp Bar's CanvasGroup Component.</summary>
	private CanvasGroup staminaBarGroup; 						/// <summary>Stamina Bar's CanvasGroup Component.</summary>
	private float playerHeight; 								/// <summary>Player's Half-Height by it's bounds extents.</summary>

	/// <summary>Assign private properties.</summary>
	void Awake()
	{
		hpBarGroup = hpBar.GetComponent<CanvasGroup>();
		staminaBarGroup = staminaBar.GetComponent<CanvasGroup>();
		hpBarImage = hpBarGauge.GetComponent<Image>();
		staminaBarImage = staminaBarGauge.GetComponent<Image>();

		hpBarImage.color = hpBarMaxValueColor;
		staminaBarImage.color = staminaBarMaxValueColor;
	}

	/// <summary>Method invoked when GUI's Data is assigned.</summary>
	protected override void OnDataAssigned()
	{
		//Where Data is PlayerModel.
		playerHeight = Data.collider.bounds.extents.y;
	}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_player">Data that the GUI will recieve. This Data is of PlayerModel Type</param>
	public override void UpdateGUI(PlayerModel _player)
	{
		transform.position = _player.transform.position.SetY(_player.transform.position.y + (playerHeight + offsetFromPlayer));

		//Update Scales.
		hpBarGauge.localScale = new Vector3((_player.hp * _player.hpNormalMultiplier), 1f, 1f);
		staminaBarGauge.localScale = new Vector3((_player.stamina * _player.staminaNormalMultiplier), 1f, 1f);

		//Update CanvasGroup Alphas.
		hpBarGroup.alpha = (_player.hp < _player.maxHp) ? 1.0f : 0.0f; 
		staminaBarGroup.alpha = (_player.stamina < _player.maxStamina) ? 1.0f : 0.0f; 
	}
}
