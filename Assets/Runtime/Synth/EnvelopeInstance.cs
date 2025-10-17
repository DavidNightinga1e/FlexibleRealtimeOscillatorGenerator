using System;
using Runtime.Common;

namespace Runtime.Synth
{
	public class EnvelopeInstance : INoteHandler, ISampleProvider
	{
		public double Sample { get; private set; }

		private readonly EnvelopeSettings _settings;
		private readonly int _sampleRate;

		private EnvelopeState _state = EnvelopeState.Attack;

		private double _attackRate;
		private double _decayRate;
		private double _releaseRate;

		public EnvelopeInstance
		(
			int sampleRate,
			EnvelopeSettings settings
		)
		{
			_sampleRate = sampleRate;
			_settings = settings;
		}

		public void NoteOn()
		{
			_state = EnvelopeState.Attack;

			_attackRate = (1 - Sample) / (_settings.AttackDuration * _sampleRate);
			_decayRate = (1 - _settings.SustainValue) / (_settings.DecayDuration * _sampleRate);
		}

		public void NoteOff()
		{
			_state = EnvelopeState.Release;

			_releaseRate = Sample / (_settings.ReleaseDuration * _sampleRate);
		}

		public void UpdateSample()
		{
			switch (_state)
			{
				case EnvelopeState.Attack:
					UpdateAttack();
					break;
				case EnvelopeState.Decay:
					UpdateDecay();
					break;
				case EnvelopeState.Sustain:
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
			Sample += _attackRate;
			if (Sample < 1)
				return;

			Sample = 1;
			_state = EnvelopeState.Decay;
		}

		private void UpdateDecay()
		{
			Sample -= _decayRate;
			if (Sample > _settings.SustainValue)
				return;

			Sample = _settings.SustainValue;
			_state = EnvelopeState.Sustain;
		}

		private void UpdateRelease()
		{
			Sample -= _releaseRate;
			if (Sample < 0)
				Sample = 0;
		}
	}
}