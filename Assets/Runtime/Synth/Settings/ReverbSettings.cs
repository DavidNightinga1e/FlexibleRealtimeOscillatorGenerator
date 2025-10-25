using System;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class ReverbSettings : SettingsBase
	{
		public bool Enabled;
		public double DecayTime;
		public double Mix;
		
		public const double MaxDecayTime = 2.0;

		public static ReverbSettings CreateDefault() => new()
		{
			Enabled = false,
			DecayTime = 0.2,
			Mix = 0.5
		};
	}
}