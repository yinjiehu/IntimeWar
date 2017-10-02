using UnityEngine;

namespace MechSquad.Battle
{
	public class UnitStatusManager : MonoBehaviour
	{
		public SimpleDataMixerBool IsDead = new SimpleDataMixerBool();
		public DataMixerBool BodyVisible = new DataMixerBool();
		public SimpleDataMixerBool EventReceiverEnable = new SimpleDataMixerBool();
		public SimpleDataMixerBool BodyStrikeEnable = new SimpleDataMixerBool();
		public SimpleDataMixerBool ProvideSquadSight = new SimpleDataMixerBool();
		public SimpleDataMixerBool RaidarEnable = new SimpleDataMixerBool();
		public SimpleDataMixerBool DisplayInRaidar = new SimpleDataMixerBool();
		public SimpleDataMixerBool CanBeLockedOn = new SimpleDataMixerBool();
		public SimpleDataMixerBool AutoFireControlAvaliable = new SimpleDataMixerBool();
		public SimpleDataMixerBool TurretLineVisible = new SimpleDataMixerBool();
		
		public DataMixerFloat SightPercentModify = new DataMixerFloat();
	}
}