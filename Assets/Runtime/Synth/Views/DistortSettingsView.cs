using System;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class DistortSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle toggle;

		private DistortSettings _settings;
		
		public void SetSettings(DistortSettings settings)
		{
			_settings = settings;
			
			toggle.SetValueWithoutNotify(settings.Enabled);
		}

		private void Awake()
		{
			toggle.ValueChanged += ToggleValueChanged;
		}

		private void ToggleValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}

		private void OnDestroy()
		{
			toggle.ValueChanged -= ToggleValueChanged;
		}
	}
}