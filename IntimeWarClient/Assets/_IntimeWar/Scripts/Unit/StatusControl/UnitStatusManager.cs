using UnityEngine;

namespace YJH.Unit
{
	public class UnitStatusManager : MonoBehaviour
	{
		public SimpleDataMixerBool IsDead = new SimpleDataMixerBool();
        public DataMixerBool BodyVisible = new DataMixerBool();
        public SimpleDataMixerBool UIVisible = new SimpleDataMixerBool();
    }
}