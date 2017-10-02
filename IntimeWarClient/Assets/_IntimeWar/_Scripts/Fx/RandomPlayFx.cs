using Haruna.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad
{
	[Serializable]
	public class RandomPlayFx
	{
		[SerializeField]
		List<FxHandler> _fxList;

		public FxHandler Show()
		{
			return _fxList.RandomGet().Show();
		}

		public FxHandler Show(Vector3 position)
		{
			return _fxList.RandomGet().Show(position);
		}
		public FxHandler Show(Vector3 position, Vector3 direction)
		{
			return _fxList.RandomGet().Show(position, direction);
		}
		public FxHandler Show(Transform tran)
		{
			return _fxList.RandomGet().Show(tran);
		}

	}
}
