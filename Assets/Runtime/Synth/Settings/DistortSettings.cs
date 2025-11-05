using System;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class DistortSettings : SettingsBase
	{
		public bool Enabled;
		public double InputGain;
		public double Drive;
		public double Mix;
		public double OutputGain;

		public const double InputGainMinValue = 0.5;
		public const double InputGainMaxValue = 4.0;
		public const double OutputGainMinValue = 0.1;
		public const double OutputGainMaxValue = 2.0;
		public const double DriveMinValue = 1.0;
		public const double DriveMaxValue = 8.0;

		public static DistortSettings CreateDefault() => new()
		{
			Enabled = false,
			InputGain = 1.0,
			OutputGain = 1.0,
			Drive = 2.0,
			Mix = 1,
		};
	}
}