using System;

namespace Runtime.Common
{
	public static class WaveformUtility
	{
		public static double Evaluate(Waveform waveform, double phase) => waveform switch
		{
			Waveform.Sine => Sine(phase),
			Waveform.Square => Square(phase),
			Waveform.Triangle => Triangle(phase),
			Waveform.Sawtooth => Sawtooth(phase),
			_ => throw new ArgumentOutOfRangeException()
		};

		public static double Sawtooth(double phase)
		{
			return phase / Math.PI - 1;
		}

		public static double Triangle(double phase)
		{
			if (phase < Math.PI)
				return -1 + (2 * phase / Math.PI);
			return 3 - (2 * phase / Math.PI);
		}

		public static int Square(double phase)
		{
			return phase < Math.PI ? 1 : -1;
		}

		public static double Sine(double phase)
		{
			return Math.Sin(phase);
		}
	}
}