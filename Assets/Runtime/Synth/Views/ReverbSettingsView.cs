using System;
using Runtime.Synth.Presenters;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class ReverbSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle toggle;
		[SerializeField] private GainPresenter roomPresenter;
		[SerializeField] private GainPresenter mixPresenter;
		[SerializeField] private GainPresenter dampPresenter;

		private ReverbSettings _settings;

		public void SetSettings(ReverbSettings settings)
		{
			_settings = settings;

			toggle.SetValueWithoutNotify(settings.Enabled);
			mixPresenter.SetValueWithoutNotify(settings.Mix);
			roomPresenter.SetValueWithoutNotify(settings.RoomSize);
			dampPresenter.SetValueWithoutNotify(settings.Damp);
		}

		private void Awake()
		{
			toggle.ValueChanged += ToggleOnValueChanged;
			mixPresenter.ValueChanged += OnMixValueChanged;
			roomPresenter.ValueChanged += OnRoomValueChanged;
			dampPresenter.ValueChanged += OnDampValueChanged;
		}

		private void OnDampValueChanged(double obj)
		{
			_settings.Damp = obj;
			_settings.InvokeChanged();
		}

		private void OnRoomValueChanged(double obj)
		{
			_settings.RoomSize = obj;
			_settings.InvokeChanged();
		}

		private void OnMixValueChanged(double obj)
		{
			_settings.Mix = obj;
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
			mixPresenter.ValueChanged -= OnMixValueChanged;
			roomPresenter.ValueChanged -= OnRoomValueChanged;
			dampPresenter.ValueChanged -= OnDampValueChanged;
		}
	}
}