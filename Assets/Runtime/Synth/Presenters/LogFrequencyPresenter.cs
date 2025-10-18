using System;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class LogFrequencyPresenter : ValuePresenter<double>
	{
		[SerializeField] private FloatKnob knob;

		protected virtual double MinFrequency => 1.0;
		protected virtual double MaxFrequency => 1000.0;
		protected virtual double CurveStrength => 2.0;

		protected virtual string GetTextByFrequency(double frequency)
		{
			return $"{frequency:0.00} Hz";
		}

		private void Awake()
		{
			knob.ValueChanged += OnValueChanged;
		}

		private void OnValueChanged(float knobValue)
		{
			double frequency = KnobToFrequency(knobValue);
			InvokeValueChanged(frequency);
			UpdateKnobText(frequency);
		}

		public override void SetValueWithoutNotify(double frequency)
		{
			knob.SetValueWithoutNotify(FrequencyToKnob(frequency));
			UpdateKnobText(frequency);
		}

		private void UpdateKnobText(double frequency)
		{
			knob.SetValueText(GetTextByFrequency(frequency));
		}

		public double KnobToFrequency(float knobValue)
		{
			knobValue = Math.Max(0f, Math.Min(1f, knobValue));
        
			// Exponential mapping with adjustable strength
			double normalized = Math.Pow(knobValue, CurveStrength);
        
			// Logarithmic frequency mapping
			double logMin = Math.Log10(MinFrequency);
			double logMax = Math.Log10(MaxFrequency);
        
			double frequency = Math.Pow(10, logMin + normalized * (logMax - logMin));
        
			return frequency;
		}
    
		public float FrequencyToKnob(double frequency)
		{
			frequency = Math.Max(MinFrequency, Math.Min(MaxFrequency, frequency));
        
			// Convert to logarithmic scale
			double logMin = Math.Log10(MinFrequency);
			double logMax = Math.Log10(MaxFrequency);
        
			double logFreq = Math.Log10(frequency);
			double normalized = (logFreq - logMin) / (logMax - logMin);
        
			// Inverse exponential mapping
			float knobValue = (float)Math.Pow(normalized, 1.0 / CurveStrength);
        
			return knobValue;
		}

		private void OnDestroy()
		{
			knob.ValueChanged -= OnValueChanged;
		}
	}
}