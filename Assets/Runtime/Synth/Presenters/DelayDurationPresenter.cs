using System;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class DelayDurationPresenter : ValuePresenter<double>
	{
		[SerializeField] private FloatKnob knob;

		private const double MinValue = 0.001;
		private const double MaxValue = 2.0;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		public override void SetValueWithoutNotify(double delay)
		{
			knob.SetValueWithoutNotify(DelayToKnob(delay));
			UpdateText(delay);
		}

		private double KnobToDelay(float knobValue)
		{
			return Mathf.Lerp((float)MinValue, (float)MaxValue, knobValue);
		}

		private float DelayToKnob(double delay)
		{
			return Mathf.InverseLerp((float)MinValue, (float)MaxValue, (float)delay);
		}

		private void OnValueChanged(float knobValue)
		{
			double delay = KnobToDelay(knobValue);
			InvokeValueChanged(delay);
			UpdateText(delay);
		}

		private void UpdateText(double delayTime)
		{
			knob.SetValueText(delayTime < 1 ? $"{delayTime * 1000:0} ms" : $"{delayTime:0.00} s");
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}