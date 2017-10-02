using UnityEngine;

namespace MechSquad.Battle
{
	public class BattleScene : MonoBehaviour
	{
		static BattleScene _instance;
		public static BattleScene Instance { get { return _instance; } }

		AudioListener _audioListener;
		
		private void Awake()
		{
			_instance = this;
			_instance._audioListener = FindObjectOfType<AudioListener>();
		}

		
		public void AddKillCount(int attackerActorID, int receiverActorID)
		{
			if(PhotonNetwork.player.ID == attackerActorID)
			{
				RPCAddKillCount(attackerActorID);
			}
			else
			{
				var attackerPlayer = PhotonHelper.GetPlayer(attackerActorID);
				this.GetPhotonView().RPC("RPCAddKillCount", PhotonTargets.Others, attackerActorID);
			}
			if(PhotonNetwork.player.ID == receiverActorID)
			{
				RPCAddDeathCount(receiverActorID);
			}
			else
			{
				var p = PhotonHelper.GetPlayer(receiverActorID);
				this.GetPhotonView().RPC("RPCAddDeathCount", PhotonTargets.Others, receiverActorID);
			}
		}

		[PunRPC]
		void RPCAddKillCount(int attackerActorID)
		{
			if(PhotonNetwork.player.ID == attackerActorID)
				PhotonHelper.AddKillCount();
		}

		[PunRPC]
		void RPCAddDeathCount(int receiverActorID)
		{
			if (PhotonNetwork.player.ID == receiverActorID)
				PhotonHelper.AddDeathCount();
		}

		public void PauseAllUnit()
		{
			var allUnits = UnitManager.Instance.AllUnits;
			using (var itr = allUnits.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					if(!itr.Current.IsDead)
						itr.Current.Pause();
				}
			}
		}
	}
}