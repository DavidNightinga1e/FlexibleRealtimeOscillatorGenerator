using System;
using Runtime.Synth;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UI.Keyboard
{
	// todo хендлинг инпутов мышкой
	public class KeyView : MonoBehaviour
	{
		[SerializeField] private Image image;
		[SerializeField] private Sprite noteOn;
		[SerializeField] private Sprite noteOff;
		
		public event Action<KeyView> KeyViewPointerDownEvent;
		public event Action<KeyView> KeyViewPointerUpEvent;

		public void SetNoteDown()
		{
			image.sprite = noteOn;
		}

		public void SetNoteUp()
		{
			image.sprite = noteOff;
		}

		private void RaiseKeyViewPointerDown() => KeyViewPointerDownEvent?.Invoke(this);

		private void RaiseKeyViewPointerUp() => KeyViewPointerUpEvent?.Invoke(this);

		[ContextMenu(nameof(SerializeImage))]
		private void SerializeImage()
		{
			image = GetComponent<Image>();
		}
	}
} 