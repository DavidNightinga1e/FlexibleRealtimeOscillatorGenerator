using System;
using System.Globalization;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class GainPresenter : ValuePresenter<double>
	{
		[SerializeField] private FloatKnob knob;

		public double MinValue { get; set; } = 0;
		public double MaxValue { get; set; } = 1;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(float knobValue)
		{
			double value = KnobToValue(knobValue);
			RaiseValueChanged(value);
			UpdateText(value);
		}

		public override void SetValueWithoutNotify(double value)
		{
			var knobValue = ValueToKnob(value);
			knob.SetValueWithoutNotify(knobValue);
			UpdateText(value);
		}

		private void UpdateText(double gainValue)
		{
			knob.SetValueText(gainValue.ToString("0.00", CultureInfo.InvariantCulture));
		}

		private double KnobToValue(float knobValue)
		{
			return MinValue + (MaxValue - MinValue) * knobValue;
		}

		private float ValueToKnob(double value)
		{
			return (float)((value - MinValue) / (MaxValue - MinValue));
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}