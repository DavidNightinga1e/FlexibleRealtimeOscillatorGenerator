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

		private int _sampleRate;

		private readonly Voice[] _voices = new Voice[(int)(Note.C8 + 1)];
		private DistortInstance _distortInstance;
		private DelayInstance _delayInstance;
		private ReverbInstance _reverbInstance;
		
		private readonly Stopwatch _stopwatch = new();

		private void Awake()
		{
			_presetsView.OnPresetChanged += OnPresetChanged;
			
			keyboard.NoteDownEvent += OnNoteDown;
			keyboard.NoteUpEvent += OnNoteUp;
			
			_sampleRate = AudioSettings.outputSampleRate;
		}

		private void OnPresetChanged()
		{
			_preset = _presetsView.ActivePreset;
			OnPresetLoaded();
		}

		private void OnPresetLoaded()
		{
			PrepareSettings();
			
			PrepareVoices();
			PrepareEffects();			
		}

		private void OnDestroy()
		{
			keyboard.NoteDownEvent -= OnNoteDown;
			keyboard.NoteUpEvent -= OnNoteUp;
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

		private void OnNoteUp(Note note)
		{
			var i = (int)note;
			_voices[i].NoteUp();
		}

		private void OnNoteDown(Note note)
		{
			var i = (int)note;
			_voices[i].NoteDown();
		}

		private void PrepareVoices()
		{
			for (int i = 0; i < _voices.Length; i++)
			{
				_voices[i] = new Voice
				(
					_sampleRate,
					NoteToFrequency.GetFrequency((Note)i),
					_preset.Osc1Settings,
					_preset.Osc2Settings,
					_preset.Lfo1Settings,
					_preset.Lfo2Settings,
					_preset.FilterSettings,
					_preset.AmpSettings,
					_preset.Env1Settings,
					_preset.Env2Settings
				);
			}
		}

		private void PrepareEffects()
		{
			_distortInstance = new DistortInstance(_sampleRate, _preset.DistortSettings);
			_delayInstance = new DelayInstance(_sampleRate, _preset.DelaySettings);
			_reverbInstance = new ReverbInstance(_sampleRate, _preset.ReverbSettings);
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_preset is null)
				return;
			
			_stopwatch.Restart();
			
			int dataLength = data.Length / channels;

			for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
			{
				var output = MixVoices();
				
				output = ApplyEffects(output);
				
				RaiseSampleCompleted(output);

				for (int channelIndex = 0; channelIndex < channels; channelIndex++)
				{
					data[dataIndex * channels + channelIndex] += (float)output;
				}
			}
			
			_stopwatch.Stop();
		}

		private void RaiseSampleCompleted(double sample)
		{
			SampleCompletedEvent?.Invoke(sample);
		}

		private double ApplyEffects(double sample)
		{
			sample = _distortInstance.ProcessSample(sample);
			sample = _delayInstance.ProcessSample(sample);
			sample = _reverbInstance.ProcessSample(sample);
			return sample;
		}

		private double MixVoices()
		{
			double signal = 0;
			double envelopeSum = 0;

			foreach (Voice v in _voices)
			{
				if (v.IsFinished)
					continue;

				v.UpdateSample();
				signal += v.Sample;
				envelopeSum += v.AmpEnvelopeValue;
			}

			if (envelopeSum > 1)
				signal /= envelopeSum;

			if (double.IsNaN(signal))
			{
				Debug.LogError($"Mixer: signal was NaN");
				return 0;
			}

			if (Math.Abs(signal) > 1)
			{
				Debug.LogError($"Mixer: signal outside boundary {signal}");
				return Math.Clamp(signal, -1, 1);
			}

			return signal;
		}
	}
}