using System;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Synth
{
	[Serializable]
	public class OscillatorSettings : SettingsBase
	{
		public bool Enabled;

		public Waveform Waveform;
		public int OctaveShift;
		public double Gain;

		public EnvelopeSelection EnvelopeSelection;

		public LfoSelection VibratoLfoSelection;
		public double VibratoAmountSemitones;
		
		public LfoSelection TremoloLfoSelection;
		public double TremoloDepth;

		public static OscillatorSettings CreateBasicSine() => new()
		{
			Enabled = true,
			Waveform = Waveform.Sine,
			OctaveShift = 0,
			Gain = 1,
			EnvelopeSelection = EnvelopeSelection.Off,
			VibratoLfoSelection = LfoSelection.Off,
			VibratoAmountSemitones = 0,
			TremoloLfoSelection = LfoSelection.Off,
			TremoloDepth = 0,
		};

		public static OscillatorSettings CreateDisabledBasicSawtooth() => new()
		{
			Enabled = false,
			Waveform = Waveform.Sawtooth,
			OctaveShift = 0,
			Gain = 1,
			EnvelopeSelection = EnvelopeSelection.Off,
			VibratoLfoSelection = LfoSelection.Off,
			VibratoAmountSemitones = 0,
			TremoloLfoSelection = LfoSelection.Off,
			TremoloDepth = 0,
		};
	}
}