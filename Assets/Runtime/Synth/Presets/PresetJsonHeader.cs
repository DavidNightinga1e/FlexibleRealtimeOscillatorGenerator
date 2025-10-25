using System;

namespace Runtime.Synth.Presets
{
	// Maybe in future something will change in presets,
	// so it's good to store "version"
	[Serializable]
	public class PresetJsonHeader
	{
		public string Info = "FlexibleRealtimeOscillatorGenerator Preset File";
		public string Version = "1.0"; 
	}
}