using System;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class FilterSettings : SettingsBase
	{
		public bool Enabled;

		public FilterType FilterType;
		
		public double Gain;
		public double CutoffFrequency;
		public double QFactor;

		public double KeyTracking;

		public LfoSelection LfoSelection;
		public double LfoAmount;

		public EnvelopeSelection EnvelopeSelection;
		public double EnvelopeAmount;

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