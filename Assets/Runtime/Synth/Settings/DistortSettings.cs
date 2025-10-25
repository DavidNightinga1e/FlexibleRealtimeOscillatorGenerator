using System;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class DistortSettings : SettingsBase
	{
		public bool Enabled;

		public static DistortSettings CreateDefault() => new()
		{
			Enabled = false
		};
	}
}