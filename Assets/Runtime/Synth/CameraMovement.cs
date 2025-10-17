using System;
using UnityEngine;

namespace Runtime.Synth
{
	public class CameraMovement : MonoBehaviour
	{
		[SerializeField] private Camera c;
		[SerializeField] private float maxSize;
		[SerializeField] private float minSize;
		[SerializeField] private float sizeChangeMultiplier;

		private void Update()
		{
			float scrollValue = Input.mouseScrollDelta.y;
			if (scrollValue != 0)
			{
				var s = c.orthographicSize + scrollValue * sizeChangeMultiplier;
				c.orthographicSize = Mathf.Clamp(s, minSize, maxSize);
			}

			if (Input.GetMouseButton(2))
			{
				c.transform.position -= new Vector3
				(
					Input.mousePositionDelta.x,
					Input.mousePositionDelta.y,
					0
				);
			}
		}

		private void PanStart()
		{
		}

		private void PanEnd()
		{
		}
	}
}