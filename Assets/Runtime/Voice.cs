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
		
		private readonly List<Oscillator> _oscillators;
		private readonly int _sampleRate;

		public Voice(List<Oscillator> oscillators, EnvelopeParameters envelopeParameters, int sampleRate, double frequency)
		{
			Frequency = frequency;
			_sampleRate = sampleRate;
			_oscillators = oscillators;
			Envelope = new Envelope(sampleRate, envelopeParameters);
			Attack();
		}

		public double UpdateSample()
		{
			Envelope.Update();
			double oscillatorValue = _oscillators.Sum(t => t.Evaluate()) / _oscillators.Count;
			return 0.7f * Amplitude * oscillatorValue * Envelope.Value;
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