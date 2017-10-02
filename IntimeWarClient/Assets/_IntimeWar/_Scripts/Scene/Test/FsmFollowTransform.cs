using UnityEngine;

namespace MechSquad
{
	public class FsmFollowTransform : MonoBehaviour
	{
		[SerializeField]
		Transform _followTarget;
		[SerializeField]
		Vector3 _offset;

		private void Update()
		{
			transform.position = _followTarget.position + _offset;
		}
	}
}