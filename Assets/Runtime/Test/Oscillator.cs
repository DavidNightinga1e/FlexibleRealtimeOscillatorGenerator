using System;
using Runtime.Common;

namespace Runtime.Test
{
	public class Oscillator
	{
		private readonly Waveform _waveform;
		
		private double _phaseIncrement;
		private double _phase;
		private double _activeFrequency;

		private readonly Oscillator _vibratoLfo;
		private readonly float _vibratoAmount;
		private readonly double _frequency;
		private readonly int _octaveShift;
		private readonly int _sampleRate;
		
		public Oscillator(Waveform waveform, double frequency, int sampleRate, Oscillator vibratoLfo, float vibratoAmount, int octaveShift = 0)
		{
			_waveform = waveform;
			
			_sampleRate = sampleRate;
			_octaveShift = octaveShift;
			_activeFrequency = _frequency = frequency;
			
			_vibratoLfo = vibratoLfo;
			_vibratoAmount = vibratoAmount;
			
			UpdatePhaseIncrement();
		}

		private void UpdatePhaseIncrement()
		{
			double adjustedFrequency = _activeFrequency * GetPowerOfTwoMultiplier(_octaveShift);
			_phaseIncrement = 2 * Math.PI * adjustedFrequency / _sampleRate;
		}
		
		public static float GetPowerOfTwoMultiplier(int exponent)
		{
			return exponent >= 0 
				? 1 << exponent 
				: 1.0f / (1 << -exponent);
		}
		
		public double Evaluate()
		{
			_phase += _phaseIncrement;
			
			while (_phase > 2 * Math.PI)
				_phase -= 2 * Math.PI;

			if (_vibratoLfo != null)
			{
				double vibratoLfoValue = _vibratoLfo.Evaluate();
				var pitchShift = vibratoLfoValue * _vibratoAmount;
				var frequencyMultiplier = Math.Pow(2, pitchShift / 12);
				_activeFrequency = _frequency * frequencyMultiplier;
				UpdatePhaseIncrement();
			}

			double output = _waveform switch
			{
				Waveform.Sine => GetSine(),
				Waveform.Square => GetSquare(),
				Waveform.Triangle => GetTriangle(),
				Waveform.Sawtooth => GetSawtooth(),
				_ => throw new ArgumentOutOfRangeException()
			};
			
			return output;
		}

		private double GetSawtooth()
		{
			return _phase / Math.PI - 1;
		}

		private double GetTriangle()
		{
			if (_phase < Math.PI)
				return -1 + (2 * _phase / Math.PI);
			return 3 - (2 * _phase / Math.PI);
		}

		private int GetSquare()
		{
			return _phase < Math.PI ? 1 : -1;
		}

		private double GetSine()
		{
			return Math.Sin(_phase);
		}
	}
}