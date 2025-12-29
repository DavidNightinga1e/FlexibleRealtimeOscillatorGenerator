using System;
using System.Diagnostics;
using Runtime.Common;
using Runtime.Synth.Presets;
using Runtime.Synth.Views;
using Runtime.Synth.Views.Presets;
using Runtime.UI.Keyboard;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Runtime.Synth
{
	public class SynthesizerBehaviour : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI timeText;
		[SerializeField] private Keyboard keyboard;
		[SerializeField] private EnvelopeSettingsView _ampSettingsView;
		[SerializeField] private EnvelopeSettingsView _env1SettingsView;
		[SerializeField] private EnvelopeSettingsView _env2SettingsView;
		[SerializeField] private OscillatorSettingsView _osc1SettingsView;
		[SerializeField] private OscillatorSettingsView _osc2SettingsView;
		[SerializeField] private LfoSettingsView _lfo1SettingsView;
		[SerializeField] private LfoSettingsView _lfo2SettingsView;
		[SerializeField] private FilterSettingsView _filterSettingsView;
		[SerializeField] private DistortSettingsView _distortSettingsView;
		[SerializeField] private DelaySettingsView _delaySettingsView;
		[SerializeField] private ReverbSettingsView _reverbSettingsView;
		[SerializeField] private PresetsView _presetsView;

		public event Action<double> SampleCompletedEvent; 
		
		private SynthesizerPreset _preset = BuiltInPresets.CreateDefault();
		
		private SynthesizerInstance _synthesizerInstance;

		private int _sampleRate;
		
		private readonly Stopwatch _stopwatch = new();

		private void Awake()
		{
			_presetsView.OnPresetChanged += OnPresetChanged;
			_sampleRate = AudioSettings.outputSampleRate;
		}

		private void OnEnable()
		{
			keyboard.NoteDownEvent += OnNoteDown;
			keyboard.NoteUpEvent += OnNoteUp;
		}

		private void OnNoteUp(Note obj)
		{
			_synthesizerInstance.OnNoteUp(obj);
		}

		private void OnNoteDown(Note obj)
		{
			_synthesizerInstance.OnNoteDown(obj);
		}

		private void OnDisable()
		{
			keyboard.NoteDownEvent -= OnNoteDown;
			keyboard.NoteUpEvent -= OnNoteUp;
		}

		private void OnPresetChanged()
		{
			_preset = _presetsView.ActivePreset;
			OnPresetLoaded();
		}

		private void OnPresetLoaded()
		{
			PrepareSettings();
			
			_synthesizerInstance = new SynthesizerInstance(_sampleRate, _preset);
			_synthesizerInstance.SampleCompletedEvent += d => SampleCompletedEvent?.Invoke(d);
		}

		private void PrepareSettings()
		{
			_ampSettingsView.SetSettings(_preset.AmpSettings);
			_env1SettingsView.SetSettings(_preset.Env1Settings);
			_env2SettingsView.SetSettings(_preset.Env2Settings);

			_lfo1SettingsView.SetSettings(_preset.Lfo1Settings);
			_lfo2SettingsView.SetSettings(_preset.Lfo2Settings);

			_osc1SettingsView.SetSettings(_preset.Osc1Settings);
			_osc2SettingsView.SetSettings(_preset.Osc2Settings);

			_filterSettingsView.SetSettings(_preset.FilterSettings);
			
			_distortSettingsView.SetSettings(_preset.DistortSettings);
			_delaySettingsView.SetSettings(_preset.DelaySettings);
			_reverbSettingsView.SetSettings(_preset.ReverbSettings);
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_synthesizerInstance is null)
				return;
			
			_stopwatch.Restart();
			
			_synthesizerInstance.OnAudioFilterRead(data, channels);
			
			_stopwatch.Stop();
		}
	}
}