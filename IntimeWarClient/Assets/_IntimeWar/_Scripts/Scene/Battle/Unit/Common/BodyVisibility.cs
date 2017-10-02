using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using MechSquad.Event;
using UnityEngine.Events;
using MechSquad.View;

namespace MechSquad.Battle
{
	public class BodyVisibility : Ability
	{
		Renderer[] _renderers;
		Collider[] _colliders;
		
		public override void Init()
		{
			_renderers = _unit.Model.GetComponentsInChildren<Renderer>();
			_colliders = _unit.Model.GetComponentsInChildren<Collider>();

			SetRendererAndColliderEnable(false);
			_unit.STS.BodyVisible.EvOnValueChange += OnChange;
		}

		private void OnChange(bool visibility)
		{
			SetRendererAndColliderEnable(visibility);
		}
		
		void SetRendererAndColliderEnable(bool b)
		{
			for (var i = 0; i < _renderers.Length; i++)
			{
				_renderers[i].enabled = b;
			}
			
			for (var i = 0; i < _colliders.Length; i++)
			{
				_colliders[i].enabled = b;
			}
		}
	}
}