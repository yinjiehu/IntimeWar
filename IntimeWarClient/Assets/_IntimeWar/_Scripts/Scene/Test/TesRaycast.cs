using MechSquad.Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TesRaycast : MonoBehaviour
{
	[SerializeField]
	Haruna.Inspector.InspectorButton _rrrr;

	[SerializeField]
	LayerMask _layer;

	[SerializeField]
	List<Transform> _transforms;

	[SerializeField]
	float _range;
	[SerializeField]
	float _radius = 10;
	void Rrrr()
	{
		//RaycastHit hit;
		//CollisionEventReceiver
		//MechSquad.Battle.At_Gatlin.CollideFirst(transform.position, transform.forward, _range, _layer,
		//	out )


		var hits = Physics.SphereCastAll(
			transform.position,
			_radius,
			transform.forward,
			_range,
			_layer,
			QueryTriggerInteraction.Collide)
			.OrderBy(h => Vector3.Distance(transform.position, h.point))
			.ToList();

		_transforms = hits.Select(h => h.collider.transform).ToList();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * _range);
		Gizmos.color = Color.green;
		if(_radius > 0 && _range > _radius && _range / _radius < 30)
		for(float i = 0; i <= _range; i = i + _radius * 2)
		{
			Gizmos.DrawSphere(transform.position + transform.forward * (i + _radius), _radius);
		}
	}
}
