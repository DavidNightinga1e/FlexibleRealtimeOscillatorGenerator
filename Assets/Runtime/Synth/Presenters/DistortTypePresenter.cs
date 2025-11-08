using Runtime.Common;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class DistortTypePresenter : ValuePresenter<DistortType>
	{
		[SerializeField] private IntegerKnob knob;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(int obj)
		{
			RaiseValueChanged((DistortType)obj);
		}

		public override void SetValueWithoutNotify(DistortType value)
		{
			knob.SetValueWithoutNotify((int)value);
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}