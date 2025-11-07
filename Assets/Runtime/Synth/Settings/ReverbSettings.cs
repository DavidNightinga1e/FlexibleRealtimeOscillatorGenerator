using System;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class ReverbSettings : SettingsBase
	{
		public bool Enabled;
		public double RoomSize;
		public double Mix;
		public double Damp;

		public static ReverbSettings CreateDefault() => new()
		{
			Enabled = false,
			RoomSize = 0.8,
			Mix = 0.4,
			Damp = 0.7,
		};
	}
}