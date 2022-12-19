using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidlessUtilities;

namespace DashPokTa
{
public class PlayerMarkerGUI : BaseGUI<Transform>
{
	[Header("UI Resources:")]
	[SerializeField] private Text markerText; 				/// <summary>GUI's Main Text. Holds the Player's Index.</summary>
	[SerializeField] private Image markerImage; 			/// <summary>Marker's Image.</summary>
	[Space(5f)]
	[Header("Visual Attributes:")]
	[SerializeField] private Color markerColor; 			/// <summary>Marker's Color.</summary>
	[SerializeField] private string _markerDescription; 	/// <summary>Marker's Description.</summary>
	[SerializeField] private float offsetFromPlayer; 		/// <summary>Marker's Y offset from Player's Y.</summary>
	[Space(5f)]
	[Header("Movement Attributes:")]
	[SerializeField] private float movementAmplitude; 		/// <summary>Markers amplitude movement on the Y-Axis.</summary>
	[SerializeField] private float movementSpeed; 			/// <summary>Markers Amplitude movement's speed.</summary>
	private Transform gameplayCamera; 						/// <summary>GameplayCamera's Instance Transform.</summary>

	/// <summary>Gets and Sets markerDescription.</summary>
	public string markerDescription
	{
		get { return _markerDescription; }
		set { markerText.text = _markerDescription = value; }
	}

	/// <summary>(Overriden) Gets image property.</summary>
	public override Image image
	{
		get
		{
			image = markerImage;
			return image;
		}
	}

	/// <summary>(Overriden) Gets text property.</summary>
	public override Text text
	{
		get
		{
			text = markerText;
			return text;
		}
	}

	/// <summary>GUI's Initialization.</summary>
	void Awake()
	{
		markerText.text = _markerDescription;
		markerImage.color = markerColor;
	}

	/// <summary>Get GameplayCamera's Instance Transform property.</summary>
	void Start()
	{
		gameplayCamera = GameplayCamera.Instance.transform;
	}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_updatedData">Data that the GUI will recieve.</param>
	public override void UpdateGUI(Transform _updatedData)
	{
		//transform.LookAt(gameplayCamera);
		transform.rotation = Quaternion.LookRotation(transform.position.GetDirectionTowards(gameplayCamera.position)).SetY(0f);
		//targetY + (amplitude * Sen(speed * time)).
		transform.localPosition = _updatedData.position.SetY((_updatedData.position.y + offsetFromPlayer) + (movementAmplitude * (Mathf.Sin(movementSpeed * Time.time))));
	}
}
}