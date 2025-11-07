using System;

namespace Runtime.Synth
{
	public class CombFilter
	{
		private readonly double[] _delayBuffer;
		private readonly int _delaySamples;
		
		private double _damping;
		private double _feedback;
		
		private double _filterState;
		private int _bufferIndex;

		public CombFilter(int delaySamples)
		{
			_delaySamples = delaySamples;
			_delayBuffer = new double[_delaySamples];
			_bufferIndex = 0;
		}

		public void SetFeedback(double value)
		{
			_feedback = value;
		}

		public void SetDamping(double value)
		{
			_damping = value;
		}

		public double Process(double input)
		{
			// Read from delay line
			double output = _delayBuffer[_bufferIndex];
        
			// Apply dampening (simple low-pass)
			_filterState = output * (1 - _damping) + _filterState * _damping;
        
			// Write to delay line
			_delayBuffer[_bufferIndex] = input + _filterState * _feedback;
        
			// Increment and wrap index
			_bufferIndex = (_bufferIndex + 1) % _delaySamples;
        
			return output;
		}
	}
}