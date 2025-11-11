using System;
using Runtime.Common;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class LfoSelectionPresenter : ValuePresenter<LfoSelection>
	{
		[SerializeField] private IntegerKnob knob;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(int obj)
		{
			RaiseValueChanged((LfoSelection)obj);
		}

		public override void SetValueWithoutNotify(LfoSelection value)
		{
			knob.SetValueWithoutNotify((int)value);
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}