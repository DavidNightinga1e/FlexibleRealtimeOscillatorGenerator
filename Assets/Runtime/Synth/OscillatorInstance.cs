using System;
using Runtime.Common;
using Runtime.Test;
using UnityEngine;

namespace Runtime.Synth
{
	public class OscillatorInstance : ISampleProvider, INoteHandler
	{
		private readonly int _sampleRate;
		private readonly double _baseFrequency;
		private readonly OscillatorSettings _settings;
		private readonly LfoInstance _lfo1;
		private readonly LfoInstance _lfo2;
		private readonly EnvelopeInstance _env1;
		private readonly EnvelopeInstance _env2;

		private double _activeFrequency;
		private double _phase;
		private double _phaseIncrement;

		public double Sample { get; private set; }

		public OscillatorInstance
		(
			int sampleRate,
			double baseFrequency,
			OscillatorSettings settings,
			LfoInstance lfo1,
			LfoInstance lfo2,
			EnvelopeInstance env1,
			EnvelopeInstance env2
		)
		{
			_sampleRate = sampleRate;
			_baseFrequency = baseFrequency;
			_settings = settings;
			_lfo1 = lfo1;
			_lfo2 = lfo2;
			_env1 = env1;
			_env2 = env2;
		}

		public void UpdateSample()
		{
			if (!_settings.Enabled)
			{
				Sample = 0;
				return;
			}

			LfoInstance vibratoLfoSelection = SelectorUtilities.SelectLfo(_lfo1, _lfo2, _settings.VibratoLfoSelection);
			UpdateVibrato(vibratoLfoSelection);

			UpdatePhase();
			
			EnvelopeInstance env = SelectorUtilities.SelectEnvelope(_env1, _env2, _settings.EnvelopeSelection);
			double envSample = env?.Sample ?? 1;

			double sample = WaveformUtility.Evaluate(_settings.Waveform, _phase);
			sample *= _settings.Gain;
			sample *= envSample;
			
			Sample = sample;
		}

		private void UpdateVibrato(LfoInstance vibratoLfo)
		{
			if (vibratoLfo == null)
			{
				_activeFrequency = _baseFrequency;
			}
			else
			{
				double lfoSample = vibratoLfo.Sample;
				var pitchShift = lfoSample * _settings.VibratoSemitone;
				var frequencyMultiplier = Math.Pow(2, pitchShift / 12);
				_activeFrequency = _baseFrequency * frequencyMultiplier;
			}

			UpdatePhaseIncrement();
		}

		private void UpdatePhaseIncrement()
		{
			float octaveShiftValue = GetPowerOfTwoMultiplier(_settings.OctaveShift);
			var adjustedFrequency = _activeFrequency * octaveShiftValue;
			_phaseIncrement = 2 * Math.PI * adjustedFrequency / _sampleRate;
		}

		// Translate integer to power of two for octave shift
		private float GetPowerOfTwoMultiplier(int exponent)
		{
			return exponent >= 0
				? 1 << exponent
				: 1.0f / (1 << -exponent);
		}

		private void UpdatePhase()
		{
			_phase += _phaseIncrement;

			while (_phase > 2 * Math.PI)
				_phase -= 2 * Math.PI;
		}

		public void NoteOn()
		{
			_phase = 0;
		}

		public void NoteOff()
		{
		}
	}
}