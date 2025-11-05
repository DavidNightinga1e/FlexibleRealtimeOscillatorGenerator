using System;
using Runtime.Common;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class WaveformPresenter : ValuePresenter<Waveform>
	{
		[SerializeField] private IntegerKnob knob;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(int obj)
		{
			RaiseValueChanged((Waveform)obj);
		}

		public override void SetValueWithoutNotify(Waveform waveform)
		{
			knob.SetValueWithoutNotify((int)waveform);
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}