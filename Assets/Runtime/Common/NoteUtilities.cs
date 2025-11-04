using System.Collections.Generic;

namespace Runtime.Common
{
	public static class NoteUtilities
	{
		public const int OctaveNoteCount = 12;

		public static int GetOctave(Note note)
		{
			return (int)note / OctaveNoteCount;
		}
		
		public static Note LocalToNote(int octave, LocalNote localNote)
		{
			return (Note)(OctaveNoteCount * octave + localNote);
		}

		public static LocalNote NoteToLocalNote(Note note)
		{
			return (LocalNote)((int)note % OctaveNoteCount);
		}

		public static IEnumerable<LocalNote> GetLocalNotes()
		{
			yield return LocalNote.C;
			yield return LocalNote.CSharp;
			yield return LocalNote.D;
			yield return LocalNote.DSharp;
			yield return LocalNote.E;
			yield return LocalNote.F;
			yield return LocalNote.FSharp;
			yield return LocalNote.G;
			yield return LocalNote.GSharp;
			yield return LocalNote.A;
			yield return LocalNote.ASharp;
			yield return LocalNote.B;
		}
	}
}