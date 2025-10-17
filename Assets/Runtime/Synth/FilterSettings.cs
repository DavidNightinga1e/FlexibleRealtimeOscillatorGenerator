using Runtime.Test;

namespace Runtime.Synth
{
	// Low-pass filter parameters
	public class FilterSettings : SettingsBase
	{
		public bool Enabled { get; set; }

		public double CutoffFrequency { get; set; }
		public double QFactor { get; set; }

		public LfoSelection LfoSelection { get; set; }
		public double LfoFrequencyShift { get; set; }

		public EnvelopeSelection EnvelopeSelection { get; set; }
		public double EnvelopeFrequencyShift { get; set; }

		public static FilterSettings CreateDefault() => new()
		{
			Enabled = false,
			CutoffFrequency = 400,
			QFactor = 0.9,
			LfoSelection = LfoSelection.Off,
			LfoFrequencyShift = 0,
			EnvelopeSelection = EnvelopeSelection.Off,
			EnvelopeFrequencyShift = 0
		};
	}
}