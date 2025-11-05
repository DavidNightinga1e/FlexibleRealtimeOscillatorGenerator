using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class VibratoAmountPresenter : ValuePresenter<double>
	{
		[SerializeField] private FloatKnob knob;
		
		private const float VibratoRangeSemitones = 3;

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(float knobValue)
		{
			double vibrato = KnobToVibrato(knobValue);
			RaiseValueChanged(vibrato);
			UpdateKnobText(vibrato);
		}

		public override void SetValueWithoutNotify(double vibrato)
		{
			knob.SetValueWithoutNotify(VibratoToKnob(vibrato));
			UpdateKnobText(vibrato);
		}

		private float VibratoToKnob(double vibrato)
		{
			return Mathf.InverseLerp(0, VibratoRangeSemitones, (float)vibrato);
		}

		private double KnobToVibrato(float knobValue)
		{
			return Mathf.Lerp(0, VibratoRangeSemitones, knobValue);
		}

		private void UpdateKnobText(double vibrato)
		{
			// Semitones to cents. 100 cents = 1 semitone
			knob.SetValueText($"{vibrato * 100:0}");
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}