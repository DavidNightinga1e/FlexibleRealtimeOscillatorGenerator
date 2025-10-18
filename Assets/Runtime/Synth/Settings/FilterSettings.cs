using Runtime.Common;
using Runtime.Test;

namespace Runtime.Synth
{
	public class FilterSettings : SettingsBase
	{
		public bool Enabled { get; set; }

		public FilterType FilterType { get; set; }
		
		public double Gain { get; set; }
		public double CutoffFrequency { get; set; }
		public double QFactor { get; set; }

		public double KeyTracking { get; set; }

		public LfoSelection LfoSelection { get; set; }
		public double LfoAmount { get; set; }

		public EnvelopeSelection EnvelopeSelection { get; set; }
		public double EnvelopeAmount { get; set; }

		public static FilterSettings CreateDefault() => new()
		{
			Enabled = true,
			Gain = 1.0,
			FilterType = FilterType.LowPass,
			CutoffFrequency = 400,
			QFactor = 0.9,
			KeyTracking = 1.0,
			LfoSelection = LfoSelection.Off,
			LfoAmount = 0,
			EnvelopeSelection = EnvelopeSelection.Off,
			EnvelopeAmount = 0
		};
	}
}