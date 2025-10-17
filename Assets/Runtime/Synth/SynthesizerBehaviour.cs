using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Synth
{
	public class SynthesizerBehaviour : MonoBehaviour
	{
		[SerializeField] private Keyboard keyboard;

		private readonly OscillatorSettings _osc1Settings = OscillatorSettings.CreateBasicSine();
		private readonly OscillatorSettings _osc2Settings = OscillatorSettings.CreateDisabledBasicSquare();
		private readonly LfoSettings _lfo1Settings = LfoSettings.CreateDisabled1HzSine();
		private readonly LfoSettings _lfo2Settings = LfoSettings.CreateDisabled4HzSquare();
		private readonly FilterSettings _filterSettings = FilterSettings.CreateDefault();
		private readonly EnvelopeSettings _ampSettings = EnvelopeSettings.CreateDefault();
		private readonly EnvelopeSettings _env1Settings = EnvelopeSettings.CreateDefault();
		private readonly EnvelopeSettings _env2Settings = EnvelopeSettings.CreateDefault();

		private int _sampleRate;

		private readonly Voice[] _voices = new Voice[(int)(Note.C8 + 1)];

		private void Start()
		{
			_sampleRate = AudioSettings.outputSampleRate;
			PrepareVoices();
			keyboard.OnNotePressed += NoteOn;
			keyboard.OnNoteReleased += NoteOff;
		}

		private void NoteOff(Note note)
		{
			var i = (int)note;
			_voices[i].NoteOff();
		}

		private void NoteOn(Note note)
		{
			var i = (int)note;
			_voices[i].NoteOn();
		}

		private void PrepareVoices()
		{
			for (int i = 0; i < _voices.Length; i++)
			{
				_voices[i] = new Voice
				(
					_sampleRate,
					NoteToFrequency.GetFrequency((Note)i),
					_osc1Settings,
					_osc2Settings,
					_lfo1Settings,
					_lfo2Settings,
					_filterSettings,
					_ampSettings,
					_env1Settings,
					_env2Settings
				);
			}
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			int dataLength = data.Length / channels;
			for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
			{
				var output = MixVoices();

				for (int channelIndex = 0; channelIndex < channels; channelIndex++)
				{
					data[dataIndex * channels + channelIndex] += (float)output;
				}
			}
		}

		private double MixVoices()
		{
			double signal = 0;
			double envelopeSum = 0;

			foreach (Voice v in _voices)
			{
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