using HutongGames.PlayMaker;
using MechSquad.View;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MechSquad.Fsm
{
	[ActionCategory("MechSquad_Common")]
	public class WaitTweenerComplete : FsmStateAction
	{
		public WhiteCat.Tween.Tweener _tweener;

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!_tweener.enabled)
				Finish();
		}
	}
}