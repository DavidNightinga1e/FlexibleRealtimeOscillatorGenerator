using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.UI.Keyboard;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Runtime.Common
{
	public class Keyboard : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI octaveText;
		[SerializeField] private Button upOctaveButton;
		[SerializeField] private Button downOctaveButton;
		[SerializeField] private OctaveView firstOctaveView;
		[SerializeField] private OctaveView secondOctaveView;

		public event Action<Note> NoteDownEvent;
		public event Action<Note> NoteUpEvent;
		
		private const int DefaultSelectedOctave = 3;

		private Note _baseNote;
		private int _selectedOctave = DefaultSelectedOctave;

		private readonly HashSet<Note> _heldNotes = new();

		private void Awake()
		{
			firstOctaveView.NoteDownRequestEvent += OnFirstOctaveNoteDownRequest;
			secondOctaveView.NoteDownRequestEvent += OnSecondOctaveNoteDownRequest;
			firstOctaveView.NoteUpRequestEvent += OnFirstOctaveNoteUpRequest;
			secondOctaveView.NoteUpRequestEvent += OnSecondOctaveNoteUpRequest;
			
			upOctaveButton.onClick.AddListener(() => OffsetSelectedOctave(+1));
			downOctaveButton.onClick.AddListener(() => OffsetSelectedOctave(-1));

			_baseNote = NoteUtilities.LocalToNote(_selectedOctave, LocalNote.C);
			octaveText.text = _selectedOctave.ToString();
		}

		private void OnFirstOctaveNoteDownRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(_selectedOctave, obj);
			RequestNoteDown(note);
		}

		private void OnFirstOctaveNoteUpRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(_selectedOctave, obj);
			RequestNoteUp(note);
		}

		private void OnSecondOctaveNoteDownRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(_selectedOctave + 1, obj);
			RequestNoteDown(note);
		}

		private void OnSecondOctaveNoteUpRequest(LocalNote obj)
		{
			Note note = NoteUtilities.LocalToNote(_selectedOctave + 1, obj);
			RequestNoteUp(note);
		}

		private void RequestNoteUp(Note obj)
		{
			if (!_heldNotes.Contains(obj))
				return;
			
			SetNoteUp(obj);
		}

		private void RequestNoteDown(Note obj)
		{
			if (_heldNotes.Contains(obj))
				return;

			SetNoteDown(obj);
		}

		private void SetNoteUp(Note obj)
		{
			_heldNotes.Remove(obj);

			OctaveView octaveView = SelectOctaveViewByNote(obj);
			octaveView.SetNoteUp(NoteUtilities.NoteToLocalNote(obj));
			
			RaiseNoteUp(obj);
		}

		private void SetNoteDown(Note obj)
		{
			_heldNotes.Add(obj);

			OctaveView octaveView = SelectOctaveViewByNote(obj);
			octaveView.SetNoteDown(NoteUtilities.NoteToLocalNote(obj));
			
			RaiseNoteDown(obj);
		}

		private OctaveView SelectOctaveViewByNote(Note note)
		{
			int octave = NoteUtilities.GetOctave(note);
			int octaveFromBase = octave - _selectedOctave;

			return octaveFromBase switch
			{
				0 => firstOctaveView,
				1 => secondOctaveView,
				_ => throw new ArgumentOutOfRangeException
				(
					$"Note {note} was not in range of currently selected octaves " +
					$"{_selectedOctave}, {_selectedOctave + 1}"
				)
			};
		}

		private Note TransposeFromBaseNote(Note note, int additionalOffset = 0)
		{
			return _baseNote + (int)note + additionalOffset;
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
			foreach (Note note in _heldNotes.ToList()) 
				SetNoteUp(note);
			
			_selectedOctave += direction;
			_selectedOctave = Math.Clamp(_selectedOctave, 0, 5); // todo consts
			_baseNote = NoteUtilities.LocalToNote(_selectedOctave, LocalNote.C);
			
			octaveText.text = _selectedOctave.ToString();
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