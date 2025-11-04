using System;
using System.Collections.Generic;
using Runtime.UI.Keyboard;
using UnityEngine;

namespace Runtime.Common
{
	public class Keyboard : MonoBehaviour
	{
		[SerializeField] private OctaveView firstOctaveView;
		[SerializeField] private OctaveView secondOctaveView;

		public event Action<Note> NoteDownEvent;
		public event Action<Note> NoteUpEvent;

		public Note baseNote = Note.C3;
		public int SelectedOctave = 2;

		private HashSet<Note> _heldNotes = new();

		private void Awake()
		{
			firstOctaveView.NoteDownRequestEvent += OnFirstOctaveNoteDownRequest;
			secondOctaveView.NoteDownRequestEvent += OnSecondOctaveNoteDownRequest;
			firstOctaveView.NoteUpRequestEvent += OnFirstOctaveNoteUpRequest;
			secondOctaveView.NoteUpRequestEvent += OnSecondOctaveNoteUpRequest;
		}

		private void OnFirstOctaveNoteDownRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(SelectedOctave, obj);
			RequestNoteDown(note);
		}

		private void OnFirstOctaveNoteUpRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(SelectedOctave, obj);
			RequestNoteUp(note);
		}

		private void OnSecondOctaveNoteDownRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(SelectedOctave + 1, obj);
			RequestNoteDown(note);
		}

		private void OnSecondOctaveNoteUpRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(SelectedOctave + 1, obj);
			RequestNoteUp(note);
		}

		private void RequestNoteUp(Note obj)
		{
			if (!_heldNotes.Contains(obj))
				return;
			
			SetNoteUp(obj);
			RaiseNoteUp(obj);
		}

		private void RequestNoteDown(Note obj)
		{
			if (_heldNotes.Contains(obj))
				return;

			SetNoteDown(obj);
			RaiseNoteDown(obj);
		}

		private void SetNoteUp(Note obj)
		{
			_heldNotes.Remove(obj);

			OctaveView octaveView = SelectOctaveViewByNote(obj);
			octaveView.SetNoteUp(NoteUtilities.NoteToLocalNote(obj));
		}

		private void SetNoteDown(Note obj)
		{
			_heldNotes.Add(obj);

			OctaveView octaveView = SelectOctaveViewByNote(obj);
			octaveView.SetNoteDown(NoteUtilities.NoteToLocalNote(obj));
		}

		private OctaveView SelectOctaveViewByNote(Note note)
		{
			int octave = NoteUtilities.GetOctave(note);
			int octaveFromBase = octave - SelectedOctave;

			return octaveFromBase switch
			{
				0 => firstOctaveView,
				1 => secondOctaveView,
				_ => throw new ArgumentOutOfRangeException
				(
					$"Note {note} was not in range of currently selected octaves " +
					$"{SelectedOctave}, {SelectedOctave + 1}"
				)
			};
		}

		private Note TransposeFromBaseNote(Note note, int additionalOffset = 0)
		{
			return baseNote + (int)note + additionalOffset;
		}

		private void Update()
		{
			foreach (var pair in KeyboardInputUtility.GetKeyboardNoteOffsetPairs())
			{
				if (Input.GetKeyDown(pair.Key))
				{
					RequestNoteDown(TransposeFromBaseNote((Note)pair.Value));
				}

				if (Input.GetKeyUp(pair.Key))
				{
					RequestNoteUp(TransposeFromBaseNote((Note)pair.Value));
				}
			}

			if (Input.GetKeyDown(KeyCode.Equals))
				OffsetSelectedOctave(+1);
			if (Input.GetKeyDown(KeyCode.Minus))
				OffsetSelectedOctave(-1);
		}

		private void OffsetSelectedOctave(int direction)
		{
			SelectedOctave += direction;
			SelectedOctave = Math.Clamp(SelectedOctave, 0, 5); // todo consts
			baseNote = NoteUtilities.LocalToNote(SelectedOctave, LocalNote.C);
		}

		private void OnDestroy()
		{
			firstOctaveView.NoteDownRequestEvent -= OnFirstOctaveNoteDownRequest;
			secondOctaveView.NoteDownRequestEvent -= OnSecondOctaveNoteDownRequest;
			firstOctaveView.NoteUpRequestEvent -= OnFirstOctaveNoteUpRequest;
			secondOctaveView.NoteUpRequestEvent -= OnSecondOctaveNoteUpRequest;
		}

		private void RaiseNoteDown(Note obj) => NoteDownEvent?.Invoke(obj);
		private void RaiseNoteUp(Note obj) => NoteUpEvent?.Invoke(obj);
	}
}