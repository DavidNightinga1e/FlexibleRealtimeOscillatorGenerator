using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Synth.Presenters
{
	public class OctaveShiftPresenter : ValuePresenter<int>
	{
		[SerializeField] private Slider slider;

		private void Awake()
		{
			slider.onValueChanged.AddListener(OnValueChanged);
		}

		private void OnValueChanged(float arg0)
		{
			RaiseValueChanged((int)arg0);
		}

		public override void SetValueWithoutNotify(int value)
		{
			slider.SetValueWithoutNotify(value);
		}
	}
}