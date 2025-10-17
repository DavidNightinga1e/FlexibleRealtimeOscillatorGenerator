using System;
using Runtime.Common;
using Runtime.Test;

namespace Runtime.Synth
{
	public class OscillatorInstance : ISampleProvider
	{
		private readonly int _sampleRate;
		private readonly double _baseFrequency;
		private readonly OscillatorSettings _settings;
		private readonly LfoInstance _lfo1;
		private readonly LfoInstance _lfo2;

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
			LfoInstance lfo2
		)
		{
			_sampleRate = sampleRate;
			_baseFrequency = baseFrequency;
			_settings = settings;
			_lfo1 = lfo1;
			_lfo2 = lfo2;
		}

		public void UpdateSample()
		{
			if (!_settings.Enabled)
			{
				Sample = 0;
				return;
			}
			
			LfoInstance vibratoLfoSelection = _settings.VibratoLfoSelection switch
			{
				LfoSelection.Off => null,
				LfoSelection.Lfo1 => _lfo1,
				LfoSelection.Lfo2 => _lfo2,
				_ => throw new ArgumentOutOfRangeException()
			};
			
			UpdateVibrato(vibratoLfoSelection);

			UpdatePhase();

			Sample = _settings.Gain * WaveformUtility.Evaluate(_settings.Waveform, _phase);
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
				var pitchShift = lfoSample * _settings.VibratoFrequencyShift;
				_activeFrequency = _baseFrequency * pitchShift;
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
	}
}