using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI
{
	public class IntegerKnob : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		[SerializeField] private List<float> rotations;
		[SerializeField] private float rotationMultiplier;
		[SerializeField] private RectTransform knob;

		private Vector2 _dragStartPosition;
		private float _currentRotation;
		
		public int Value { get; private set; }
		public event Action<int> ValueChanged; 

		private void Awake()
		{
			SyncViewToState();
		}

		public void SetValueWithoutNotify(int value)
		{
			Value = value;
			SyncViewToState();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_dragStartPosition = eventData.position;
			_currentRotation = knob.rotation.eulerAngles.z;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			int closestRotationIndex = 0;
			float closestDistance = float.MaxValue;
			for (int i = 0; i < rotations.Count; i++)
			{
				float distance = Mathf.Abs(knob.eulerAngles.z - rotations[i]);
				if (distance < closestDistance)
				{
					closestRotationIndex = i;
					closestDistance = distance;
				}
			}
			
			Value = closestRotationIndex;
			ValueChanged?.Invoke(closestRotationIndex);
			
			SyncViewToState();
		}

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 delta = eventData.position - _dragStartPosition;
			var rotation = delta.x * rotationMultiplier + delta.y * rotationMultiplier;
			knob.eulerAngles = new Vector3(0, 0, _currentRotation + rotation);
		}

		private void SyncViewToState()
		{
			knob.eulerAngles = new Vector3(0, 0, rotations[Value]);
		}
	}
}