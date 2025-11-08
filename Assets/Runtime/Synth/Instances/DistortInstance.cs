using System;
using System.Collections.Generic;
using Runtime.Common;
using Runtime.Synth.Distort;

namespace Runtime.Synth
{
	public class DistortInstance
	{
		private readonly int _sampleRate;
		private readonly DistortSettings _settings;

		private readonly Dictionary<DistortType, IDistortLogic> _logics = new()
		{
			{ DistortType.SoftClip, new SoftClipDistort() },
			{ DistortType.HardClip, new HardClipDistort() },
			{ DistortType.WaveShape, new WaveDistort() },
			{ DistortType.BitCrush, new BitCrushDistort() },
			{ DistortType.Tube, new TubeDistort() }
		};

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

			double distorted = _logics[_settings.DistortType].Process(sample, _settings.Drive);

			double output = distorted * wetMix + sample * dryMix;

			return output * _settings.OutputGain;
		}
	}
}