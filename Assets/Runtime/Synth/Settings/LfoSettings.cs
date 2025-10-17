using Runtime.Common;

namespace Runtime.Synth
{
	public class LfoSettings : SettingsBase
	{
		public bool Enabled { get; set; }
		public Waveform Waveform { get; set; }
		public double Frequency { get; set; }

		public static LfoSettings CreateDisabled1HzSine() => new()
		{
			Enabled = false,
			Waveform = Waveform.Sine,
			Frequency = 1.0,
		};

		public static LfoSettings CreateDisabled4HzSquare() => new()
		{
			Enabled = false,
			Waveform = Waveform.Square,
			Frequency = 4.0,
		};
	}
}