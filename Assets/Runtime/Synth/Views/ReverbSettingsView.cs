using System;
using Runtime.Synth.Presenters;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class ReverbSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle toggle;
		[SerializeField] private DelayDurationPresenter decayDurationPresenter;
		[SerializeField] private GainPresenter mixPresenter;

		private ReverbSettings _settings;

		public void SetSettings(ReverbSettings settings)
		{
			_settings = settings;

			toggle.SetValueWithoutNotify(settings.Enabled);
			decayDurationPresenter.SetValueWithoutNotify(settings.DecayTime);
			mixPresenter.SetValueWithoutNotify(settings.Mix);
		}

		private void Awake()
		{
			toggle.ValueChanged += ToggleOnValueChanged;
			decayDurationPresenter.ValueChanged += OnDecayDurationValueChanged;
			mixPresenter.ValueChanged += OnMixValueChanged;
		}

		private void OnMixValueChanged(double obj)
		{
			_settings.Mix = obj;
			_settings.InvokeChanged();
		}

		private void OnDecayDurationValueChanged(double obj)
		{
			_settings.DecayTime = obj;
			_settings.InvokeChanged();
		}

		private void ToggleOnValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}

		private void OnDestroy()
		{
			toggle.ValueChanged -= ToggleOnValueChanged;
			decayDurationPresenter.ValueChanged -= OnDecayDurationValueChanged;
			mixPresenter.ValueChanged -= OnMixValueChanged;
		}
	}
}