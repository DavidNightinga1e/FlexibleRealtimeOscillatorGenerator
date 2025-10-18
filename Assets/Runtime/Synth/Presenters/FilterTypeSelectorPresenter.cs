using System;
using Runtime.Common;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class FilterTypeSelectorPresenter : ValuePresenter<FilterType>
	{
		[SerializeField] private IntegerKnob knob;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(int obj)
		{
			InvokeValueChanged((FilterType)obj);
		}

		public override void SetValueWithoutNotify(FilterType value)
		{
			knob.SetValueWithoutNotify((int)value);
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}