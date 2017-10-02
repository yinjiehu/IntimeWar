using UnityEngine;

namespace MechSquad.Battle
{
	public class MechSquadBattleCameraShaker : MonoBehaviour
	{
		static MechSquadBattleCameraShaker _instance;
		public static MechSquadBattleCameraShaker Instance { get { return _instance; } }

		private void Awake()
		{
			_instance = this;
		}

		public static void ShakeCameraOnce(int level, Vector3 shakePosition)
		{
			var distance = Vector3.Distance(shakePosition, Instance.transform.position);
			ShakeCameraOnce(level, distance);
		}
		public static void ShakeCameraOnce(int level, float distance)
		{
			var p = CameraParameter.CameraShakeStrengthLevel[level];
			//EZCameraShake.CameraShaker.Instance.ShakeOnce(p.StrengthMagnitude * p.DistanceDecay.Evaluate(distance), 
			//	p.Roughness, p.FadeinSeconds, p.FadeOutSeconds);
		}
	}
}