using UnityEngine;
using System;
using System.Collections;

namespace Haruna.Task
{
	public abstract class TaskBase
	{
		protected bool _complete = false;
		public bool Complete { get { return _complete; } }

		protected bool _paused = false;
		public bool Paused { get { return _paused; } set { _paused = value; } }

		protected TaskBase()
		{
		}
		
		// Update is called once per frame
		public virtual void Execute()
		{
			_complete = true;
		}

		public virtual void Stop()
		{
			_complete = true;
		}

		public virtual void Pause()
		{
			_paused = true;
		}

		public virtual void Resume()
		{
			_paused = false;
		}

		public virtual void Run()
		{
			TaskManager.Instance.RunAction(this);
		}
	}
}