using System;

namespace Runtime.Synth
{
	public class DistortInstance
	{
		private readonly int _sampleRate;
		private readonly DistortSettings _settings;
		
		public DistortInstance
		(
			int sampleRate,
			DistortSettings settings
		)
		{
			_sampleRate = sampleRate;
			_settings = settings;
		}

		public double ProcessSample(double sample)
		{
			if (!_settings.Enabled)
				return sample;

			double gain = 2;
			sample = Math.Tanh(sample * gain);
			return sample;
		}
	}
}