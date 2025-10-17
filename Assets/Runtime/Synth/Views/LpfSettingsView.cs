using System;
using Runtime.Test;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class LpfSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle enableToggle;
		[SerializeField] private FloatKnob cutoffFrequencyKnob;
		[SerializeField] private FloatKnob qFactorKnob;
		[SerializeField] private IntegerKnob envSelectionKnob;
		[SerializeField] private FloatKnob envAmountKnob;
		[SerializeField] private IntegerKnob lfoSelectionKnob;
		[SerializeField] private FloatKnob lfoAmountKnob;

		private LpfSettings _settings;

		private const float CutoffMin = 20f;
		private const float CutoffMax = 20000f;
		private const float QFactorMin = 0f;
		private const float QFactorMax = 1.5f;

		public void SetSettings(LpfSettings settings)
		{
			_settings = settings;

			enableToggle.SetValueWithoutNotify(_settings.Enabled);
			cutoffFrequencyKnob.SetValueWithoutNotify(Mathf.InverseLerp(CutoffMin, CutoffMax,
				(float)_settings.CutoffFrequency));
			qFactorKnob.SetValueWithoutNotify(Mathf.InverseLerp(QFactorMin, QFactorMax, (float)_settings.QFactor));
			envSelectionKnob.SetValueWithoutNotify((int)_settings.EnvelopeSelection);
			envAmountKnob.SetValueWithoutNotify((float)settings.EnvelopeAmount);
			lfoSelectionKnob.SetValueWithoutNotify((int)_settings.LfoSelection);
			lfoAmountKnob.SetValueWithoutNotify((float)settings.LfoAmount);
		}

		private void Awake()
		{
			enableToggle.ValueChanged += OnEnableValueChanged;
			cutoffFrequencyKnob.ValueChanged += OnCutoffFrequencyValueChanged;
			qFactorKnob.ValueChanged += OnQFactorValueChanged;
			envSelectionKnob.ValueChanged += OnEnvSelectionValueChanged;
			envAmountKnob.ValueChanged += OnEnvAmountValueChanged;
			lfoSelectionKnob.ValueChanged += OnLfoSelectionValueChanged;
			lfoAmountKnob.ValueChanged += OnLfoAmountValueChanged;
		}

		private void OnLfoAmountValueChanged(float obj)
		{
			_settings.LfoAmount = obj;
			_settings.InvokeChanged();
		}

		private void OnLfoSelectionValueChanged(int obj)
		{
			_settings.LfoSelection = (LfoSelection)obj;
			_settings.InvokeChanged();
		}

		private void OnEnvAmountValueChanged(float obj)
		{
			_settings.EnvelopeAmount = obj;
			_settings.InvokeChanged();
		}

		private void OnEnvSelectionValueChanged(int obj)
		{
			_settings.LfoSelection = (LfoSelection)obj;
			_settings.InvokeChanged();
		}

		private void OnQFactorValueChanged(float obj)
		{
			_settings.EnvelopeAmount = obj;
			_settings.InvokeChanged();
		}

		private void OnCutoffFrequencyValueChanged(float obj)
		{
			_settings.CutoffFrequency = obj;
			_settings.InvokeChanged();
		}

		private void OnEnableValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}
	}
}