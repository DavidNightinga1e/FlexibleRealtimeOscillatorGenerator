using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
	public class Toggle : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private Image on;
		[SerializeField] private Image off;
		
		public bool Value { get; private set; }
		public event Action<bool> ValueChanged; 

		public void SetValueWithoutNotify(bool value)
		{
			Value = value;
			SyncViewToState();
		}

		private void Awake()
		{
			button.onClick.AddListener(OnButtonClick);
			SyncViewToState();
		}

		private void SyncViewToState()
		{
			on.enabled = Value;
			off.enabled = !Value;
		}

		private void OnButtonClick()
		{
			Value = !Value;
			SyncViewToState();
			ValueChanged?.Invoke(Value);
		}
	}
}