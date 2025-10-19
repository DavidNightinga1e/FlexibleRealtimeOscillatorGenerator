using System;
using Runtime.Synth.Presenters;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class DelaySettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle toggle;
		[SerializeField] private DelayDurationPresenter delayDurationPresenter;
		[SerializeField] private GainPresenter mixPresenter;
		[SerializeField] private GainPresenter feedbackPresenter;

		private DelaySettings _settings;
		
		public void SetSettings(DelaySettings settings)
		{
			_settings = settings;
			
			toggle.SetValueWithoutNotify(settings.Enabled);
			delayDurationPresenter.SetValueWithoutNotify(settings.DelayTime);
			mixPresenter.SetValueWithoutNotify(settings.Mix);
			feedbackPresenter.SetValueWithoutNotify(settings.Feedback);
		}

		private void Awake()
		{
			toggle.ValueChanged += OnToggleValueChanged;
			delayDurationPresenter.ValueChanged += OnDelayValueChanged;
			mixPresenter.ValueChanged += OnMixValueChanged;
			feedbackPresenter.ValueChanged += OnFeedbackValueChanged;
		}

		private void OnFeedbackValueChanged(double obj)
		{
			_settings.Feedback = obj;
			_settings.InvokeChanged();
		}

		private void OnMixValueChanged(double obj)
		{
			_settings.Mix = obj;
			_settings.InvokeChanged();
		}

		private void OnDelayValueChanged(double obj)
		{
			_settings.DelayTime = obj;
			_settings.InvokeChanged();
		}

		private void OnToggleValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}

		private void OnDestroy()
		{
			toggle.ValueChanged -= OnToggleValueChanged;
			delayDurationPresenter.ValueChanged -= OnDelayValueChanged;
			mixPresenter.ValueChanged -= OnMixValueChanged;
			feedbackPresenter.ValueChanged -= OnFeedbackValueChanged;
		}
	}
}