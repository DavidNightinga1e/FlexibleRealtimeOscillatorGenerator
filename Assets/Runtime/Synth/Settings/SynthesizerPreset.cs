using System;
using Runtime.Synth.Presets;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class SynthesizerPreset
	{
		public PresetJsonHeader Header;
		public OscillatorSettings Osc1Settings;
		public OscillatorSettings Osc2Settings;
		public LfoSettings Lfo1Settings;
		public LfoSettings Lfo2Settings;
		public FilterSettings FilterSettings;
		public EnvelopeSettings AmpSettings;
		public EnvelopeSettings Env1Settings;
		public EnvelopeSettings Env2Settings;
		public DistortSettings DistortSettings;
		public DelaySettings DelaySettings;
		public ReverbSettings ReverbSettings;

		public static SynthesizerPreset FromJson(string json)
		{
			return JsonUtility.FromJson<SynthesizerPreset>(json);
		}

		public string ToJson()
		{
			return JsonUtility.ToJson(this, true);
		}
	}
}