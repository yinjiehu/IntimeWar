using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MechSquad.Battle
{
	public abstract class CollisionTiggerProcessor : Ability
	{
		public enum SelectTriggerTypeEnum
		{
			All,
			DirectReference,
			ByTagName
		}
		[SerializeField]
		protected SelectTriggerTypeEnum _attachTriggers;

		[SerializeField]
		protected List<string> _collisionTriggerTags;
		[SerializeField]
		protected List<CollisionEventTrigger> _collisionEventTriggers;

		protected List<CollisionEventTrigger> _actualCollisionEventTriggers = new List<CollisionEventTrigger>();

		IExtraCollisionFilter _extraFilter;

		[System.Serializable]
		public class CollisionProcessUnityEvent : UnityEngine.Events.UnityEvent<CollisionEventReceiver> { }
		[SerializeField]
		CollisionProcessUnityEvent _onTriggerEvent;

		public override void LateInit()
		{
			_extraFilter = _unit.GetComponent<IExtraCollisionFilter>();

			if (_attachTriggers == SelectTriggerTypeEnum.DirectReference)
			{
				for(var i = 0; i < _collisionEventTriggers.Count; i++)
				{
					var t = _collisionEventTriggers[i];
					t.EvOnTriggerEnter += OnUnitTriggerEnter;
					t.EvOnCollisionEnter += OnUnitCollisionEnter;

					_actualCollisionEventTriggers.Add(t);
				}
			}
			else
			{
				var transfers = _unit.GetAllAbilities<CollisionEventTrigger>();
				var itr = transfers.GetEnumerator();
				while (itr.MoveNext())
				{
					var t = itr.Current;
					if (_attachTriggers == SelectTriggerTypeEnum.All || _collisionTriggerTags.Contains(t.TagName))
					{
						t.EvOnTriggerEnter += OnUnitTriggerEnter;
						t.EvOnCollisionEnter += OnUnitCollisionEnter;

						_actualCollisionEventTriggers.Add(t);
					}
				}
			}
		}

		protected virtual void OnUnitCollisionEnter(Collision collision)
		{
			var target = collision.collider.GetComponent<CollisionEventReceiver>();
			if (target != null)
			{
				if (_extraFilter == null || _extraFilter.IsTaget(target))
				{
					OnHitTarget(target, collision);
				}
			}
		}

		protected virtual void OnUnitTriggerEnter(Collider other)
		{
			var receiver = other.GetComponent<CollisionEventReceiver>();
			if (receiver != null)
			{
				if (_extraFilter == null || _extraFilter.IsTaget(receiver))
				{
					OnHitTarget(receiver, null);
				}
			}
		}
		void OnHitTarget(CollisionEventReceiver targetUnit, Collision collision)
		{
			ProcessCollisionEvent(targetUnit, collision);
			if (_onTriggerEvent != null)
				_onTriggerEvent.Invoke(targetUnit);
		}

		public abstract void ProcessCollisionEvent(CollisionEventReceiver otherUnit, Collision collision);

#if UNITY_EDITOR
		protected virtual void Editor_OnInspectorGUI(SerializedObject serializedObject)
		{
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_attachTriggers"));
			if (_attachTriggers == SelectTriggerTypeEnum.ByTagName)
			{
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_collisionTriggerTags"), true);
			}
			else if (_attachTriggers == SelectTriggerTypeEnum.DirectReference)
			{
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_collisionEventTriggers"), true);
			}
			EditorGUI.indentLevel = 0;

			EditorGUILayout.Space();

			var property = serializedObject.FindProperty("_onTriggerEvent");
			EditorGUILayout.PropertyField(property, true);
			while (property.NextVisible(false))
			{
				EditorGUILayout.PropertyField(property, true);
			}
		}

		[CustomEditor(typeof(CollisionTiggerProcessor), true)]
		protected class CollisionTiggerProcessorEditor : Editor
		{
			public override void OnInspectorGUI()
			{
				((CollisionTiggerProcessor)target).Editor_OnInspectorGUI(serializedObject);
				serializedObject.ApplyModifiedProperties();
			}
		}
#endif

	}
}
