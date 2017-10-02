using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MechSquad.View
{
	public class Mode10ResultView : BaseView
	{
		[SerializeField]
		Text _totalAlly;
		[SerializeField]
		Text _totalEnemy;

		[SerializeField]
		Haruna.UI.ListMemberUpdater _ally;
		[SerializeField]
		Haruna.UI.ListMemberUpdater _enemy;
		
		float _updateDuration = 0.5f;
		float _elapasedTime;

		public struct Model
		{
			public int TotalKillCountAlly;
			public int TotalKillCountEnemy;

			public List<Mode10ResultPlayerElement.Model> Allies;
			public List<Mode10ResultPlayerElement.Model> Enemies;
		}

		public override void Show()
		{
			base.Show();
			
			var data = Mode10Presenter.GetBattleResultInfo();
			BindModel(data);
		}

		private void Update()
		{
			_elapasedTime += Time.deltaTime;
			if(_elapasedTime > _updateDuration)
			{
				_elapasedTime = 0;

				var data = Mode10Presenter.GetBattleResultInfo();
				BindModel(data);
			}
		}

		public void BindModel(Model data)
		{
			_totalAlly.text = data.TotalKillCountAlly.ToString();
			_totalEnemy.text = data.TotalKillCountEnemy.ToString();
			_ally.OnListUpdate(data.Allies, (d, go, index) =>
			{
				go.GetComponent<Mode10ResultPlayerElement>().BindModel(d);
			});

			_enemy.OnListUpdate(data.Enemies, (d, go, index) =>
			{
				go.GetComponent<Mode10ResultPlayerElement>().BindModel(d);
			});
		}
	}
}