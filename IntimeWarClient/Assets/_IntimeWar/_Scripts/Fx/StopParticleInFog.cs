
using Haruna.Pool;
using System;
using UnityEngine;

namespace MechSquad
{
	public class StopParticleInFog : PoolElement
	{
		[SerializeField]
		[Range(0.0f, 1.0f)]
		float _minFogStrength = 0.2f;

		ParticleSystem _rootParticle;

		private void Awake()
		{
			_rootParticle = GetComponentInChildren<ParticleSystem>();
		}

		private void Update()
		{
			//if (FogOfWar.current.IsInFog(transform.position, _minFogStrength))
			//{
			//	if (_rootParticle.isPlaying)
			//		_rootParticle.Stop();
			//}
			//else
			//{
			//	if (_rootParticle.isStopped)
			//		_rootParticle.Play();
			//}
		}
	}
}
