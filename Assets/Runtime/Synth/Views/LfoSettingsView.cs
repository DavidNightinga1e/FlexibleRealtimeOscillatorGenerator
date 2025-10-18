using System;
using Runtime.Common;
using Runtime.Synth.Presenters;
using Runtime.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class LfoSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle toggle;
		[SerializeField] private WaveformPresenter waveformPresenter;
		[SerializeField] private GainPresenter gainPresenter;
		[SerializeField] private LfoFrequencyPresenter frequencyPresenter;
		
		private LfoSettings _settings;

		public void SetSettings(LfoSettings settings)
		{
			_settings = settings;
			
			toggle.SetValueWithoutNotify(settings.Enabled);
			waveformPresenter.SetValueWithoutNotify(settings.Waveform);
			gainPresenter.SetValueWithoutNotify(settings.Gain);
			frequencyPresenter.SetValueWithoutNotify(settings.Frequency);
		}

		private void Awake()
		{
			toggle.ValueChanged += OnToggleValueChanged;
			waveformPresenter.ValueChanged += OnWaveformValueChanged;
			gainPresenter.ValueChanged += OnGainValueChanged;
			frequencyPresenter.ValueChanged += OnFrequencyValueChanged;
		}

		private void OnFrequencyValueChanged(double obj)
		{
			_settings.Frequency = obj;
			_settings.InvokeChanged();
		}

		private void OnGainValueChanged(double obj)
		{
			_settings.Gain = obj;
			_settings.InvokeChanged();
		}

		private void OnWaveformValueChanged(Waveform obj)
		{
			_settings.Waveform = obj;
			_settings.InvokeChanged();
		}

		private void OnToggleValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}
	}
}