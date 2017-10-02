using UnityEngine;
using System.Collections;
namespace Haruna.Utility
{
	public class LineMatchTarget : MonoBehaviour
	{
		GameObject _target = null;

		[SerializeField]
		QuadMesh _quadMesh;
		[SerializeField]
		float _toCenterLength;

		[SerializeField]
		float _defaultLength = 3;
		[SerializeField]
		bool _hideWhenNoTarget;
		[SerializeField]
		bool _updateDirection;

		public void SetTarget(GameObject target)
		{
			_target = target;
		}

		void Update()
		{
			if (_target != null)
			{
				_quadMesh.gameObject.SetActive(true);

				if (_updateDirection)
				{
					var diretion = _target.transform.position - transform.position;
					diretion.y = 0;
					transform.forward = diretion;
				}

				float distance = Vector3.Distance(transform.position, _target.transform.position);
				_quadMesh._uvRatioWidth = (distance - _toCenterLength) / _quadMesh._width;
			}
			else
			{
				if (_hideWhenNoTarget)
					_quadMesh.gameObject.SetActive(false);
				else
					_quadMesh._uvRatioWidth = _defaultLength;
			}
		}
	}
}