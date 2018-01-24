using System.Linq;
using UnityEngine;

namespace YJH.Unit
{
	public abstract class FsmStateAbility : HutongGames.PlayMaker.FsmStateAction, IUnitAbility
	{
		protected BattleUnit _unit;
		protected Animator _animator;

		[SerializeField]
		bool _sendSynchronization;
		public bool IsSyncAbility { get { return _sendSynchronization; } }

		public bool IsRunning { private set; get; }

		protected string _name;
		public new string Name
		{
			get
			{
				if (string.IsNullOrEmpty(_name))
				{
					_name = string.Format("{0}$[{1}]", State.Name, State.Actions.ToList().IndexOf(this));
				}
				return _name;
			}
		}

		public virtual void SetupInstance(BattleUnit unit)
		{
			_unit = unit;
			_animator = _unit.Animator;
		}

		public virtual void Init()
		{

		}

		public virtual void LateInit()
		{
		}
	}
}