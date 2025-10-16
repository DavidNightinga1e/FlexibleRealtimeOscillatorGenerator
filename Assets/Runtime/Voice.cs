using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Runtime
{
	public class Voice
	{
		public Envelope AmpEnvelope { get; private set; }

		public bool IsReleasing { get; private set; }
		public bool IsFinished => IsReleasing && AmpEnvelope.Value <= 0.000001;

		private readonly List<Oscillator> _oscillators;
		private readonly LowPassFilter _lowPassFilter;
		private readonly Oscillator _lfo;
		private readonly Envelope _filterEnvelope;
		private readonly LowPassFilterModule _lowPassFilterModule;
		private readonly int _sampleRate;

		public Voice(
			List<Oscillator> oscillators,
			LowPassFilterModule filterModule,
			EnvelopeParameters ampEnvelopeParameters,
			[CanBeNull] EnvelopeParameters filterEnvelopeParameters,
			[CanBeNull] Oscillator lfo,
			int sampleRate)
		{
			_sampleRate = sampleRate;
			_oscillators = oscillators;
			AmpEnvelope = new Envelope(sampleRate, ampEnvelopeParameters);
			if (filterModule.master)
			{
				_lowPassFilterModule = filterModule;
				_lowPassFilter = new LowPassFilter(filterModule.cutoff, filterModule.q, sampleRate);

				_lfo = lfo;

				if (filterEnvelopeParameters != null)
					_filterEnvelope = new Envelope(sampleRate, filterEnvelopeParameters);
			}

			Attack();
		}

		public double UpdateSample()
		{
			double oscillatorValue = _oscillators.Sum(t => t.Evaluate()) / _oscillators.Count;
			double filtered = oscillatorValue;
			if (_lowPassFilter != null)
			{
				double filterEnvelope = 1;
				if (_filterEnvelope != null)
				{
					_filterEnvelope.Update();
					filterEnvelope = _filterEnvelope.Value;
				}

				double frequencyShiftFromLfo = 0;
				if (_lfo != null)
				{
					frequencyShiftFromLfo = _lfo.Evaluate() * _lowPassFilterModule.lfoAmount;
				}

				double newCutoffFrequency = _lowPassFilterModule.cutoff + (filterEnvelope * _lowPassFilterModule.envelopeAmount) + frequencyShiftFromLfo;
				_lowPassFilter.SetParameters(
					newCutoffFrequency,
					_lowPassFilterModule.q,
					_sampleRate);

				filtered = _lowPassFilter.Process(oscillatorValue);
			}

			AmpEnvelope.Update();
			return 0.707f * filtered * AmpEnvelope.Value;
		}

		public void Attack()
		{
			IsReleasing = false;
			AmpEnvelope.OnAttack();
			_filterEnvelope?.OnAttack();
		}

		public void Release()
		{
			IsReleasing = true;
			AmpEnvelope.OnRelease();
			_filterEnvelope?.OnRelease();
		}
	}
}