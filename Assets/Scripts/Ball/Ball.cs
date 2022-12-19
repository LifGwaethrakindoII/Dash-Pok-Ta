using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DashPokTaGameData;

namespace DashPokTa
{
public class Ball : BallModel
{
#region Events:
	///<summary>Event triggered when the ball leaves (or enters) the scenario boundaries.</summary>
	/// <param name="_left">True if left, False if entered.</param>
	public delegate void OnBallLeft(bool _left);
	public static event OnBallLeft onBallLeft; 								/// <summary>Subscription delegate.</summary>

	///<summary>Event triggered when the ball collides with a player.</summary>
	/// <param name="_team">Team the player belongs.</param>
	public delegate void OnBallCollidedWithPlayer(TeamManager _team);
	public static event OnBallCollidedWithPlayer onBallCollidedWithPlayer; 	/// <summary>Subscription Delegate.</summary>
#endregion
	
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
	}

	void OnDrawGizmosSelected()
	{
		int spheresOnGizmoLine = ((int)raycastLength / (int)(/*Mathf.Floor*/(sphereColliderRadius) * 2f)); //Define the number of spheres that will be drawn relative to the diameter of the spherecast and teh raycast length.

		for(int j = 1 ; j < (spheresOnGizmoLine + 1); j++)
		{
			float normalizedPosition = ((1.0f * j) / (1.0f * spheresOnGizmoLine)); //Parse the variables to float by multiplying by a normalized float...

			for(int i = 0; i < identities.Length; i++)
			{
				Vector3 normalizedProjection = (transform.position + transform.TransformDirection( identities[i] * (raycastLength * normalizedPosition) ));
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(normalizedProjection, sphereColliderRadius);

				if(j == spheresOnGizmoLine) //Just paint the line one time...
				{
					Vector3 projection = (transform.position + transform.TransformDirection( identities[i] * raycastLength ));
					Gizmos.color = Color.blue;
					Gizmos.DrawLine(transform.position, projection);
				}
			}
		}
	}

	/// < <summary>Pushes the ball on the normalized sumatory of the identity normals SphereCasts that hitted the ball.</summary>
	/// <param name="_obj">The GameObject where the Spherecasts will be casted.</param>
	public override void PushBall(GameObject _obj)
	{
		Vector3 finalForce = Vector3.zero;

		foreach(Vector3 identity in identities)
		{
			Ray ray = new Ray(_obj.transform.position, (_obj.transform.TransformDirection(identity) * raycastLength));
			RaycastHit hit;

			Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.blue, 5f);

			if(Physics.SphereCast(ray, sphereColliderRadius, out hit, raycastLength))
			{
				finalForce += identity;
				Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.red, 10f);
				//Debug.Log("Final Force " + finalForce);
			}
		}

		rigidBody.AddForce(_obj.transform.TransformDirection(finalForce).normalized * ballForce);
	}

	void OnCollisionEnter(Collision col)
	{
		GameObject obj = col.gameObject;

		switch(obj.tag)
		{
			case Keys.PLAYER_TAG:
			PushBall(obj);
			if(onBallCollidedWithPlayer != null) onBallCollidedWithPlayer(obj.GetComponent<PlayerModel>().team);
			break;

			default:
			break;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;

		switch(obj.tag)
		{
			case "ScenarioBoundaries":
			if(onBallLeft != null) onBallLeft(false);
			break;

			default:
			break;
		}
	}

	void OnTriggerExit(Collider col)
	{
		GameObject obj = col.gameObject;

		switch(obj.tag)
		{
			case "ScenarioBoundaries":
			if(onBallLeft != null) onBallLeft(true);
			break;

			default:
			break;
		}
	}
}
}