using Runtime.Test;

namespace Runtime.Synth
{
	// Low-pass filter parameters
	public class LpfSettings : SettingsBase
	{
		public bool Enabled { get; set; }

		public double CutoffFrequency { get; set; }
		public double QFactor { get; set; }

		public LfoSelection LfoSelection { get; set; }
		public double LfoAmount { get; set; }

		public EnvelopeSelection EnvelopeSelection { get; set; }
		public double EnvelopeAmount { get; set; }

		public static LpfSettings CreateDefault() => new()
		{
			Enabled = false,
			CutoffFrequency = 400,
			QFactor = 0.9,
			LfoSelection = LfoSelection.Off,
			LfoAmount = 0,
			EnvelopeSelection = EnvelopeSelection.Off,
			EnvelopeAmount = 0
		};
	}
}