using System;
using System.Collections.Generic;

namespace Runtime
{
	public class Oscillator
	{
		private readonly Waveform _waveform;
		
		private readonly double _phaseIncrement;
		
		private double _phase;
		
		public Oscillator(Waveform waveform, double frequency, int sampleRate, int octaveShift = 0)
		{
			_waveform = waveform;
			
			double adjustedFrequency = frequency * GetPowerOfTwoMultiplier(octaveShift);
			
			_phaseIncrement = 2 * Math.PI * adjustedFrequency / sampleRate;
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

			return _waveform switch
			{
				Waveform.Sine => GetSine(),
				Waveform.Square => GetSquare(),
				Waveform.Triangle => GetTriangle(),
				Waveform.Sawtooth => GetSawtooth(),
				_ => throw new ArgumentOutOfRangeException()
			};
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