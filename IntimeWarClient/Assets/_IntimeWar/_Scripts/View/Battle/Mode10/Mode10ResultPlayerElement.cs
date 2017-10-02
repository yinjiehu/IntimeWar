using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class Mode10ResultPlayerElement : MonoBehaviour
	{
		[SerializeField]
		GameObject _selfMark;
		[SerializeField]
		Text _name;
		[SerializeField]
		Text _kill;
		[SerializeField]
		Text _death;

		public struct Model
		{
			public bool Self;
			public string NickName;
			public string KillCount;
			public string DeathCount;
		}

		public void BindModel(Model data)
		{
			_selfMark.SetActive(data.Self);
			_name.text = data.NickName;
			_kill.text = data.KillCount.ToString();
			_death.text = data.DeathCount.ToString();
		}
	}
}