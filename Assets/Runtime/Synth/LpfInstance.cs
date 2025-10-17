using System;
using Runtime.Test;

namespace Runtime.Synth
{
	public class LpfInstance
	{
		private readonly LpfSettings _settings;

		private readonly EnvelopeInstance _env1;
		private readonly EnvelopeInstance _env2;
		private readonly LfoInstance _lfo1;
		private readonly LfoInstance _lfo2;

		public LpfInstance
		(
			LpfSettings lpfSettings,
			EnvelopeInstance env1,
			EnvelopeInstance env2,
			LfoInstance lfo1,
			LfoInstance lfo2
		)
		{
			_settings = lpfSettings;
			_env1 = env1;
			_env2 = env2;
			_lfo1 = lfo1;
			_lfo2 = lfo2;
		}

		// does nothing yet
		public double ProcessSample(double sample)
		{
			EnvelopeInstance targetEnv = _settings.EnvelopeSelection switch
			{
				EnvelopeSelection.Off => null,
				EnvelopeSelection.Env1 => _env1,
				EnvelopeSelection.Env2 => _env2,
				_ => throw new ArgumentOutOfRangeException()
			};
			LfoInstance targetLfo = _settings.LfoSelection switch
			{
				LfoSelection.Off => null,
				LfoSelection.Lfo1 => _lfo1,
				LfoSelection.Lfo2 => _lfo2,
				_ => throw new ArgumentOutOfRangeException()
			};
			return sample;
		}
	}
}