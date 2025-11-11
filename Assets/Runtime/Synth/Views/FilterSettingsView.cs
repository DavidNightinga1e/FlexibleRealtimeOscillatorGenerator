using System;
using Runtime.Common;
using Runtime.Synth.Presenters;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class FilterSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle toggle;
		[SerializeField] private FilterTypeSelectorPresenter filterTypeSelectorPresenter;
		[SerializeField] private GainPresenter gainPresenter;
		[SerializeField] private CutoffFrequencyPresenter cutoffFrequencyPresenter;
		[SerializeField] private GainPresenter qFactorPresenter;
		[SerializeField] private EnvelopeSelectionPresenter envelopeSelectionPresenter;
		[SerializeField] private GainPresenter envelopeAmountPresenter;
		[SerializeField] private LfoSelectionPresenter lfoSelectionPresenter;
		[SerializeField] private GainPresenter lfoAmountPresenter;
		[SerializeField] private GainPresenter keyTrackingPresenter;

		private FilterSettings _settings;

		public void SetSettings(FilterSettings settings)
		{
			_settings = settings;

			toggle.SetValueWithoutNotify(_settings.Enabled);
			filterTypeSelectorPresenter.SetValueWithoutNotify(_settings.FilterType);
			gainPresenter.SetValueWithoutNotify(_settings.Gain);
			cutoffFrequencyPresenter.SetValueWithoutNotify(_settings.CutoffFrequency);
			qFactorPresenter.SetValueWithoutNotify(_settings.QFactor);
			envelopeSelectionPresenter.SetValueWithoutNotify(_settings.EnvelopeSelection);
			envelopeAmountPresenter.SetValueWithoutNotify(_settings.EnvelopeAmount);
			lfoSelectionPresenter.SetValueWithoutNotify(_settings.LfoSelection);
			lfoAmountPresenter.SetValueWithoutNotify(_settings.LfoAmount);
			keyTrackingPresenter.SetValueWithoutNotify(_settings.KeyTracking);
		}

		private void Awake()
		{
			toggle.ValueChanged += ToggleValueChanged;
			filterTypeSelectorPresenter.ValueChanged += FilterTypeSelectorValueChanged;
			gainPresenter.ValueChanged += GainValueChanged;
			cutoffFrequencyPresenter.ValueChanged += CutoffValueChanged;
			qFactorPresenter.ValueChanged += QFactorValueChanged;
			envelopeSelectionPresenter.ValueChanged += EnvelopeValueChanged;
			envelopeAmountPresenter.ValueChanged += EnvelopeAmountValueChanged;
			lfoSelectionPresenter.ValueChanged += LfoSelectionValueChanged;
			lfoAmountPresenter.ValueChanged += LfoAmountValueChanged;
			keyTrackingPresenter.ValueChanged += KeyTrackingValueChanged;
		}

		private void KeyTrackingValueChanged(double obj)
		{
			_settings.KeyTracking = obj;
			_settings.InvokeChanged();
		}

		private void LfoAmountValueChanged(double obj)
		{
			_settings.LfoAmount = obj;
			_settings.InvokeChanged();
		}

		private void LfoSelectionValueChanged(LfoSelection obj)
		{
			_settings.LfoSelection = obj;
			_settings.InvokeChanged();
		}

		private void EnvelopeAmountValueChanged(double obj)
		{
			_settings.EnvelopeAmount = obj;
			_settings.InvokeChanged();
		}

		private void EnvelopeValueChanged(EnvelopeSelection obj)
		{
			_settings.EnvelopeSelection = obj;
			_settings.InvokeChanged();
		}

		private void QFactorValueChanged(double obj)
		{
			_settings.QFactor = obj;
			_settings.InvokeChanged();
		}

		private void CutoffValueChanged(double obj)
		{
			_settings.CutoffFrequency = obj;
			_settings.InvokeChanged();
		}

		private void GainValueChanged(double obj)
		{
			_settings.Gain = obj;
			_settings.InvokeChanged();
		}

		private void FilterTypeSelectorValueChanged(FilterType obj)
		{
			_settings.FilterType = obj;
			_settings.InvokeChanged();
		}

		private void ToggleValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}

		private void OnDestroy()
		{
			toggle.ValueChanged -= ToggleValueChanged;
			filterTypeSelectorPresenter.ValueChanged -= FilterTypeSelectorValueChanged;
			gainPresenter.ValueChanged -= GainValueChanged;
			cutoffFrequencyPresenter.ValueChanged -= CutoffValueChanged;
			qFactorPresenter.ValueChanged -= QFactorValueChanged;
			envelopeSelectionPresenter.ValueChanged -= EnvelopeValueChanged;
			envelopeAmountPresenter.ValueChanged -= EnvelopeAmountValueChanged;
			lfoSelectionPresenter.ValueChanged -= LfoSelectionValueChanged;
			lfoAmountPresenter.ValueChanged -= LfoAmountValueChanged;
			keyTrackingPresenter.ValueChanged -= KeyTrackingValueChanged;
		}
	}
}