using HutongGames.PlayMaker;
using MechSquad.Battle;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Unit")]
	public class DestroyUnitOnPlayerLeave : Photon.PunBehaviour
	{
		public override void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
		{
			base.OnPhotonPlayerActivityChanged(otherPlayer);

			if (otherPlayer.IsInactive)
			{
				Debug.LogFormat("player {0} {1} inactive", otherPlayer.ID, otherPlayer.NickName);
			}
			else
			{
				Debug.LogFormat("player {0} {1} return to battle", otherPlayer.ID, otherPlayer.NickName);
			}

		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			base.OnPhotonPlayerDisconnected(otherPlayer);

			Debug.LogFormat("Player {0} {1} leave", otherPlayer.ID, otherPlayer.NickName);
			//var units = UnitManager.Instance.AllUnits.Where(u => u.View != null && u.View.CreatorActorNr == otherPlayer.ID);
			var views = PhotonNetwork.networkingPeer.photonViewList.Where(kv => kv.Value.CreatorActorNr == otherPlayer.ID);

			foreach (var v in views)
			{
				var u = v.Value.GetComponent<BattleUnit>();
				if (u == null)
					continue;

				if (u.IsDead)
				{
					u.OnUnitDestroy();
					Destroy(u);
				}
				else
				{
					UnitManager.Instance.DestroyUnitItsSelf(u, true);
				}
			}
		}
	}
}