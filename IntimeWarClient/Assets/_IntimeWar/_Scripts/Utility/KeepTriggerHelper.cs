using System;


namespace MechSquad
{
	public class KeepTriggerHelper
	{
		public float KeepMinInterval;
		public Action OnStop;
		public Action OnKeep;

		bool _enabled;
		float _elapsedTime;

		public void Keep()
		{
			_elapsedTime = 0;
			_enabled = true;

			if (OnKeep != null)
				OnKeep();
		}

		public void Update(float deltaTime)
		{
			if (_enabled)
			{
				_elapsedTime += deltaTime;
				if (_elapsedTime > KeepMinInterval)
				{
					if (OnStop != null)
						OnStop();

					Stop();
				}
			}
		}

		public void Stop()
		{
			_enabled = false;
			_elapsedTime = 0;
		}
	}
}
