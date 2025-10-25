namespace Runtime.Synth.Presets
{
	public static class BuiltInPresets
	{
		public static SynthesizerPreset CreateDefault()
		{
			return new SynthesizerPreset
			{
				Osc1Settings = OscillatorSettings.CreateBasicSine(),
				Osc2Settings = OscillatorSettings.CreateDisabledBasicSawtooth(),
				Lfo1Settings = LfoSettings.Create1HzSine(),
				Lfo2Settings = LfoSettings.Create4HzSquare(),
				FilterSettings = FilterSettings.CreateDefault(),
				AmpSettings = EnvelopeSettings.CreateDefault(),
				Env1Settings = EnvelopeSettings.CreateDefault(),
				Env2Settings = EnvelopeSettings.CreateDefault(),
				DistortSettings = DistortSettings.CreateDefault(),
				DelaySettings = DelaySettings.CreateDefault(),
				ReverbSettings = ReverbSettings.CreateDefault()
			};
		}
	}
}