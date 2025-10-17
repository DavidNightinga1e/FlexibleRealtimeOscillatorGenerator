using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI
{
	public class FloatKnob : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private RectTransform knob;
		[SerializeField] private float rotationMultiplier;
		[Range(0, 1), SerializeField] private float valueDebug;

		private float _dragStartValue;
		private Vector2 _dragStartPosition;
		
		public float Value { get; private set; }

		private void Awake()
		{
			SyncViewToState();
		}

		public void SetValueText(string t)
		{
			valueText.text = t;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_dragStartPosition = eventData.position;
			_dragStartValue = Value;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
		}

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 delta = eventData.position - _dragStartPosition;
			var rotation = delta.x * rotationMultiplier + delta.y * rotationMultiplier;
			Value = Mathf.Clamp01(_dragStartValue + rotation);
			SyncViewToState();
		}

		private void SyncViewToState()
		{
			if (Value <= 0.5)
			{
				// [0, 1] -> [135, 0]
				// actual value range [0, 0.5]
				knob.eulerAngles = Vector3.forward * Mathf.Lerp(135f, 0f, Value * 2f);
			}
			else
			{
				// [0, 1] -> [360, 225]
				// actual value range [0.5, 1]
				knob.eulerAngles = Vector3.forward * Mathf.Lerp(360f, 225f, (Value - 0.5f) * 2f);
			}
		}
	}
}