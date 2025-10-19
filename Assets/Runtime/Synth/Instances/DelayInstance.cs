namespace Runtime.Synth
{
	public class DelayInstance
	{
		private readonly int _sampleRate;
		private readonly DelaySettings _delaySettings;

		private readonly double[] _buffer;
		
		private const int MaxDelayTimeSeconds = 2;
		
		private int _writeIndex;
		
		public DelayInstance(int sampleRate, DelaySettings delaySettings)
		{
			_sampleRate = sampleRate;
			_delaySettings = delaySettings;
			
			_buffer = new double[_sampleRate * MaxDelayTimeSeconds];
		}

		public double ProcessSample(double sample)
		{
			if (!_delaySettings.Enabled)
				return sample;
			
			var delayedSample = _buffer[_writeIndex];

			_buffer[_writeIndex] = sample + delayedSample * _delaySettings.Feedback;
			
			var output = sample * (1 - _delaySettings.Mix) + delayedSample * _delaySettings.Mix;
			
			_writeIndex = (_writeIndex + 1) % (int)(_sampleRate * _delaySettings.DelayTime);
			
			return output;
		}
	}
}