using MechSquad.Battle;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MechSquad.View
{
	public class EmergencyButtonView : BaseView
	{
		[SerializeField]
		Button _button01;

		[SerializeField]
		Button _back;
		public override void Init()
		{
			base.Init();

			_button01.onClick.AddListener(()=>
			{
				var player = MechSquad.Battle.UnitManager.ThisClientPlayerUnit;
				if (player != null)
					player.Model.position = player.Model.position.ChangeY();
			});

			StartCoroutine(CheckPlayerInstatiate());
		}

		bool _showing;

		IEnumerator CheckPlayerInstatiate()
		{
			yield return new WaitForSeconds(3);

			Show();

			while (UnitManager.ThisClientPlayerUnit == null || UnitManager.ThisClientPlayerUnit.Model.position.y > 30)
				yield return new WaitForSeconds(0.1f);

			Hide();
		}


		public override void Show()
		{
			if (!_showing)
			{
				base.Show();
				_showing = true;
			}
		}
	}
}
