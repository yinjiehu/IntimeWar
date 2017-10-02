using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MechSquad.View
{
	public class Mode10CurrentPlayerInfoView : BaseView
	{
		[SerializeField]
		Haruna.UI.ListMemberUpdater _ally;
		[SerializeField]
		Haruna.UI.ListMemberUpdater _enemy;

		public struct Model
		{
			public List<Mode10CurrentPlayerInfoElement.Model> Allies;
			public List<Mode10CurrentPlayerInfoElement.Model> Enemies;
		}

		public override void Show()
		{
			base.Show();
			
			var data = Mode10Presenter.GetPlayersInfo();
			BindModel(data);
		}

		float _elpasedTime;
		float _updateDuration = 2f;
		private void Update()
		{
			_elpasedTime += Time.deltaTime;
			if(_elpasedTime > _updateDuration)
			{
				_elpasedTime = 0;

				var data = Mode10Presenter.GetPlayersInfo();
				BindModel(data);
			}
		}

		public void BindModel(Model data)
		{
			_ally.OnListUpdate(data.Allies, (d, go, index) =>
			{
				go.GetComponent<Mode10CurrentPlayerInfoElement>().BindModel(d);
			});

			_enemy.OnListUpdate(data.Enemies, (d, go, index) =>
			{
				go.GetComponent<Mode10CurrentPlayerInfoElement>().BindModel(d);
			});
		}
	}
}