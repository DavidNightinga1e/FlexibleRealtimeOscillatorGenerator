namespace Runtime.Synth
{
	public class SynthesizerPreset
	{
		public OscillatorSettings Osc1Settings { get; set; }
		public OscillatorSettings Osc2Settings { get; set; }
		public LfoSettings Lfo1Settings { get; set; }
		public LfoSettings Lfo2Settings { get; set; }
		public FilterSettings FilterSettings { get; set; }
		public EnvelopeSettings AmpSettings { get; set; }
		public EnvelopeSettings Env1Settings { get; set; }
		public EnvelopeSettings Env2Settings { get; set; }
		public DistortSettings DistortSettings { get; set; }
		public DelaySettings DelaySettings { get; set; }
		public ReverbSettings ReverbSettings { get; set; }
	}
}