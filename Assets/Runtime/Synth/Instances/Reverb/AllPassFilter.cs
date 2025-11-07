namespace Runtime.Synth
{
	public class AllPassFilter
	{
		private readonly double[] _delayBuffer;
		private readonly int _delaySamples;
		
		private double _feedback;

		private int _bufferIndex;

		public AllPassFilter(int delaySamples)
		{
			_delaySamples = delaySamples;
			_delayBuffer = new double[_delaySamples];
			_bufferIndex = 0;
		}

		public void SetFeedback(double feedback)
		{
			_feedback = feedback;
		}

		public double Process(double input)
		{
			// Read from delay line
			double delayed = _delayBuffer[_bufferIndex];

			// All-pass filter equation
			double output = -input + delayed;

			// Write to delay line
			_delayBuffer[_bufferIndex] = input + delayed * _feedback;

			// Increment and wrap index
			_bufferIndex = (_bufferIndex + 1) % _delaySamples;

			return output;
		}
	}
}