using Runtime.Common;

namespace Runtime.Synth
{
	public class Voice : INoteHandler, ISampleProvider
	{
		private const double VoiceGain = 0.707;
		
		private readonly int _sampleRate;
		private readonly double _baseFrequency;
		private readonly OscillatorSettings _osc1Settings;
		private readonly OscillatorSettings _osc2Settings;
		private readonly LfoSettings _lfo1Settings;
		private readonly LfoSettings _lfo2Settings;
		private readonly FilterSettings _filterSettings;
		private readonly EnvelopeSettings _ampSettings;
		private readonly EnvelopeSettings _env1Settings;
		private readonly EnvelopeSettings _env2Settings;

		private OscillatorInstance _osc1;
		private OscillatorInstance _osc2;
		private LfoInstance _lfo1;
		private LfoInstance _lfo2;
		private FilterInstance _filter;
		private EnvelopeInstance _amp;
		private EnvelopeInstance _env1;
		private EnvelopeInstance _env2;

		public double Sample { get; private set; }
		
		public double AmpEnvelopeValue => _amp.Sample;
		public bool IsFinished => _amp.State is EnvelopeState.Release && _amp.Sample < 0.001;

		public Voice
		(
			int sampleRate,
			double baseFrequency,
			OscillatorSettings osc1Settings,
			OscillatorSettings osc2Settings,
			LfoSettings lfo1Settings,
			LfoSettings lfo2Settings,
			FilterSettings filterSettings,
			EnvelopeSettings ampSettings,
			EnvelopeSettings env1Settings,
			EnvelopeSettings env2Settings
		)
		{
			_sampleRate = sampleRate;
			_baseFrequency = baseFrequency;
			_osc1Settings = osc1Settings;
			_osc2Settings = osc2Settings;
			_lfo1Settings = lfo1Settings;
			_lfo2Settings = lfo2Settings;
			_filterSettings = filterSettings;
			_ampSettings = ampSettings;
			_env1Settings = env1Settings;
			_env2Settings = env2Settings;

			CreateInstances();
		}

		private void CreateInstances()
		{
			_amp = new EnvelopeInstance(_sampleRate, _ampSettings);
			_env1 = new EnvelopeInstance(_sampleRate, _env1Settings);
			_env2 = new EnvelopeInstance(_sampleRate, _env2Settings);

			_lfo1 = new LfoInstance(_sampleRate, _lfo1Settings);
			_lfo2 = new LfoInstance(_sampleRate, _lfo2Settings);

			_filter = new FilterInstance(_filterSettings, _env1, _env2, _lfo1, _lfo2);

			_osc1 = new OscillatorInstance(_sampleRate, _baseFrequency, _osc1Settings, _lfo1, _lfo2);
			_osc2 = new OscillatorInstance(_sampleRate, _baseFrequency, _osc2Settings, _lfo1, _lfo2);
		}

		public void NoteOn()
		{
			_amp.NoteOn();
			_env1.NoteOn();
			_env2.NoteOn();
			_osc1.NoteOn();
			_osc2.NoteOn();
		}

		public void NoteOff()
		{
			_amp.NoteOff();
			_env1.NoteOff();
			_env2.NoteOff();
			_osc1.NoteOff();
			_osc2.NoteOff();
		}

		public void UpdateSample()
		{
			// Update envelopes
			_amp.UpdateSample();
			_env1.UpdateSample();
			_env2.UpdateSample();

			// Update LFOs first because they have no dependencies
			_lfo1.UpdateSample();
			_lfo2.UpdateSample();

			// Update OSC that might use LFOs or envelopes
			_osc1.UpdateSample();
			_osc2.UpdateSample();

			double sample = _osc1.Sample + _osc2.Sample;
			if (_osc1Settings.Enabled && _osc2Settings.Enabled)
				sample /= 2;
			
			_filter.ProcessSample(sample);

			Sample = VoiceGain * sample * _amp.Sample;
		}
	}
}