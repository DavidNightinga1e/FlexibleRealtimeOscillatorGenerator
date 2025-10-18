using System;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class GainPresenter : ValuePresenter<double>
	{
		[SerializeField] private FloatKnob knob;

		private void Awake()
		{
			knob.SetValueText(string.Empty);
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(float obj)
		{
			InvokeValueChanged(obj);
		}

		public override void SetValueWithoutNotify(double value)
		{
			knob.SetValueWithoutNotify((float)value);
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}