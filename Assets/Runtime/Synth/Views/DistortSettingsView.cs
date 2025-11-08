using Runtime.Common;
using Runtime.Synth.Presenters;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class DistortSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle toggle;
		[SerializeField] private GainPresenter inputGainPresenter;
		[SerializeField] private GainPresenter outputGainPresenter;
		[SerializeField] private GainPresenter drivePresenter;
		[SerializeField] private GainPresenter mixPresenter;
		[SerializeField] private DistortTypePresenter distortTypePresenter;

		private DistortSettings _settings;
		
		public void SetSettings(DistortSettings settings)
		{
			_settings = settings;
			
			toggle.SetValueWithoutNotify(settings.Enabled);
			inputGainPresenter.SetValueWithoutNotify(settings.InputGain);
			outputGainPresenter.SetValueWithoutNotify(settings.OutputGain);
			drivePresenter.SetValueWithoutNotify(settings.Drive);
			mixPresenter.SetValueWithoutNotify(settings.Mix);
			distortTypePresenter.SetValueWithoutNotify(settings.DistortType);
		}

		private void Awake()
		{
			inputGainPresenter.MinValue = DistortSettings.InputGainMinValue;
			inputGainPresenter.MaxValue = DistortSettings.InputGainMaxValue;
			outputGainPresenter.MinValue = DistortSettings.OutputGainMinValue;
			outputGainPresenter.MaxValue = DistortSettings.OutputGainMaxValue;
			drivePresenter.MinValue = DistortSettings.DriveMinValue;
			drivePresenter.MaxValue = DistortSettings.DriveMaxValue;
			
			toggle.ValueChanged += ToggleValueChanged;
			inputGainPresenter.ValueChanged += OnInputGainChanged;
			outputGainPresenter.ValueChanged += OnOutputGainChanged;
			drivePresenter.ValueChanged += OnDriveValueChanged;
			mixPresenter.ValueChanged += OnMixValueChanged;
			distortTypePresenter.ValueChanged += OnDistortValueChanged;
		}

		private void OnDistortValueChanged(DistortType obj)
		{
			_settings.DistortType = obj;
			_settings.InvokeChanged();
		}

		private void OnMixValueChanged(double obj)
		{
			_settings.Mix = obj;
			_settings.InvokeChanged();
		}

		private void OnDriveValueChanged(double obj)
		{
			_settings.Drive = obj;
			_settings.InvokeChanged();
		}

		private void OnOutputGainChanged(double obj)
		{
			_settings.OutputGain = obj;
			_settings.InvokeChanged();
		}

		private void OnInputGainChanged(double obj)
		{
			_settings.InputGain = obj;
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
			inputGainPresenter.ValueChanged -= OnInputGainChanged;
			outputGainPresenter.ValueChanged -= OnOutputGainChanged;
			drivePresenter.ValueChanged -= OnDriveValueChanged;
			mixPresenter.ValueChanged -= OnMixValueChanged;
			distortTypePresenter.ValueChanged -= OnDistortValueChanged;
		}
	}
}