using MechSquad.Battle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class Mode10CurrentPlayerInfoElement : MonoBehaviour
	{
		public struct Model
		{
			public int ActorID;
			public string DisplayText;
			public bool ShowDeadFlag;
		}

		[SerializeField]
		Text _nameText;
		[SerializeField]
		Image _background;
		[SerializeField]
		GameObject _deadFlag;

		int _actorID;

		BattleUnit _unit;

		public void BindModel(Model data)
		{
			enabled = true;

			_unit = null;
			_actorID = data.ActorID;
			_nameText.text = data.DisplayText;
			_deadFlag.SetActive(data.ShowDeadFlag);
		}


		//private void Update()
		//{
		//	if (_unit == null)
		//	{
		//		_unit = UnitManager.Instance.GetPlayerUnitByActorID(_actorID);
		//	}

		//	if (_unit != null)
		//	{
		//		if (_unit.IsDead)
		//		{
		//			_deadFlag.SetActive(true);
		//			enabled = false;
		//		}
		//	}
		//}
	}
}