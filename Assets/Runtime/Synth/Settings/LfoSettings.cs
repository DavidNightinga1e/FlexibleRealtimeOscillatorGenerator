using System;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class LfoSettings : SettingsBase
	{
		public bool Enabled;
		public Waveform Waveform;
		public double Frequency;
		public double Gain;

		public static LfoSettings Create1HzSine() => new()
		{
			Enabled = true,
			Waveform = Waveform.Sine,
			Frequency = 1.0,
			Gain = 1.0,
		};

		public static LfoSettings Create4HzSquare() => new()
		{
			Enabled = true,
			Waveform = Waveform.Square,
			Frequency = 4.0,
			Gain = 1.0,
		};
	}
}