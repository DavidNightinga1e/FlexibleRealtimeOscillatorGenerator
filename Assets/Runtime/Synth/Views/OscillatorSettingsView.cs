using System;
using Runtime.Common;
using Runtime.Test;
using Runtime.UI;
using UnityEngine;
using UnityEngine.UI;
using Toggle = Runtime.UI.Toggle;

namespace Runtime.Synth.Views
{
	public class OscillatorSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle enabledToggle;
		[SerializeField] private IntegerKnob waveformKnob;
		[SerializeField] private FloatKnob gainKnob;
		[SerializeField] private Slider octaveShiftSlider;
		[SerializeField] private IntegerKnob envSelectionKnob;
		[SerializeField] private LfoSelection vibratoLfoSelection;
		[SerializeField] private FloatKnob vibratoAmountKnob;

		private OscillatorSettings _settings;
		
		public void SetSettings(OscillatorSettings settings)
		{
			_settings = settings;

			enabledToggle.SetValueWithoutNotify(_settings.Enabled);
			waveformKnob.SetValueWithoutNotify((int)_settings.Waveform);
			gainKnob.SetValueWithoutNotify((float)_settings.Gain);
			octaveShiftSlider.SetValueWithoutNotify(_settings.OctaveShift);
			gainKnob.SetValueText($"{_settings.Gain:0.00}");
		}

		private void Awake()
		{
			enabledToggle.ValueChanged += OnEnabledValueChanged;
			waveformKnob.ValueChanged += OnWaveformChanged;
			gainKnob.ValueChanged += OnGainChanged;
			octaveShiftSlider.onValueChanged.AddListener(OnOctaveShiftValueChanged);
		}

		private void OnEnabledValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}

		private void OnOctaveShiftValueChanged(float arg0)
		{
			_settings.OctaveShift = (int)arg0;
			_settings.InvokeChanged();
		}

		private void OnGainChanged(float obj)
		{
			gainKnob.SetValueText($"{obj:0.00}");
			_settings.Gain = obj;
			_settings.InvokeChanged();
		}

		private void OnWaveformChanged(int obj)
		{
			_settings.Waveform = (Waveform)obj;
			_settings.InvokeChanged();
		}

		private void OnDestroy()
		{
			waveformKnob.ValueChanged -= OnWaveformChanged;
			gainKnob.ValueChanged -= OnGainChanged;
		}
	}
}