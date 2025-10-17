namespace Runtime.Test
{
	public class DelayEffect
	{
		private readonly int _sampleRate;
		private readonly double _delayTime;
		private readonly double _feedback;
		private readonly double _mix;
		
		private readonly double[] _buffer;
		private readonly double _dryMix;
		private readonly double _wetMix;
		
		private int _writeIndex;
		private int _readIndex;
		
		public DelayEffect(int sampleRate, double delayTime, double feedback = 0.9, double mix = 0.5)
		{
			_sampleRate = sampleRate;
			_delayTime = delayTime;
			_feedback = feedback;
			_mix = mix;

			int bufferSize = (int)(_sampleRate * _delayTime);
			_buffer = new double[bufferSize];

			_writeIndex = 0;
			_readIndex = (_writeIndex - _buffer.Length) % _buffer.Length;

			_dryMix = 1 - mix; //Math.Sqrt(1 - mix);
			_wetMix = mix; //Math.Sqrt(mix);
		}

		public double Process(double sample)
		{
			var delayedSample = _buffer[_readIndex];
			
			_buffer[_writeIndex] = sample + delayedSample * _feedback;

			var output = sample * _dryMix + delayedSample * _wetMix;
			
			_writeIndex = (_writeIndex + 1) % _buffer.Length;
			_readIndex = (_readIndex + 1) % _buffer.Length;

			return output;
		}
	}
}