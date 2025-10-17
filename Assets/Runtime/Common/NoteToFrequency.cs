using System.Collections.Generic;

namespace Runtime.Common
{
	public static class NoteToFrequency
	{
		private static readonly Dictionary<Note, double> _noteFrequency = new Dictionary<Note, double>
		{
			{ Note.C0, 16.35 },
			{ Note.CSharp0, 17.32 },
			{ Note.D0, 18.35 },
			{ Note.DSharp0, 19.45 },
			{ Note.E0, 20.60 },
			{ Note.F0, 21.83 },
			{ Note.FSharp0, 23.12 },
			{ Note.G0, 24.50 },
			{ Note.GSharp0, 25.96 },
			{ Note.A0, 27.50 },
			{ Note.ASharp0, 29.14 },
			{ Note.B0, 30.87 },

			{ Note.C1, 32.70 },
			{ Note.CSharp1, 34.65 },
			{ Note.D1, 36.71 },
			{ Note.DSharp1, 38.89 },
			{ Note.E1, 41.20 },
			{ Note.F1, 43.65 },
			{ Note.FSharp1, 46.25 },
			{ Note.G1, 49.00 },
			{ Note.GSharp1, 51.91 },
			{ Note.A1, 55.00 },
			{ Note.ASharp1, 58.27 },
			{ Note.B1, 61.74 },

			{ Note.C2, 65.41 },
			{ Note.CSharp2, 69.30 },
			{ Note.D2, 73.42 },
			{ Note.DSharp2, 77.78 },
			{ Note.E2, 82.41 },
			{ Note.F2, 87.31 },
			{ Note.FSharp2, 92.50 },
			{ Note.G2, 98.00 },
			{ Note.GSharp2, 103.83 },
			{ Note.A2, 110.00 },
			{ Note.ASharp2, 116.54 },
			{ Note.B2, 123.47 },

			{ Note.C3, 130.81 },
			{ Note.CSharp3, 138.59 },
			{ Note.D3, 146.83 },
			{ Note.DSharp3, 155.56 },
			{ Note.E3, 164.81 },
			{ Note.F3, 174.61 },
			{ Note.FSharp3, 185.00 },
			{ Note.G3, 196.00 },
			{ Note.GSharp3, 207.65 },
			{ Note.A3, 220.00 },
			{ Note.ASharp3, 233.08 },
			{ Note.B3, 246.94 },

			{ Note.C4, 261.63 },
			{ Note.CSharp4, 277.18 },
			{ Note.D4, 293.66 },
			{ Note.DSharp4, 311.13 },
			{ Note.E4, 329.63 },
			{ Note.F4, 349.23 },
			{ Note.FSharp4, 369.99 },
			{ Note.G4, 392.00 },
			{ Note.GSharp4, 415.30 },
			{ Note.A4, 440.00 },
			{ Note.ASharp4, 466.16 },
			{ Note.B4, 493.88 },

			{ Note.C5, 523.25 },
			{ Note.CSharp5, 554.37 },
			{ Note.D5, 587.33 },
			{ Note.DSharp5, 622.25 },
			{ Note.E5, 659.25 },
			{ Note.F5, 698.46 },
			{ Note.FSharp5, 739.99 },
			{ Note.G5, 783.99 },
			{ Note.GSharp5, 830.61 },
			{ Note.A5, 880.00 },
			{ Note.ASharp5, 932.33 },
			{ Note.B5, 987.77 },

			{ Note.C6, 1046.50 },
			{ Note.CSharp6, 1108.73 },
			{ Note.D6, 1174.66 },
			{ Note.DSharp6, 1244.51 },
			{ Note.E6, 1318.51 },
			{ Note.F6, 1396.91 },
			{ Note.FSharp6, 1479.98 },
			{ Note.G6, 1567.98 },
			{ Note.GSharp6, 1661.22 },
			{ Note.A6, 1760.00 },
			{ Note.ASharp6, 1864.66 },
			{ Note.B6, 1975.53 },

			{ Note.C7, 2093.00 },
			{ Note.CSharp7, 2217.46 },
			{ Note.D7, 2349.32 },
			{ Note.DSharp7, 2489.02 },
			{ Note.E7, 2637.02 },
			{ Note.F7, 2793.83 },
			{ Note.FSharp7, 2959.96 },
			{ Note.G7, 3135.96 },
			{ Note.GSharp7, 3322.44 },
			{ Note.A7, 3520.00 },
			{ Note.ASharp7, 3729.31 },
			{ Note.B7, 3951.07 },

			{ Note.C8, 4186.01 }
		};

		public static double GetFrequency(Note note)
		{
			return _noteFrequency[note];
		}
	}
}