using System;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class DelaySettings : SettingsBase
	{
		public bool Enabled;
		public double DelayTime;
		public double Feedback;
		public double Mix;

		public static DelaySettings CreateDefault() => new()
		{
			Enabled = false,
			DelayTime = 0.2,
			Feedback = 0.6,
			Mix = 0.5
		};
	}
}