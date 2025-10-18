using Runtime.Common;

namespace Runtime.Synth
{
	public class LfoSettings : SettingsBase
	{
		public bool Enabled { get; set; }
		public Waveform Waveform { get; set; }
		public double Frequency { get; set; }

		public static LfoSettings Create1HzSine() => new()
		{
			Enabled = true,
			Waveform = Waveform.Sine,
			Frequency = 1.0,
		};

		public static LfoSettings Create4HzSquare() => new()
		{
			Enabled = true,
			Waveform = Waveform.Square,
			Frequency = 4.0,
		};
	}
}