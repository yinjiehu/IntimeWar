using UnityEngine;
using System.Collections.Generic;
using System;

namespace Haruna.Task
{
	public class TaskManager : MonoBehaviour
	{
		static TaskManager _instance;
		public static TaskManager Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				var go = new GameObject("Scheduler");
				_instance = go.AddComponent<TaskManager>();
				DontDestroyOnLoad(go);

				return _instance;
			}
		}
		
		void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		HashSet<TaskBase> _actionList = new HashSet<TaskBase>();
		HashSet<TaskBase> _tempActionList = new HashSet<TaskBase>();
		
		void Update()
		{			
			_actionList.RemoveWhere(a => a.Complete);
			_actionList.UnionWith(_tempActionList);
			_tempActionList.Clear();

			var itr = _actionList.GetEnumerator();
			while (itr.MoveNext())
			{
				var action = itr.Current;
				if (!action.Paused)
				{
					action.Execute();
				}
			}
		}

		public void RunAction(TaskBase action)
		{
			_tempActionList.Add(action);
		}

		public static CallFuncTask Schedule(Action function, 
				float delaySec = 0, float interval = 0, int repeatTimes = int.MaxValue)
		{
			var action = new CallFuncTask(function) { DelaySec = delaySec, Interval = interval, Repeat = repeatTimes };
			if (repeatTimes == int.MaxValue)
				action.AllowCallerDestroy = true;
			Instance.RunAction(action);
			return action;
		}
		public static CallFuncTask Schedule(Action<CallFuncTask.CallFuncArgs> function,
				float delaySec = 0, float interval = 0, int repeatTimes = int.MaxValue)
		{
			var action = new CallFuncTask(function) { DelaySec = delaySec, Interval = interval, Repeat = repeatTimes };
			if (repeatTimes == int.MaxValue)
				action.AllowCallerDestroy = true;
			Instance.RunAction(action);
			return action;
		}
		public static CallFuncTask ScheduleOnce(Action function, float delaySec = 0)
		{
			var action = new CallFuncTask(function) { DelaySec = delaySec };
			Instance.RunAction(action);
			return action;
		}
	}	
}