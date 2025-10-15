using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime
{
	public class VUMeter
	{
		private readonly double _sampleRate;
		private readonly double _attackCoeff;
		private readonly double _releaseCoeff;
		private double _rms;
		private double _peak;
		private readonly Queue<double> _buffer = new Queue<double>();
		private readonly int _bufferSize;

		public VUMeter(int sampleRate = 44100, double attackTime = 0.05, double releaseTime = 0.1, int bufferSize = 1024)
		{
			_sampleRate = sampleRate;
			_attackCoeff = Math.Exp(-1.0 / (sampleRate * attackTime));
			_releaseCoeff = Math.Exp(-1.0 / (sampleRate * releaseTime));
			_bufferSize = bufferSize;
			Reset();
		}

		public void ProcessSample(double sample)
		{
			// Добавляем sample в буфер для RMS расчета по окну
			_buffer.Enqueue(sample);
			if (_buffer.Count > _bufferSize)
			{
				_buffer.Dequeue();
			}

			// RMS детектор (по скользящему окну)
			var squared = sample * sample;
			if (squared > _rms)
			{
				_rms = _attackCoeff * _rms + (1 - _attackCoeff) * squared;
			}
			else
			{
				_rms = _releaseCoeff * _rms + (1 - _releaseCoeff) * squared;
			}

			// Peak детектор
			var absSample = Math.Abs(sample);
			if (absSample > _peak)
			{
				_peak = _attackCoeff * _peak + (1 - _attackCoeff) * absSample;
			}
			else
			{
				_peak = _releaseCoeff * _peak + (1 - _releaseCoeff) * absSample;
			}
		}

		public void ProcessBuffer(IEnumerable<double> samples)
		{
			foreach (var sample in samples)
			{
				ProcessSample(sample);
			}
		}

		public void ProcessBuffer(float[] samples)
		{
			foreach (var sample in samples)
			{
				ProcessSample(sample);
			}
		}

		public double GetRmsDbfs()
		{
			if (_rms <= 0 || double.IsNaN(_rms))
				return double.NegativeInfinity;

			var rmsValue = Math.Sqrt(_rms);
			return 20 * Math.Log10(rmsValue);
		}

		public double GetPeakDbfs()
		{
			if (_peak <= 0 || double.IsNaN(_peak))
				return double.NegativeInfinity;

			return 20 * Math.Log10(_peak);
		}

		public double GetRmsDbfsWindowed()
		{
			// RMS по текущему окну буфера
			if (!_buffer.Any())
				return double.NegativeInfinity;

			var sumSquares = _buffer.Sum(sample => sample * sample);
			var rms = Math.Sqrt(sumSquares / _buffer.Count);
			return 20 * Math.Log10(rms);
		}

		public void Reset()
		{
			_rms = 0.0;
			_peak = 0.0;
			_buffer.Clear();
		}

		public double Rms => _rms;
		public double Peak => _peak;
	}
}