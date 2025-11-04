using System;
using Runtime.Synth;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UI.Keyboard
{
	public class KeyView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
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

		public void OnPointerDown(PointerEventData eventData)
		{
			RaiseKeyViewPointerDown();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			RaiseKeyViewPointerUp();
		}

		private void RaiseKeyViewPointerDown() => KeyViewPointerDownEvent?.Invoke(this);

		private void RaiseKeyViewPointerUp() => KeyViewPointerUpEvent?.Invoke(this);

		[ContextMenu(nameof(SerializeImage))]
		private void SerializeImage()
		{
			image = GetComponent<Image>();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (Input.GetMouseButton(0))
			{
				RaiseKeyViewPointerUp();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Input.GetMouseButton(0))
			{
				RaiseKeyViewPointerDown();
			}
		}
	}
} 