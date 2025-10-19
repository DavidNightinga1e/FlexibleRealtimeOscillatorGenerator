using System;
using Runtime.Common;
using Runtime.Test;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class EnvelopeSelectionPresenter : ValuePresenter<EnvelopeSelection>
	{
		[SerializeField] private IntegerKnob knob;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(int obj)
		{
			InvokeValueChanged((EnvelopeSelection)obj);
		}

		public override void SetValueWithoutNotify(EnvelopeSelection value)
		{
			knob.SetValueWithoutNotify((int)value);
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}