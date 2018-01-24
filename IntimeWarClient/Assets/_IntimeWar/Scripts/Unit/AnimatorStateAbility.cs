using UnityEngine;

namespace YJH.Unit
{
	public abstract class AnimatorStateAbility : StateMachineBehaviour, IUnitAbility
	{
		public virtual string Name { get { return name; } }

		protected BattleUnit _unit;
		protected Animator _animator;
		
		[SerializeField]
		bool _sendSynchronization;
		public bool IsSyncAbility { get { return _sendSynchronization; } }

		public bool IsRunning { private set; get; }
		
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

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			IsRunning = true;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			IsRunning = false;
		}
	}
}