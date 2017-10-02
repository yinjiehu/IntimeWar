using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MechSquad.Battle
{
	public class CollisionEventTrigger : Ability
	{
		List<Collider> _colliders;
		public List<Collider> Colliders { get { return _colliders; } }

		[SerializeField]
		string _tagName;
		public string TagName { get { return _tagName; } }

		public event Action<Collision> EvOnCollisionEnter;
		public event Action<Collider> EvOnTriggerEnter;

		public override void Init()
		{
			base.Init();
			_colliders = GetComponents<Collider>().ToList();
		}

		public void EnableAllColliders()
		{
			for (var i = 0; i < _colliders.Count; i++)
			{
				_colliders[i].enabled = true;
			}
		}
		public void DisableAllColliders()
		{
			for (var i = 0; i < _colliders.Count; i++)
			{
				_colliders[i].enabled = false;
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			if (EvOnCollisionEnter != null)
				EvOnCollisionEnter(collision);
		}
		void OnTriggerEnter(Collider other)
		{
			if (EvOnTriggerEnter != null)
				EvOnTriggerEnter(other);
		}
	}
}