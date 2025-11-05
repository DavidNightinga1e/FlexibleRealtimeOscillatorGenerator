using System;
using System.Globalization;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class GainPresenter : ValuePresenter<double>
	{
		[SerializeField] private FloatKnob knob;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(float obj)
		{
			InvokeValueChanged(obj);
			UpdateText(obj);
		}

		public override void SetValueWithoutNotify(double value)
		{
			knob.SetValueWithoutNotify((float)value);
			UpdateText(value);
		}

		private void UpdateText(double gainValue)
		{
			knob.SetValueText(gainValue.ToString("0.00", CultureInfo.InvariantCulture));
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}