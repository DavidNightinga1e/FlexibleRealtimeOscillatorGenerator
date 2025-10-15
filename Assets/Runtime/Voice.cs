using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime
{
	public class Voice
	{
		public double Amplitude { get; } = 1f;
		public Envelope Envelope { get; private set; }
		
		public double Frequency { get; }

		public double Phase;
		
		public bool IsReleasing { get; private set; }
		public bool IsFinished => IsReleasing && Envelope.Value <= 0.000001;

		private readonly List<Oscillator> _oscillators;
		private readonly int _sampleRate;
		private readonly LowPassFilter _lowPassFilter;

		public Voice(List<Oscillator> oscillators, LowPassFilter filter, EnvelopeParameters envelopeParameters, int sampleRate, double frequency)
		{
			Frequency = frequency;
			_sampleRate = sampleRate;
			_oscillators = oscillators;
			Envelope = new Envelope(sampleRate, envelopeParameters);
			_lowPassFilter = filter;
			Attack();
		}

		public double UpdateSample()
		{
			double oscillatorValue = _oscillators.Sum(t => t.Evaluate()) / _oscillators.Count;
			double filtered = _lowPassFilter.Process(oscillatorValue);
			Envelope.Update();
			return 0.7f * Amplitude * filtered * Envelope.Value;
		}

		public void Attack()
		{
			IsReleasing = false;
			Envelope.OnAttack();
		}

		public void Release()
		{
			IsReleasing = true;
			Envelope.OnRelease();
		}
	}
}