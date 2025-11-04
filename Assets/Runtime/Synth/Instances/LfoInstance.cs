using System;
using Runtime.Common;

namespace Runtime.Synth
{
	public class LfoInstance : ISampleProvider, INoteHandler
	{
		private readonly int _sampleRate;
		private readonly LfoSettings _settings;

		private double _phase;
		private double _phaseIncrement;

		public double Sample { get; private set; }

		public LfoInstance
		(
			int sampleRate,
			LfoSettings settings
		)
		{
			_sampleRate = sampleRate;
			_settings = settings;
		}

		public void UpdateSample()
		{
			if (!_settings.Enabled)
			{
				Sample = 0;
				return;
			}

			UpdatePhaseIncrement();
			UpdatePhase();
			
			Sample = _settings.Gain * WaveformUtility.Evaluate(_settings.Waveform, _phase);
		}

		private void UpdatePhase()
		{
			_phase += _phaseIncrement;

			while (_phase > 2 * Math.PI)
				_phase -= 2 * Math.PI;
		}

		private void UpdatePhaseIncrement()
		{
			_phaseIncrement = 2 * Math.PI * _settings.Frequency / _sampleRate;
		}

		public void NoteDown()
		{
			_phase = 0;
		}

		public void NoteUp()
		{
		}
	}
}