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

			sample *= _settings.InputGain;

			double wetMix = _settings.Mix;
			double dryMix = 1 - wetMix;

			double distorted = (float)(2.0 / Math.PI * Math.Atan(sample * _settings.Drive));

			double output = distorted * wetMix + sample * dryMix;
			
			return output * _settings.OutputGain;
		}
	}
}