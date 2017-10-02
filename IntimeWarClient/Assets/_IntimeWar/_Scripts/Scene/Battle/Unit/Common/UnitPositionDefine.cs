using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MechSquad.Battle
{
	public class UnitPositionDefine : Ability
	{
		[SerializeField]
		Transform _top;
		public Transform Top { get { return _top; } }

		[SerializeField]
		Transform _head;
		public Transform Head { get { return _head; } }

		[SerializeField]
		Transform _mainFireControl;
		public Transform MainFireControl { get { return _mainFireControl; } }
		[SerializeField]
		Transform _leftHandAttachment;
		public Transform LeftHandAttachment { get { return _leftHandAttachment; } }
		[SerializeField]
		Transform _rightHandAttachment;
		public Transform RightHandAttachment { get { return _rightHandAttachment; } }
		[SerializeField]
		Transform _leftShoulderAttachment;
		public Transform LeftShoulderAttachment { get { return _leftShoulderAttachment; } }
		[SerializeField]
		Transform _rightShoulderAttachment;
		public Transform RightShoulderAttachment { get { return _rightShoulderAttachment; } }
		[SerializeField]
		Transform _leftFoot;
		public Transform LeftFoot { get { return _leftFoot; } }
		[SerializeField]
		Transform _rifhtFoot;
		public Transform RightFoot { get { return _rifhtFoot; } }

		[Serializable]
		public class Define
		{
			public string Name;
			public Transform Position;
		}
		[SerializeField]
		List<Define> _positionDefine;

		public Transform GetPosition(string pName)
		{
			switch (pName)
			{
				case SlotID.LHand:
					return LeftHandAttachment;
				case SlotID.RHand:
					return RightHandAttachment;
				case SlotID.LShoulder:
					return LeftShoulderAttachment;
				case SlotID.RShoulder:
					return RightShoulderAttachment;
			}

			var define = _positionDefine.Find(p => p.Name == pName);
			if(define == null)
			{
				Debug.LogErrorFormat(this, "can not find position named {0}", pName);
				return null;
			}
			return define.Position;
		}
	}
}