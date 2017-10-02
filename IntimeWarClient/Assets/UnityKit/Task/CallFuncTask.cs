using UnityEngine;
using System;
using System.Collections;

namespace Haruna.Task
{
	public class CallFuncTask : TaskBase
	{
		public class CallFuncArgs
		{
			CallFuncTask _task;

			public CallFuncArgs(CallFuncTask task)
			{
				_task = task;
			}

			public float DeltaTime { internal protected set; get; }

			public CallFuncTask Task { get { return _task; } }
		}

		Action _function;
		Action<CallFuncArgs> _functionWithArgs;
		CallFuncArgs _args;

		bool _isStaticMethod;
		bool _isUnityCaller;
		object _caller;
		UnityEngine.Object _unityCaller;
		string _name;
		Type _callerType;

		public int Repeat { set; get; }

		public float DelaySec { set; get; }

		public float Interval { set; get; }

		public bool AllowCallerDestroy { set; get; }

		public bool IgnorTimeScale { set; get; }

		public Func<float> CustomTimer { set; get; }

		protected int _calledTimes;
		protected float _nextInterval;
		protected float _elapsedTime;

		protected bool _firstTime = true;

		public CallFuncTask(Action function)
		{
			if (function == null)
				throw new UnityException("function should not be null!");
			
			_function = function;
			SetCaller(_function);
		}

		public CallFuncTask(Action<CallFuncArgs> function)
		{
			if (function == null)
				throw new UnityException("function should not be null!");

			_functionWithArgs = function;
			_args = new CallFuncArgs(this);
			SetCaller(_functionWithArgs);			
		}

		void SetCaller(Delegate function)
		{
			_isStaticMethod = function.Method.IsStatic;
			if (!_isStaticMethod)
			{
				_isUnityCaller = function.Target is UnityEngine.Object;
				if (_isUnityCaller)
				{
					_unityCaller = function.Target as UnityEngine.Object;
					_callerType = _unityCaller.GetType();
					_name = _unityCaller.name;
				}
				else
				{
					_caller = function.Target;
					_callerType = _caller.GetType();
				}
			}
		}

		protected bool IsCallerNull()
		{
			if (_isStaticMethod)
				return false;

			if (_isUnityCaller)
				return _unityCaller == null;

			return _caller == null;
		}

		public override void Execute()
		{
			if (_firstTime)
			{
				_nextInterval = DelaySec;
				_firstTime = false;
			}

			if (CustomTimer == null)
			{
				if (IgnorTimeScale)
					_elapsedTime += Time.unscaledDeltaTime;
				else
					_elapsedTime += Time.deltaTime;
			}
			else
				_elapsedTime += CustomTimer();

			if (IsCallerNull())
			{
				if (!AllowCallerDestroy)
				{
					Debug.LogErrorFormat("Schedule target has been destoryed! \t [{0}.{1}()] \t [{2}] ",
						_callerType.Name, _function.Method.Name, _name);
				}
				_complete = true;
			}
			else if (_elapsedTime > _nextInterval)
			{
				try
				{
					if (_args == null)
						_function();
					else
					{
						_args.DeltaTime = _elapsedTime;
						_functionWithArgs(_args);
					}
				}
				catch (Exception e)
				{
					if (_unityCaller != null)
						Debug.LogException(e, _unityCaller);
					else
						Debug.LogException(e);
				}

				_calledTimes++;
				_nextInterval = Interval - (_elapsedTime - _nextInterval);
				_elapsedTime = 0;

				if (_calledTimes >= Repeat)
				{
					_complete = true;
				}
			}
		}
	}
}