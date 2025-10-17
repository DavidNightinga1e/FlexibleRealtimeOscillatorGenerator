﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Common
{
	public class Keyboard : MonoBehaviour
	{
		private static readonly Dictionary<KeyCode, int> _keyboardNoteOffset = new()
		{
			// Первый ряд (нижний) - белые клавиши: Z X C V B N M
			{KeyCode.Z, 0},  // C
			{KeyCode.X, 2},  // D
			{KeyCode.C, 4},  // E
			{KeyCode.V, 5},  // F
			{KeyCode.B, 7},  // G
			{KeyCode.N, 9},  // A
			{KeyCode.M, 11}, // B
    
			// Первый ряд (нижний) - черные клавиши: S D G H J
			{KeyCode.S, 1},  // C#
			{KeyCode.D, 3},  // D#
			{KeyCode.G, 6},  // F#
			{KeyCode.H, 8},  // G#
			{KeyCode.J, 10}, // A#
    
			// Второй ряд (верхний) - белые клавиши: Q W E R T Y U I O P
			{KeyCode.Q, 12}, // C (октавой выше)
			{KeyCode.W, 14}, // D
			{KeyCode.E, 16}, // E
			{KeyCode.R, 17}, // F
			{KeyCode.T, 19}, // G
			{KeyCode.Y, 21}, // A
			{KeyCode.U, 23}, // B
			{KeyCode.I, 24}, // C (еще октавой выше)
			{KeyCode.O, 26}, // D
			{KeyCode.P, 28}, // E
    
			// Второй ряд (верхний) - черные клавиши: 2 3 5 6 7 9 0
			{KeyCode.Alpha2, 13}, // C#
			{KeyCode.Alpha3, 15}, // D#
			{KeyCode.Alpha5, 18}, // F#
			{KeyCode.Alpha6, 20}, // G#
			{KeyCode.Alpha7, 22}, // A#
			{KeyCode.Alpha9, 25}, // C# (октавой выше)
			{KeyCode.Alpha0, 27}, // D#
		};
		
		public event Action<Note> OnNotePressed; 
		public event Action<Note> OnNoteReleased;

		public Note baseNote;

		private const int BaseNoteOffsetUp = 12;
		private const int BaseNoteOffsetDown = -BaseNoteOffsetUp;

		private void Update()
		{
			foreach (var pair in _keyboardNoteOffset)
			{
				if (Input.GetKeyDown(pair.Key))
				{
					OnNotePressed?.Invoke((Note)((int)baseNote + pair.Value));
				}

				if (Input.GetKeyUp(pair.Key))
				{
					OnNoteReleased?.Invoke((Note)((int)baseNote + pair.Value));
				}
			}

			if (Input.GetKeyDown(KeyCode.Equals)) 
				OffsetBaseNoteClamped(BaseNoteOffsetUp);
			if (Input.GetKeyDown(KeyCode.Minus))
				OffsetBaseNoteClamped(BaseNoteOffsetDown);
		}

		private void OffsetBaseNoteClamped(int offset)
		{
			int newBaseNoteInt = (int)baseNote + offset;
			newBaseNoteInt = Math.Clamp(newBaseNoteInt, (int)Note.LowestBaseNote, (int)Note.TopBaseNote);
			baseNote = (Note)newBaseNoteInt;
		}
	}
}