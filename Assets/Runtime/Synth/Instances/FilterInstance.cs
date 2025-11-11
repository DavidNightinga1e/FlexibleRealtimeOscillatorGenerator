using System;
using Runtime.Common;
using Runtime.Test;

namespace Runtime.Synth
{
	public class FilterInstance
	{
		private readonly int _sampleRate;
		private readonly double _baseFrequency;
		private readonly FilterSettings _settings;

		private readonly EnvelopeInstance _env1;
		private readonly EnvelopeInstance _env2;
		private readonly LfoInstance _lfo1;
		private readonly LfoInstance _lfo2;

		private double _x1, _x2, _y1, _y2;
		private double _b0, _b1, _b2, _a1, _a2;

		private double _cutoffFrequency;

		public FilterInstance
		(
			int sampleRate,
			double baseFrequency,
			FilterSettings filterSettings,
			EnvelopeInstance env1,
			EnvelopeInstance env2,
			LfoInstance lfo1,
			LfoInstance lfo2
		)
		{
			_sampleRate = sampleRate;
			_baseFrequency = baseFrequency;
			_settings = filterSettings;
			_env1 = env1;
			_env2 = env2;
			_lfo1 = lfo1;
			_lfo2 = lfo2;
		}

		public double ProcessSample(double sample)
		{
			if (!_settings.Enabled)
				return sample;

			ModulateCutoff();

			double output =
				_b0 * sample
				+ _b1 * _x1
				+ _b2 * _x2
				- _a1 * _y1
				- _a2 * _y2;

			_x2 = _x1;
			_x1 = sample;
			_y2 = _y1;
			_y1 = output;

			return _settings.Gain * output;
		}

		private void ModulateCutoff()
		{
			EnvelopeInstance targetEnv = SelectorUtilities.SelectEnvelope(_env1, _env2, _settings.EnvelopeSelection);
			LfoInstance targetLfo = SelectorUtilities.SelectLfo(_lfo1, _lfo2, _settings.LfoSelection);

			double lfoValue = targetLfo?.Sample ?? 0.0;
			double envValue = targetEnv?.Sample ?? 0.0;

			double modulatedCutoff = _settings.CutoffFrequency;

			modulatedCutoff *= Math.Pow(2.0, lfoValue * _settings.LfoAmount * 4.0);

			modulatedCutoff += envValue * _settings.EnvelopeAmount * 8000.0;
			
			modulatedCutoff += _settings.KeyTracking * _baseFrequency;

			_cutoffFrequency = Math.Clamp(modulatedCutoff, 20.0, _sampleRate * 0.45);

			CalculateCoefficients();
		}

		private void CalculateCoefficients()
		{
			double omega = 2.0 * Math.PI * _cutoffFrequency / _sampleRate;
			double sinOmega = Math.Sin(omega);
			double cosOmega = Math.Cos(omega);
			double alpha = sinOmega / (2.0 * (_settings.QFactor + 0.00001));

			double b0, b1, b2;
			
			double a0 = 1.0 + alpha;
			double a1 = -2.0 * cosOmega;
			double a2 = 1.0 - alpha;

			switch (_settings.FilterType)
			{
				case FilterType.LowPass:
					b0 = (1.0 - cosOmega) / 2.0;
					b1 = 1.0 - cosOmega;
					b2 = b0;
					break;

				case FilterType.HighPass:
					b0 = (1.0 + cosOmega) / 2.0;
					b1 = -(1.0 + cosOmega);
					b2 = b0;
					break;

				case FilterType.BandPass:
					b0 = alpha;
					b1 = 0;
					b2 = -alpha;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			_b0 = b0 / a0;
			_b1 = b1 / a0;
			_b2 = b2 / a0;
			_a1 = a1 / a0;
			_a2 = a2 / a0;
		}
	}
}