using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
	public class Keyboard : MonoBehaviour
	{
		private Dictionary<KeyCode, Note> _keyCodeToNote = new Dictionary<KeyCode, Note>
		{
			{ KeyCode.A, Note.C3 },
			{ KeyCode.S, Note.D3 },
			{ KeyCode.D, Note.E3 },
			{ KeyCode.F, Note.F3 },
			{ KeyCode.G, Note.G3 },
			{ KeyCode.H, Note.A3 },
			{ KeyCode.J, Note.B3 },
			
			{ KeyCode.Q, Note.C4 },
			{ KeyCode.W, Note.D4 },
			{ KeyCode.E, Note.E4 },
			{ KeyCode.R, Note.F4 },
			{ KeyCode.T, Note.G4 },
			{ KeyCode.Y, Note.A4 },
			{ KeyCode.U, Note.B4 },
		};
		
		public event Action<Note> OnNotePressed; 
		public event Action<Note> OnNoteReleased; 

		private void Update()
		{
			foreach (var pair in _keyCodeToNote)
			{
				if (Input.GetKeyDown(pair.Key))
				{
					OnNotePressed?.Invoke(pair.Value);
				}

				if (Input.GetKeyUp(pair.Key))
				{
					OnNoteReleased?.Invoke(pair.Value);
				}
			}
		}
	}
}