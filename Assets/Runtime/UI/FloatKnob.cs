using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UI
{
	public class FloatKnob : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private TextMeshProUGUI labelText;
		[SerializeField] private RectTransform knob;
		[SerializeField] private Image valueFill;
		[SerializeField] private float rotationMultiplier;
		[Range(0, 1), SerializeField] private float valueDebug;

		[SerializeField] private Color defaultLabelColor;
		[SerializeField] private Color valueLabelColor;

		private float _dragStartValue;
		private Vector2 _dragStartPosition;
		private float _valueShowEndTime;
		
		public float Value { get; private set; }
		public event Action<float> ValueChanged;

		private string _defaultLabelString;
		private string _valueText;
		private bool _isHovering;
		private bool _isDragging;

		private void Awake()
		{
			_defaultLabelString = labelText.text;
			SyncViewToState();
		}

		public void SetValueWithoutNotify(float value)
		{
			Value = value;
			SyncViewToState();
		}

		private void Update()
		{
			if (!_isHovering && !_isDragging)
			{
				labelText.text = _defaultLabelString;
				labelText.color = defaultLabelColor;
			}
			else
			{
				labelText.text = _valueText;
				labelText.color = valueLabelColor;
			}
		}

		public void SetValueText(string t)
		{
			_valueText = t;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_dragStartPosition = eventData.position;
			_dragStartValue = Value;

			_isDragging = true;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			_isDragging = false;
		}

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 delta = eventData.position - _dragStartPosition;
			var rotation = delta.x * rotationMultiplier + delta.y * rotationMultiplier;
			Value = Mathf.Clamp01(_dragStartValue + rotation);
			ValueChanged?.Invoke(Value);
			SyncViewToState();
		}

		private void SyncViewToState()
		{
			valueFill.fillAmount = Mathf.Lerp(0.1f, 0.9f, Value);
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
		
		public void OnPointerExit(PointerEventData eventData)
		{
			_isHovering = false;
		}
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!Input.GetMouseButton(0))
				_isHovering = true;
		}
	}
}