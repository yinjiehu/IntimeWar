using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.View;
using UnityEngine.UI;

namespace MechSquad.Battle
{
	public class StepFx : MonoBehaviour
	{
		[SerializeField]
		FxHandler _dirtFx;
		[SerializeField]
		FxHandler _waterFx;
		[SerializeField]
		SePlayer _dirtSe;
		[SerializeField]
		SePlayer _waterSe;

		float _elapsedTimeSinceLastFx;
		[SerializeField]
		float _fxMinInterval = 0.1f;
		private void Update()
		{
			_elapsedTimeSinceLastFx += Time.deltaTime;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (_elapsedTimeSinceLastFx > _fxMinInterval)
			{
				var layerName = LayerMask.LayerToName(other.gameObject.layer);
				if (layerName == "Surface_Dirt")
				{
					_dirtFx.Show(transform.position, transform.forward);
					_dirtSe.Play(transform.position);
				}
				else if (layerName == "Surface_Water")
				{
					_waterFx.Show(transform.position, transform.forward);
					_waterSe.Play(transform.position);
				}
				_elapsedTimeSinceLastFx = 0;
			}
		}
	}
}