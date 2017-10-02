using System;
using System.Collections.Generic;

namespace MechSquad.Battle
{
	public class DataMixerBool
	{
		public event Action<bool> EvOnValueChange;
		bool _previousValue;

		int _sequence = 0;
		Dictionary<int, Control> _values = new Dictionary<int, Control>();

		public Control CreateControl(bool value)
		{
			var seq = _sequence++;
			var control = new Control(this, seq);
			_values.Add(seq, control);

			CheckAndTriggerEvent();

			return control;
		}

		public void RemoveControl(int seq)
		{
			_values.Remove(seq);
			CheckAndTriggerEvent();
		}

		public Control GetControl(int seq)
		{
			Control ret;
			if(_values.TryGetValue(seq, out ret))
			{
				return ret;
			}
			throw new Exception("Can not find control " + seq);
		}

		public void CheckAndTriggerEvent()
		{
			var value = true;
			using(var itr = _values.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					if (!itr.Current.Value.Value)
						value = false;
				}
			}

			if (value != _previousValue && EvOnValueChange != null)
			{
				EvOnValueChange(value);
				_previousValue = value;
			}
		}

		public bool GetValue()
		{
			return _previousValue;
		}

		public static implicit operator bool(DataMixerBool s)
		{
			return s.GetValue();
		}

		public class Control
		{
			int _sequenceNo;
			public int SequencNo { get { return _sequenceNo; } }
			DataMixerBool _holder;
			public DataMixerBool Holder { get { return _holder; } }

			public Control(DataMixerBool holder, int seq)
			{
				_holder = holder;
				_sequenceNo = seq;
			}

			bool _value;
			public bool Value
			{
				set
				{
					_value = value;
					Holder.CheckAndTriggerEvent();
				}
				get { return _value; }
			}

			public void RemoveSelf()
			{
				Holder.RemoveControl(SequencNo);
			}
		}
	}

	[Serializable]
	public class SimpleDataMixerBool
	{
		public event Action<bool> EvOnValueChange;

		bool _value;
		public bool Value
		{
			set
			{
				if (_value != value && EvOnValueChange != null)
					EvOnValueChange(value);
				_value = value;
			}
			get { return _value; }
		}

		public static implicit operator bool(SimpleDataMixerBool s)
		{
			return s.Value;
		}
	}

}