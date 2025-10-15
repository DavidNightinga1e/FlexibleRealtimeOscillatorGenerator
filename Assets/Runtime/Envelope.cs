using System;
using UnityEngine;

namespace Runtime
{
	public class Envelope
	{
		public double Value;

		private readonly int _sampleRate;
		private readonly EnvelopeParameters _parameters;

		private double _attackRate;
		private double _decayRate;
		private double _releaseRate;

		public EnvelopeState State { get; private set; } = EnvelopeState.Attack;

		public Envelope(int sampleRate, EnvelopeParameters parameters)
		{
			_sampleRate = sampleRate;
			_parameters = parameters;

			OnAttack();
		}

		public void OnAttack()
		{
			State = EnvelopeState.Attack;

			_attackRate = (1 - Value) / (_parameters.Attack * _sampleRate);
			_decayRate = (1 - _parameters.Sustain) / (_parameters.Decay * _sampleRate);
		}

		public void OnRelease()
		{
			State = EnvelopeState.Release;
			_releaseRate = Value / (_parameters.Release * _sampleRate);
		}

		public void Update()
		{
			switch (State)
			{
				case EnvelopeState.Attack:
					UpdateAttack();
					break;
				case EnvelopeState.Decay:
					UpdateDecay();
					break;
				case EnvelopeState.Sustain:
					UpdateSustain();
					break;
				case EnvelopeState.Release:
					UpdateRelease();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void UpdateAttack()
		{
			Value += _attackRate;
			if (Value > 1)
			{
				Value = 1;
				State = EnvelopeState.Decay;
			}
		}

		private void UpdateDecay()
		{
			Value -= _decayRate;
			if (Value < _parameters.Sustain)
			{
				Value = _parameters.Sustain;
				State = EnvelopeState.Sustain;
			}
		}

		private void UpdateSustain()
		{
		}

		private void UpdateRelease()
		{
			Value -= _releaseRate;
			if (Value < 0)
				Value = 0;
		}
	}
}