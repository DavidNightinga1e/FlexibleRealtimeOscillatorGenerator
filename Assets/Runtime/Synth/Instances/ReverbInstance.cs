using System;

namespace Runtime.Synth
{
	public class ReverbInstance
	{
		private int _writePosition;
		private int _bufferSize;
		private double _feedback;
		
		private readonly double[] _delayBuffer;
		
		private readonly ReverbSettings _settings;
		private readonly int _sampleRate;

		public ReverbInstance
		(
			int sampleRate,
			ReverbSettings reverbSettings
		)
		{
			_sampleRate = sampleRate;
			_settings = reverbSettings;
			
			_delayBuffer = new double[(int)(_sampleRate * ReverbSettings.MaxDecayTime)];
		}

		public double ProcessSample(double sample)
		{
			if (!_settings.Enabled)
				return sample;
			
			OnSettingsUpdate();

			// Read from delay buffer (with interpolation for smoother sound)
			int readPosition = (_writePosition - _bufferSize / 2 + _bufferSize) % _bufferSize;
			double delayed = _delayBuffer[readPosition];

			// Write input + feedback to delay buffer
			_delayBuffer[_writePosition] = sample + delayed * _feedback;

			// Increment and wrap write position
			_writePosition = (_writePosition + 1) % _bufferSize;

			// Mix dry and wet signals
			return sample * (1 - _settings.Mix) + delayed * _settings.Mix;
		}

		// Move that to SettingsChanged event callback when IDisposable is implemented
		private void OnSettingsUpdate()
		{
			_bufferSize = (int)(_settings.DecayTime * _sampleRate);
			_feedback = Math.Pow(0.001, 1.0 / (_settings.DecayTime * _sampleRate / _bufferSize));
		}
	}
}