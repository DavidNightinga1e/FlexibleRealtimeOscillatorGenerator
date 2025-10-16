using System;

namespace Runtime
{
	public class LowPassFilter
	{
		private double _cutoffFrequency;
		private double _qFactor;
		private int _sampleRate;

		private double _b0;
		private double _b1;
		private double _b2;
		private double _a0;
		private double _a1;
		private double _a2;

		private double _x1;
		private double _x2;
		private double _y1;
		private double _y2;

		public LowPassFilter(double cutoffFrequency, double qFactor, int sampleRate)
		{
			SetParameters(cutoffFrequency, qFactor, sampleRate);
		}

		public void SetParameters(double cutoffFrequency, double qFactor, int sampleRate)
		{
			_cutoffFrequency = cutoffFrequency;
			_qFactor = qFactor;
			_sampleRate = sampleRate;
			CalculateCoefficients();
		}

		private void CalculateCoefficients()
		{
			var w0 = 2 * Math.PI * _cutoffFrequency / _sampleRate;
			var alpha = Math.Sin(w0) / (2 * _qFactor);

			var cosW0 = Math.Cos(w0);

			_b0 = (1 - cosW0) / 2;
			_b1 = 1 - cosW0;
			_b2 = (1 - cosW0) / 2;
			_a0 = 1 + alpha;
			_a1 = -2 * cosW0;
			_a2 = 1 - alpha;

			_b0 /= _a0;
			_b1 /= _a0;
			_b2 /= _a0;
			_a1 /= _a0;
			_a2 /= _a0;
		}

		public void OnAttack()
		{
			//_x1 = _x2 = _y1 = _y2 = 0;
		}

		public double Process(double sample)
		{
			var output =
				_b0 * sample +
				_b1 * _x1 +
				_b2 * _x2 -
				_a1 * _y1 -
				_a2 * _y2;

			_x2 = _x1;
			_x1 = sample;
			_y2 = _y1;
			_y1 = output;
			return output;
		}
	}
}