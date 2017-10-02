using MechSquad.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad
{
	public class AudioListenerFollow : MonoBehaviour
	{
		Deftly.DeftlyCamera _dfCamera;

		BattleUnit _playerUnit;

		private void Start()
		{
			_dfCamera = Deftly.DeftlyCamera.Get();
		}

		private void Update()
		{
			GetPlayerUnit();
			if (_playerUnit == null)
			{
				transform.position = _dfCamera.transform.position;
			}
			else
			{
				var p = _playerUnit.Model.position;
				p.y = 0;
				transform.position = p;
			}
		}

		void GetPlayerUnit()
		{
			if(_playerUnit == null)
				_playerUnit = UnitManager.ThisClientPlayerUnit;
		}
	}
}
