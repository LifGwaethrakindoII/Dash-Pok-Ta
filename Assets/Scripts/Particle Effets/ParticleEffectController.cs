using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;

public class ParticleEffectController : MonoBehaviour
{
	[SerializeField] private List<ParticleSystem> _particleSystems; 	/// <summary>Particle Systems attached.</summary>
	[SerializeField] private float scaleInTime;
	[SerializeField] private float scaleOutTime;
	[SerializeField] private float fadeInTime; 							/// <summary>Fade In Time duration.</summary>
	[SerializeField] private float fadeOutTime; 						/// <summary>Fade Out Time duration.</summary>
	private Behavior particleEffectOutScaler;
	private Behavior particleEffectInScaler;
	private Behavior particlesOutFader; 								/// <summary>FadeOutParticleSystems coroutine controller.</summary>
	private Behavior particlesInFader; 									/// <summary>FadeInParticleSystems coroutine controller.</summary>

	/// <summary>Gets and Sets particleSystems property.</summary>
	public List<ParticleSystem> particleSystems
	{
		get { return _particleSystems; }
		set { _particleSystems = value; }
	}

	/// <summary>ParticleEffectSystem's' instance initialization.</summary>
	void Awake()
	{
		if(particleSystems != null || !particleSystems.ListFull<ParticleSystem>())
		{
			particleSystems = new List<ParticleSystem>();
			particleSystems = transform.GetComponentsFromChilds<ParticleSystem>();
		}
	}

#region PublicMethods:
	/// <summary>Emits Particle Systems.</summary>
	/*public void Emit()
	{
		for(int i = 0; i < particleSystems.Count; i++)
		{
			particleSystems[i].Emit();	
		}
	}*/

	/// <summary>Plays Particle Systems.</summary>
	public void Play()
	{
		for(int i = 0; i < particleSystems.Count; i++)
		{
			particleSystems[i].Play();	
		}
	}

	/// <summary>Pauses Particle Systems.</summary>
	public void Pause()
	{
		for(int i = 0; i < particleSystems.Count; i++)
		{
			particleSystems[i].Pause();	
		}
	}

	/// <summary>Clears Particle Systems.</summary>
	public void Clear()
	{
		for(int i = 0; i < particleSystems.Count; i++)
		{
			particleSystems[i].Clear();	
		}
	}

	public void ScaleInEffect(Action onParticleScaleEnds)
	{
		if(particleEffectInScaler != null) particleEffectInScaler.EndBehavior();
		particleEffectInScaler = new Behavior(this, ScaleInParticleSystems(onParticleScaleEnds));
	}

	public void ScaleOutEffect(Action onParticleScaleEnds)
	{
		if(particleEffectOutScaler != null) particleEffectOutScaler.EndBehavior();
		particleEffectOutScaler = new Behavior(this, ScaleOutParticleSystems(onParticleScaleEnds));
	}

	public void FadeOutParticles(Action onParticlesFadeEnds)
	{
		if(particlesOutFader != null) particlesOutFader.EndBehavior();
		particlesOutFader = new Behavior(this, FadeOutParticleSystems(onParticlesFadeEnds));
	}

	public void FadeInParticles(Action onParticlesFadeEnds)
	{
		if(particlesInFader != null) particlesInFader.EndBehavior();
		particlesInFader = new Behavior(this, FadeInParticleSystems(onParticlesFadeEnds));
	}
#endregion

#region PrivateMethods:
	private IEnumerator ScaleOutParticleSystems(Action onParticleScaleEnds)
	{
		float normalizedTime = 1.0f;

		while(normalizedTime > 0.0f)
		{
			transform.localScale = VoidlessVector3.RegularVector3(normalizedTime);
			normalizedTime -= (Time.deltaTime / scaleOutTime);
			yield return null;
		}

		transform.localScale = Vector3.zero;
		if(onParticleScaleEnds != null) onParticleScaleEnds();
	}

	private IEnumerator ScaleInParticleSystems(Action onParticleScaleEnds)
	{
		float normalizedTime = 0.0f;

		while(normalizedTime < 1.0f)
		{
			transform.localScale = VoidlessVector3.RegularVector3(normalizedTime);
			normalizedTime += (Time.deltaTime / scaleInTime);
			yield return null;
		}

		transform.localScale = Vector3.one;
		if(onParticleScaleEnds != null) onParticleScaleEnds();
	}

	private IEnumerator FadeOutParticleSystems(Action onParticlesFadeEnds)
	{
		float normalizedTime = 1.0f;

		for(int i = 0; i < particleSystems.Count; i++)
		{
			normalizedTime = 1.0f;
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystems[i].particleCount];
			particleSystems[i].GetParticles(particles);

			while(normalizedTime > 0.0f)
			{
				for(int j = 0; j < particles.Length; j++)
				{
					byte alpha;
					VoidlessMath.FloatToColor32Byte(out alpha, normalizedTime);
					particles[j].color = particles[j].color.SetAlpha(alpha);	
				}

				normalizedTime -= (Time.deltaTime / fadeOutTime);
				yield return null;
			}
		}	

		if(onParticlesFadeEnds != null) onParticlesFadeEnds();
	}

	private IEnumerator FadeInParticleSystems(Action onParticlesFadeEnds)
	{
		float normalizedTime = 0.0f;

		for(int i = 0; i < particleSystems.Count; i++)
		{
			normalizedTime = 0.0f;
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystems[i].particleCount];
			particleSystems[i].GetParticles(particles);

			while(normalizedTime < 1.0f)
			{
				for(int j = 0; j < particles.Length; j++)
				{
					byte alpha;
					VoidlessMath.FloatToColor32Byte(out alpha, normalizedTime);
					particles[j].color = particles[j].color.SetAlpha(alpha);	
				}

				normalizedTime += (Time.deltaTime / fadeInTime);
				yield return null;
			}
		}	

		if(onParticlesFadeEnds != null) onParticlesFadeEnds();
	}
#endregion
}