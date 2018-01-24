using System;
using System.Collections.Generic;

namespace YJH.Unit
{
	public class DataMixerFloat
	{
		public event Action<float> EvOnValueChange;
		float _previousValue;

		int _sequence = 0;
		Dictionary<int, Control> _values = new Dictionary<int, Control>();

		public Control CreateControl(float value)
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
			var value = 0f;
			using(var itr = _values.GetEnumerator())
			{
				while (itr.MoveNext())
				{
					value += itr.Current.Value.Value;
				}
			}

			if (value != _previousValue && EvOnValueChange != null)
			{
				EvOnValueChange(value);
				_previousValue = value;
			}
		}

		public float GetValue()
		{
			return _previousValue;
		}

		public static implicit operator float(DataMixerFloat s)
		{
			return s.GetValue();
		}


		public class Control
		{
			int _sequenceNo;
			public int SequencNo { get { return _sequenceNo; } }
			DataMixerFloat _holder;
			public DataMixerFloat Holder { get { return _holder; } }

			public Control(DataMixerFloat holder, int seq)
			{
				_holder = holder;
				_sequenceNo = seq;
			}

			float _value;
			public float Value
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
}