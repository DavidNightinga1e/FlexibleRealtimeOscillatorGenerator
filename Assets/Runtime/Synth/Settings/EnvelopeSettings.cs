using System;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class EnvelopeSettings : SettingsBase
	{
		public double AttackDuration;
		public double DecayDuration;
		public double SustainValue;
		public double ReleaseDuration;

		public static EnvelopeSettings CreateDefault() => new()
		{
			AttackDuration = 0.1,
			DecayDuration = 0.1,
			SustainValue = 0.8,
			ReleaseDuration = 0.2
		};
	}
}