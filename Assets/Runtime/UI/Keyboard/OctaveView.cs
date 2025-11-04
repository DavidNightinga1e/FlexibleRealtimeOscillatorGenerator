using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Common;
using UnityEngine;

namespace Runtime.UI.Keyboard
{
	public class OctaveView : MonoBehaviour
	{
		[SerializeField] private List<KeyView> keys;

		private readonly Dictionary<KeyView, LocalNote> _keyViewToNote = new();
		private Dictionary<LocalNote, KeyView> _noteToKeyView;

		public event Action<LocalNote> NoteDownRequestEvent;
		public event Action<LocalNote> NoteUpRequestEvent;

		public void SetNoteDown(LocalNote note)
		{
			_noteToKeyView[note].SetNoteDown();
		}

		public void SetNoteUp(LocalNote note)
		{
			_noteToKeyView[note].SetNoteUp();
		}

		private void Awake()
		{
			if (keys.Count is not 12)
				throw new Exception("KeyView list is not length of 12");

			int i = 0;
			foreach (LocalNote note in NoteUtilities.GetLocalNotes())
			{
				KeyView keyView = keys[i];
				_keyViewToNote[keyView] = note;

				keyView.KeyViewPointerUpEvent += OnKeyViewPointerUp;
				keyView.KeyViewPointerDownEvent += OnKeyViewPointerDown;

				i++;
			}

			_noteToKeyView = _keyViewToNote.ToDictionary
			(
				k => k.Value,
				v => v.Key
			);
		}

		private void OnDestroy()
		{
			foreach (KeyView keyView in keys)
			{
				keyView.KeyViewPointerUpEvent -= OnKeyViewPointerUp;
				keyView.KeyViewPointerDownEvent -= OnKeyViewPointerDown;
			}
		}

		private void OnKeyViewPointerUp(KeyView obj)
		{
			LocalNote note = _keyViewToNote[obj];
			RaiseNoteUpRequest(note);
		}

		private void OnKeyViewPointerDown(KeyView obj)
		{
			LocalNote note = _keyViewToNote[obj];
			RaiseNoteDownRequest(note);
		}

		private void RaiseNoteUpRequest(LocalNote note) => NoteUpRequestEvent?.Invoke(note);

		private void RaiseNoteDownRequest(LocalNote note) => NoteDownRequestEvent?.Invoke(note);

		[ContextMenu(nameof(SerializeKeys_Editor))]
		private void SerializeKeys_Editor()
		{
			keys = GetComponentsInChildren<KeyView>().OrderBy(t => t.name).ToList();
		}
	}
}