using Runtime.Common;
using Runtime.Test;

namespace Runtime.Synth
{
	public class OscillatorSettings : SettingsBase
	{
		public bool Enabled { get; set; }

		public Waveform Waveform { get; set; }
		public int OctaveShift { get; set; }
		public double Gain { get; set; }

		public EnvelopeSelection EnvelopeSelection { get; set; }

		public LfoSelection VibratoLfoSelection { get; set; }
		public double VibratoSemitone { get; set; }

		public static OscillatorSettings CreateBasicSine() => new()
		{
			Enabled = true,
			Waveform = Waveform.Sine,
			OctaveShift = 0,
			Gain = 1,
			EnvelopeSelection = EnvelopeSelection.Off,
			VibratoLfoSelection = LfoSelection.Off,
			VibratoSemitone = 0
		};

		public static OscillatorSettings CreateDisabledBasicSquare() => new()
		{
			Enabled = false,
			Waveform = Waveform.Square,
			OctaveShift = 0,
			Gain = 1,
			EnvelopeSelection = EnvelopeSelection.Off,
			VibratoLfoSelection = LfoSelection.Off,
			VibratoSemitone = 0
		};
	}
}