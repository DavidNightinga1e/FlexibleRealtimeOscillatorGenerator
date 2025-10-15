using System.Collections.Generic;

namespace Runtime
{
	public static class NoteToFrequency
	{
		private static readonly Dictionary<Note, double> _noteFrequency = new Dictionary<Note, double>
		{
			{Note.C3, 130.81},
			{Note.D3, 146.83},
			{Note.E3, 164.81},
			{Note.F3, 174.61},
			{Note.G3, 196.00},
			{Note.A3, 220.00},
			{Note.B3, 246.94},
			
			{Note.C4, 261.63},
			{Note.D4, 293.66},
			{Note.E4, 329.63},
			{Note.F4, 349.23},
			{Note.G4, 392.00},
			{Note.A4, 440.00},
			{Note.B4, 493.88},
		};

		public static double GetFrequency(Note note)
		{
			return _noteFrequency[note];
		}
	}
}